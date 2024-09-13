using HRIS.Domain;
using HRIS.Domain.Dtos;
using HRIS.Domain.Interfaces;
using HRIS.Domain.Models;
using HRIS.Infrastructure;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace HRIS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProcessRepository _processRepository;
        private readonly IEmailService _emailService;

        public EmployeeService(IEmployeeRepository employeeRepository, IProcessRepository processRepository, IEmailService emailService)
        {
            _employeeRepository = employeeRepository;
            _processRepository = processRepository;
            _emailService = emailService;
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task<IEnumerable<object>> GetAllEmployeesAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string sortBy = "NameEmp",
        bool sortDesc = false,
        string filterBy = "",
        string filterValue = "")
        {
            return await _employeeRepository.GetAllEmployeesAsync(pageNumber, pageSize, sortBy, sortDesc, filterBy, filterValue);
        }

        public async Task<string> AddEmployeeAsync(Employees employee)
        {
            await _employeeRepository.AddEmployeeAsync(employee);
                return "Data employee berhasil dtambah";
        }

        public async Task<bool> CreateLeaveRequestAsync(LeaveReqDto leaveRequestDto)
        {
            // Membuat leave request berdasarkan data yang diterima
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = leaveRequestDto.EmployeeId,
                StartDate = leaveRequestDto.StartDate,
                EndDate = leaveRequestDto.EndDate,
                LeaveType = leaveRequestDto.LeaveType,
                Reason = leaveRequestDto.Reason
            };


            var leaveRequestAdded = await _employeeRepository.AddLeaveRequestAsync(leaveRequest);

            if (!leaveRequestAdded)
            {
                return false;
            }


            var process = new Process
            {
                WorkflowId = 1,
                LeaveRequestId = leaveRequest.IdLeave,
                RequestType = leaveRequest.LeaveType,
                Status = "Pending",
                CurrentStepId = 2,
                RequestDate = DateTime.UtcNow
            };


            await _processRepository.AddProcessAsync(process);


            var initialAction = new WorkflowActions
            {
                ProcessId = process.ProcessId,
                StepId = 1,
                ActorId = leaveRequestDto.EmployeeId,
                Action = "Created",
                ActionDate = DateTime.UtcNow,
                Comments = "leave request created."
            };


            await _processRepository.AddWorkflowActionAsync(initialAction);
            await _processRepository.SaveChangesAsync();

            var requesterEmail = "raihansss34@gmail.com";
            var supervisorEmail = "vanquish00vip@gmail.com";
            var mailData = new MailData
            {
                EmailToId = requesterEmail,
                EmailToName = "Raihan",
                EmailSubject = "Leave Request Submitted",
                EmailBody = $"Your leave request has been submitted and is awaiting approval."
            };
            await _emailService.SendMail(mailData);

            var mailDataSupervisor = new MailData
            {
                EmailToId = supervisorEmail,
                EmailToName = "Supervisor Name",
                EmailSubject = "New Leave Request for Approval",
                EmailBody = $"A new leave request has been submitted by {leaveRequestDto.EmployeeId}. Please review and approve."
            };
            await _emailService.SendMail(mailDataSupervisor);

            return true;

        }

        public async Task<IEnumerable<LeaveReqDto>> GetLeaveRequestsByEmployeeIdAsync(int employeeId)
        {
            var leaveRequests = await _employeeRepository.GetLeaveRequestsByEmployeeIdAsync(employeeId);

            return leaveRequests.Select(lr => new LeaveReqDto
            {
                EmployeeId = lr.EmployeeId,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                LeaveType = lr.LeaveType,
                Reason = lr.Reason
            }).ToList();
        }


        public async Task<string> UpdateEmployeeAsync(Employees employee)
        {
            await _employeeRepository.UpdateEmployeeAsync(employee);
            return "Data employee berhasil di update";
        }

        
        public async Task<string> DeleteEmployeeAsync(int id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
            return "Data employee berhasil di hapus";
        }

        public async Task<string> DeactivateEmployeeAsync(int id, string reason)
        {
            try
            {
                await _employeeRepository.DeactivateEmployeeAsync(id, reason);
                return "Employee deactivated successfully";
            }
            catch (KeyNotFoundException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }



        public async Task<byte[]> GenerateEmployeeReportPdfAsync(int departmentId, int pageNumber)
        {
            int pageSize = 20;
            var employeeList = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId, pageNumber, pageSize);
            var departmentName = await _employeeRepository.GetDepartmentNameByIdAsync(departmentId);
            var totalEmployees = await _employeeRepository.GetTotalEmployeesByDepartmentAsync(departmentId);

            string htmlContent = "<h1> Employee Report </h1>";
            htmlContent += $"<h2> Department: {departmentName} - Page: {pageNumber} </h2>";
            htmlContent += "<table>";
            htmlContent += "<tr>" +

                "<th>Id</th>" +
                "<th>Name</th>" +
                "<th>Position</th>" +
                "<th>Email</th>" +
                "</tr>";

            employeeList.ForEach(item =>
            {
                htmlContent += "<tr>";
                htmlContent += $"<td>{item.IdEmp}</td>";
                htmlContent += $"<td>{item.NameEmp}</td>";
                htmlContent += $"<td>{item.JobPosition}</td>";
                htmlContent += $"<td>{item.Email}</td>";
                htmlContent += "</tr>";
            });

            htmlContent += "</table>";

            var document = new PdfDocument();
            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Landscape,
                PageSize = PageSize.A4
            };


            string cssStr = File.ReadAllText(@"./Templates/Css/Style.css");
            CssData css = PdfGenerator.ParseStyleSheet(cssStr);


            PdfGenerator.AddPdfPages(document, htmlContent, config, css);


            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }


            public async Task<byte[]> GenerateLeaveReportPdfAsync(DateOnly startDate, DateOnly endDate)
            {
                var leaveReports = await _employeeRepository.GetTotalLeavesByTypeAsync(startDate, endDate);

                string htmlContent = "<h1> Leave Report </h1>";
                htmlContent += $"<h2> From {startDate} to {endDate} </h2>";
            if (!leaveReports.Any())
            {
                
                htmlContent += "<p>Tidak ada data</p>";
            }
            else
            {
                htmlContent += "<table>";
                htmlContent += "<tr><th>Leave Type</th><th>Total Leaves</th></tr>";

                leaveReports.ForEach(report =>
                {
                    htmlContent += "<tr>";
                    htmlContent += $"<td>{report.LeaveType}</td>";
                    htmlContent += $"<td>{report.TotalLeaves}</td>";
                    htmlContent += "</tr>";
                });

                htmlContent += "</table>";
            }
            
                var document = new PdfDocument();
                var config = new PdfGenerateConfig
                {
                    PageOrientation = PageOrientation.Portrait,
                    PageSize = PageSize.A4
                };

               
                string cssStr = File.ReadAllText(@"./Templates/Css/Style.css"); 
                CssData css = PdfGenerator.ParseStyleSheet(cssStr);

                
                PdfGenerator.AddPdfPages(document, htmlContent, config, css);

                
                using (MemoryStream stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray(); 
                }
            
            }

        public async Task<byte[]> GenerateProjectReportPdfAsync(int projectId)
        {
            var projectReport = await _employeeRepository.GetProjectReportAsync(projectId);

            string htmlContent = "<h1> Project Report </h1>";
            htmlContent += $"<h2> Project: {projectReport.ProjectName} (ID: {projectReport.ProjectId}) </h2>";

            if (projectReport.TotalHoursLogged == 0 && projectReport.NumberOfEmployeesInvolved == 0)
            {
                
                htmlContent += "<p>Tidak ada data</p>";
            }
            else
            {
                htmlContent += "<table>";
                htmlContent += "<tr>" +
                    "<th>Total Hours</th>" +
                    "<th>Employees Involved</th>" +
                    "<th>Average Hours per Employee</th>" +
                    "</tr>";

                htmlContent += "<tr>";
                htmlContent += $"<td>{projectReport.TotalHoursLogged}</td>";
                htmlContent += $"<td>{projectReport.NumberOfEmployeesInvolved}</td>";
                htmlContent += $"<td>{projectReport.AverageHoursPerEmployee:F2}</td>";
                htmlContent += "</tr>";

                htmlContent += "</table>";
            }
            
            var document = new PdfDocument();
            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Portrait,
                PageSize = PageSize.A4
            };

            
            string cssStr = File.ReadAllText(@"./Templates/Css/Style.css");
            CssData css = PdfGenerator.ParseStyleSheet(cssStr);

            
            PdfGenerator.AddPdfPages(document, htmlContent, config, css);

            
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }



        public async Task<IEnumerable<LeaveReport>> GetTotalLeavesByTypeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _employeeRepository.GetTotalLeavesByTypeAsync(startDate, endDate);
        }

        public async Task<ProjectReport> GetProjectReportAsync(int projectId)
        {
            return await _employeeRepository.GetProjectReportAsync(projectId);
        }

        public async Task<IEnumerable<Employees>> GetEmployeesByDepartmentAsync(int departmentId, int pageNumber, int pageSize)
        {
            return await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId, pageNumber, pageSize);
        }
    }
    }


