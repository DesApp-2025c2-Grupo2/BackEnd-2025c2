using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HorarioAtencion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int Orden { get; set; }

    public DiaAtencion Dia { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public int DuracionConsultaMinutos { get; set; }
    [Required]
    [ForeignKey(nameof(Especialidad))]
    public int EspecialidadId { get; set; }
    public Especialidad Especialidad { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Agenda))]
    public int AgendaId { get; set; }
    public Agenda Agenda { get; set; } = null!;
    [ForeignKey(nameof(ProfesionalAsignado))]
    public int? ProfesionalAsignadoId { get; set; }
    public Profesional? ProfesionalAsignado { get; set; }
}