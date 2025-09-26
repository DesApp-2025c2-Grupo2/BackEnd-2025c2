using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Documentacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public TipoDocumento TipoDocumento { get; set; }// 1: Documento Nacional de Identidad, 2: Cédula de Identidad, 3: Matricula Nacional, 4: CUIL, 5: RUT, 6: CUIT
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
