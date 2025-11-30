using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public abstract class Agenda
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey(nameof(DireccionAtencion))]
    public int DireccionId { get; set; }
    public Direccion DireccionAtencion { get; set; } = null!;
    public List<HorarioAtencion> Horarios { get; set; } = new();
}

public class AgendaProfesional : Agenda
{
    [Required]
    [ForeignKey(nameof(Profesional))]
    public int ProfesionalId { get; set; }
    public Profesional Profesional { get; set; } = null!;
}
public class AgendaCentroMedico : Agenda
{
    [Required]
    [ForeignKey(nameof(CentroMedico))]
    public int CentroMedicoId { get; set; }
    public CentroMedico CentroMedico { get; set; } = null!;
}
