using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HorarioAtencion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public List<HorarioDia> DiasAtencion { get; set; } = new();
    public DateTime HoraInicio { get; set; }
    public DateTime HoraFin { get; set; }
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
public class HorarioDia
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DiaAtencion Dia { get; set; }

    [ForeignKey(nameof(Horario))]
    public int HorarioId { get; set; }
    public HorarioAtencion Horario { get; set; } = null!;
}