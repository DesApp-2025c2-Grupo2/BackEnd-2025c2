using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Documentacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(32)]
    public string TipoDocumento { get; set; }// 1: Documento Nacional de Identidad, 2: Cédula de Identidad, 3: Pasaporte, 4: Matricula Nacional, 5: CUIT
    [Required]
    [MaxLength(16)]
    public string Numero { get; set; }

    [ForeignKey(nameof(Persona))]
    public int? PersonaId { get; set; }
    public Persona? Persona { get; set; }
    [ForeignKey(nameof(Prestador))]
    public int? PrestadorId { get; set; }
    public Prestador? Prestador { get; set; }

}
