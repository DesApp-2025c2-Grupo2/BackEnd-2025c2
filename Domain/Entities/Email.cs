using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Email
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(64)]
    public string Correo { get; set; }

    [ForeignKey(nameof(Persona))]
    public int? PersonaId { get; set; }
    public Persona? Persona { get; set; }
    [ForeignKey(nameof(Prestador))]
    public int? PrestadorId { get; set; }
    public Prestador? Prestador { get; set; }

}