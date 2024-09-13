    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace HRIS.Infrastructure;

[Table("locations")]
public partial class Locations
{
    [Key]
    [Column("id_location")]
    public int IdLocation { get; set; }

    [Column("namelocation")]
    [StringLength(100)]
    public string Namelocation { get; set; } = null!;

    [Column("addresslocation")]
    public string Addresslocation { get; set; } = null!;

    [InverseProperty("IdLocationNavigation")]
    [JsonIgnore]
    public virtual ICollection<Departments> Departments { get; set; } = new List<Departments>();

    [InverseProperty("IdLocationNavigation")]
    [JsonIgnore]  // Tambahkan JsonIgnore jika Anda tidak ingin properti ini diserialisasi
    public virtual ICollection<Projects> Projects { get; set; } = new List<Projects>();
}
