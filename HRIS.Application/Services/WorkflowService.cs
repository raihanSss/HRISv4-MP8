using HRIS.Domain;
using HRIS.Domain.Interfaces;
using HRIS.Domain.Models;
using HRIS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HRIS.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkflowService(IProcessRepository processRepository, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _processRepository = processRepository;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> ProcessLeaveRequestAsync(int leaveRequestId, int userId, string comments, string status)
        {
            
            var process = await _processRepository.GetProcessByLeaveRequestIdAsync(leaveRequestId);

            if (process == null)
            {
                return false; 
            }

            var action = new WorkflowActions
            {
                ProcessId = process.ProcessId,
                StepId = process.CurrentStepId,
                ActorId = userId,
                Action = status.Equals("approve", StringComparison.OrdinalIgnoreCase) ? "Approve" : "Reject",
                ActionDate = DateTime.UtcNow,
                Comments = comments
            };

            await _processRepository.AddWorkflowActionAsync(action);

            var userRoles = _httpContextAccessor.HttpContext?.User?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

            bool isEmployeeSupervisor = userRoles.Contains("Employee Supervisor");
            bool isHRManager = userRoles.Contains("HR Manager");


            if (status.Equals("approve", StringComparison.OrdinalIgnoreCase))
            {
                if (isEmployeeSupervisor)
                {
                    
                    process.CurrentStepId = 3; 
                    process.Status = "Need Review from HR Manager"; 
                }
                else if (isHRManager)
                {
                    
                    process.CurrentStepId = 4; 
                    process.Status = "Approved"; 
                }

               
                var requesterEmail = "raihansss34@gmail.com"; 
                var mailData = new MailData
                {
                    EmailToId = requesterEmail,
                    EmailToName = "Raihan",
                    EmailSubject = process.Status == "Approved" ? "Leave Request Approved" : "Leave Request Needs Further Review",
                    EmailBody = process.Status == "Approved"
                        ? "Your leave request has been approved."
                        : "Your leave request has been forwarded for further review."
                };
                await _emailService.SendMail(mailData);


                if (process.CurrentStepId == 3)
                {
                    var hrManagerEmail = "vanquish00vip@gmail.com"; 
                    var mailDataNextRole = new MailData
                    {
                        EmailToId = hrManagerEmail,
                        EmailToName = "HR Manager",
                        EmailSubject = "New Leave Request Approval Required",
                        EmailBody = "A leave request has been approved by the supervisor and now requires your action."
                    };
                    await _emailService.SendMail(mailDataNextRole);
                }
            }
            else if (status.Equals("reject", StringComparison.OrdinalIgnoreCase))
            {
                process.CurrentStepId = 5;
                process.Status = "Rejected";

                
                var requesterEmail = "raihansss34@gmail.com"; 
                var mailData = new MailData
                {
                    EmailToId = requesterEmail,
                    EmailToName = "Raihan",
                    EmailSubject = "Leave Request Rejected",
                    EmailBody = "Your leave request has been rejected."
                };
                await _emailService.SendMail(mailData);
            }
            await _processRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkflowActions>> GetWorkflowActionsAsync(int leaveRequestId)
        {
            return await _processRepository.GetWorkflowActionsByLeaveRequestIdAsync(leaveRequestId);
        }

    }
}
