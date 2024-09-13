using HRIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IProcessRepository
    {
        Task<Process?> GetProcessByLeaveRequestIdAsync(int leaveRequestId);
        Task AddProcessAsync(Process process);
        Task AddWorkflowActionAsync(WorkflowActions action);
        Task SaveChangesAsync();
        Task<WorkflowSequences?> GetNextStepAsync(int currentStepId, string conditionType);
        Task<IEnumerable<WorkflowActions>> GetWorkflowActionsByLeaveRequestIdAsync(int leaveRequestId);

        Task<IEnumerable<Process>> GetProcessBasedOnRoleAsync(string role);


    }

}
