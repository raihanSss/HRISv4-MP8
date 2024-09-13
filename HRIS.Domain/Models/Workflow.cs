using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    [Table("Workflow")]
    public class Workflow
    {
        [Key]
        public int WorkflowId { get; set; }

        [Required]
        [StringLength(255)]
        public string WorkflowName { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<WorkflowSequences> WorkflowSequences { get; set; } = new List<WorkflowSequences>();
        public virtual ICollection<Process> Processes { get; set; } = new List<Process>();
    }
}
