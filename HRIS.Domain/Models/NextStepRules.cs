using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    [Table("NextStepRules")]
    public class NextStepRules
    {
        [Key]
        public int RuleId { get; set; }

        [ForeignKey("CurrentStep")]
        public int CurrentStepId { get; set; }
        public virtual WorkflowSequences CurrentStep { get; set; }

        [ForeignKey("NextStep")]
        public int NextStepId { get; set; }
        public virtual WorkflowSequences NextStep { get; set; }

        [Required]
        [StringLength(255)]
        public string ConditionType { get; set; }

        [StringLength(255)]
        public string? ConditionValue { get; set; }

       
    }
}
