using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRIS.Infrastructure;

[Table("projemp")]
public partial class Projemp
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_emp")]
    public int? IdEmp { get; set; }

    [Column("id_proj")]
    public int? IdProj { get; set; }

    [Column("hoursperweek")]
    [Precision(5, 2)]
    public decimal Hoursperweek { get; set; }

    [ForeignKey("IdEmp")]
    [InverseProperty("Projemp")]
    public virtual Employees IdEmpNavigation { get; set; }

    [ForeignKey("IdProj")]
    [InverseProperty("Projemp")]
    public virtual Projects IdProjNavigation { get; set; }
}
