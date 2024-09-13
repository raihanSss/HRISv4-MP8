using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    [Table("leave_requests")]
    public class LeaveRequest
    {
        [Key]
        [Column("id_leave")]
        public int IdLeave { get; set; }

        [Column("id_emp")]
        public int EmployeeId { get; set; }

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        [Column("leave_type")]
        [StringLength(50)]
        public string LeaveType { get; set; } = null!;

        [Column("reason")]
        public string? Reason { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("LeaveRequests")]
        public virtual Employees Employee { get; set; } = null!;

        public virtual ICollection<Process> Processes { get; set; } = new List<Process>();
    }
}
