using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace HRIS.Infrastructure;

[Table("departments")]
[Index("NumberDept", Name = "departments_number_dept_key", IsUnique = true)]
public partial class Departments
{
    [Key]
    [Column("id_dept")]
    public int IdDept { get; set; }

    [Column("name_dept")]
    [StringLength(100)]
    public string NameDept { get; set; } = null!;

    [Column("number_dept")]
    public int NumberDept { get; set; }

    [Column("heademp")]
    public int? Heademp { get; set; }

    [Column("id_location")]
    public int? IdLocation { get; set; }

    [InverseProperty("IdDeptNavigation")]
    public virtual ICollection<Employees> Employees { get; set; } = new List<Employees>();

    [ForeignKey("Heademp")]
    [InverseProperty("Departments")]
    public virtual Employees? HeadempNavigation { get; set; }

    [ForeignKey("IdLocation")]
    [InverseProperty("Departments")]
    [JsonIgnore]
    public virtual Locations? IdLocationNavigation { get; set; }

    [InverseProperty("IdDeptNavigation")]
    public virtual ICollection<Projects> Projects { get; set; } = new List<Projects>();
}
