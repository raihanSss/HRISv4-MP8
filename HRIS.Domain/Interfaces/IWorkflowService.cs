using HRIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IWorkflowService
    {
        Task<bool> ProcessLeaveRequestAsync(int leaveRequestId, int userId, string comments, string status);
        Task<IEnumerable<WorkflowActions>> GetWorkflowActionsAsync(int leaveRequestId);
    }
}
