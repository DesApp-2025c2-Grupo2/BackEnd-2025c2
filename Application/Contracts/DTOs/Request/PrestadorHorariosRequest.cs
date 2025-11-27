namespace Application.Contracts.DTOs.Request;

public class PrestadorHorariosRequest
{
    public List<DireccionHorariosDTO> Direcciones { get; set; } = new();
    public List<int> RemoveIds { get; set; } = new();
}

public class DireccionHorariosDTO
{
    public int? Id { get; set; }
    public string Direccion { get; set; } // Texto libre que coincide con Agenda.Direccion
    public int? DuracionConsulta { get; set; }
    public List<HorarioEdicionDTO> Horarios { get; set; } = new();
    public List<HorarioEdicionDTO> HorariosAtencion { get; set; } = new();
}

public class HorarioEdicionDTO
{
    public int? Id { get; set; }
    public int DiaSemana { get; set; } // 0=Domingo ... 6=Sabado (legacy)
    public List<int>? DiasDeLaSemana { get; set; } // preferido: lista de días
    // Si el prestador es un Centro Médico, este es el profesional al que se le asigna el horario
    public int? ProfesionalId { get; set; }
    public string HoraInicio { get; set; } // HH:mm
    public string HoraFin { get; set; }    // HH:mm
    public List<int> Especialidades { get; set; } = new();
    public int? DuracionMinutos { get; set; }
    public bool? Deleted { get; set; }
}



