using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Direccion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(32)]
    public string Calle { get; set; }
    [Required]
    [MaxLength(8)]
    public string Altura { get; set; }
    [MaxLength(16)]
    public string? Piso { get; set; }
    [MaxLength(16)]
    public string? Departamento { get; set; }
    [Required]
    [MaxLength(128)]
    public string ProvinciaCiudad { get; set; }
    [ForeignKey(nameof(Persona))]
    public int? PersonaId { get; set; }
    public Persona? Persona { get; set; }
    [ForeignKey(nameof(Prestador))]
    public int? PrestadorId { get; set; }
    public Prestador? Prestador { get; set; }

}
