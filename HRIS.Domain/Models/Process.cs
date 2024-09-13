using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    [Table("Process")]
    public class Process
    {
        [Key]
        public int ProcessId { get; set; }

        [ForeignKey("Workflow")]
        public int WorkflowId { get; set; }
        public virtual Workflow Workflow { get; set; }

        [ForeignKey("LeaveRequest")]
        public int LeaveRequestId { get; set; }
        public virtual LeaveRequest LeaveRequest { get; set; }

        [Required]
        [StringLength(255)]
        public string RequestType { get; set; } = "LeaveRequest";

        [Required]
        [StringLength(255)]
        public string Status { get; set; }

        [ForeignKey("CurrentStep")]
        public int CurrentStepId { get; set; }
        public virtual WorkflowSequences CurrentStep { get; set; }

        public DateTime RequestDate { get; set; }

        public virtual ICollection<WorkflowActions> WorkflowActions { get; set; } = new List<WorkflowActions>();
    }
}
