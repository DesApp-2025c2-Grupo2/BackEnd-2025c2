using Application.Contracts.DTOs.Response;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs.Internal;

public class HorarioAtencionDTO
{
    public int? Id { get; set; }
    public int? Orden { get; set; }
    public List<HorarioDiaDTO> DiasAtencion { get; set; }
    public TimeOnly HoraInicio { get; set; } // HH:mm:ss
    public TimeOnly HoraFin { get; set; } // HH:mm:ss
    public int DuracionMinutos { get; set; }
    public EspecialidadDTO Especialidad { get; set; }
    public ProfesionalDTO? ProfesionalAsignado { get; set; }

}
public class HorarioDiaDTO
{
    public int? Id { get; set; }
    public DiaAtencion Dia { get; set; }
}
