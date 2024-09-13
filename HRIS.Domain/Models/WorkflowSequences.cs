using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HRIS.Domain.Models
{
    [Table("WorkflowSequences")]
    public class WorkflowSequences
    {
        [Key]
        public int StepId { get; set; }

        [ForeignKey("Workflow")]
        public int WorkflowId { get; set; }
        public virtual Workflow Workflow { get; set; }

        public int StepOrder { get; set; }

        [Required]
        [StringLength(255)]
        public string StepName { get; set; }

        [StringLength(255)]
        [ForeignKey("Role")]
        public string? RequiredRole { get; set; }

        public virtual ICollection<Process> Processes { get; set; } = new List<Process>();
        public virtual ICollection<NextStepRules> NextStepRules { get; set; } = new List<NextStepRules>();
        public virtual ICollection<WorkflowActions> WorkflowActions { get; set; } = new List<WorkflowActions>();
        
        public virtual IdentityRole Role { get; set; }
    }
}
