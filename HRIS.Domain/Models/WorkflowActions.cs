using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    [Table("WorkflowActions")]
    public class WorkflowActions
    {
        [Key]
        public int ActionId { get; set; }

        [ForeignKey("Process")]
        public int ProcessId { get; set; }
        public virtual Process Process { get; set; }

        [ForeignKey("Step")]
        public int StepId { get; set; }
        public virtual WorkflowSequences Step { get; set; }

        [ForeignKey("Actor")]
        public int ActorId { get; set; }

        [Required]
        [StringLength(255)]
        public string Action { get; set; }

        public DateTime ActionDate { get; set; }

        public string? Comments { get; set; }
    }
}
