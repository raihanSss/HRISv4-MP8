using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace HRIS.Infrastructure;

[Table("dependents")]
public partial class Dependents
{
    [Key]
    [Column("id_dependent")]
    public int IdDependent { get; set; }

    [Column("namedependent")]
    [StringLength(100)]
    public string Namedependent { get; set; } = null!;

    [Column("sexdependent")]
    [MaxLength(1)]
    public string? Sexdependent { get; set; }

    [Column("dobdependent")]
    public DateOnly Dobdependent { get; set; }

    [Column("relation")]
    [StringLength(50)]
    public string Relation { get; set; } = null!;

    [Column("id_emp")]
    public int? IdEmp { get; set; }

    [ForeignKey("IdEmp")]
    [InverseProperty("Dependents")]
    [JsonIgnore]
    public virtual Employees? IdEmpNavigation { get; set; }
}
