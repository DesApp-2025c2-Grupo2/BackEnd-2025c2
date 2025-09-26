using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Afiliado
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int NumeroAfiliado { get; set; }
    [Required]
    public int TitularID { get; set; }
    [Required]
    [ForeignKey(nameof(PlanMedico))]
    public int PlanMedicoId { get; set; }
    public PlanMedico PlanMedico { get; set; }
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }

    // Navegacion bidireccional
    public List<Persona> Integrantes { get; set; } = new List<Persona>();

}
