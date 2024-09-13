using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRIS.Infrastructure;

[Table("employees")]
public partial class Employees
{
    [Key]
    [Column("id_emp")]
    public int IdEmp { get; set; }

    [Column("ssn")]
    [StringLength(11)]
    public string Ssn { get; set; } = null!;

    [Column("name_emp")]
    [StringLength(100)]
    public string NameEmp { get; set; } = null!;

    [Column("sex")]
    [StringLength(10)]
    public string? Sex { get; set; }

    [Column("dob")]
    public DateOnly Dob { get; set; }

    [Column("id_dept")]
    public int? IdDept { get; set; }

    [Column("id_role")]
    public string? IdRole { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(15)]
    public string? Phone { get; set; }

    [Column("job_position")]
    [StringLength(100)]
    public string? JobPosition { get; set; }

    [Column("level")]
    [StringLength(50)]
    public string? Level { get; set; }

    [Column("type")]
    [StringLength(50)]
    public string? Type { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("reason")]
    public string? Reason { get; set; }

    [Column("salary", TypeName = "decimal(18, 2)")] 
    public decimal? Salary { get; set; }

    [Column("lastupdate", TypeName = "timestamp without time zone")]
    public DateTime Lastupdate
    {
        get => _lastupdate;
        set => _lastupdate = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
    }
    private DateTime _lastupdate;

    [InverseProperty("HeadempNavigation")]
    public virtual ICollection<Departments> Departments { get; set; } = new List<Departments>();

    [InverseProperty("IdEmpNavigation")]
    public virtual ICollection<Dependents> Dependents { get; set; } = new List<Dependents>();

    [ForeignKey("IdDept")]
    [InverseProperty("Employees")]
    public virtual Departments? IdDeptNavigation { get; set; }

    [ForeignKey("IdRole")]
    public virtual IdentityRole? IdRoleNavigation { get; set; }

    [InverseProperty("IdEmpNavigation")]
    public virtual ICollection<Projemp> Projemp { get; set; } = new List<Projemp>();

    [InverseProperty("Employee")]
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual AppUser? AppUser { get; set; }
}
