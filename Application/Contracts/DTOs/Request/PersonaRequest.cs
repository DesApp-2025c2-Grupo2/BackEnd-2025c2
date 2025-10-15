
using Domain.Enums;

namespace Application.Contracts.DTOs.Request
{
    public class PersonaRequest
    {
        public int? Id { get; set; } // Solo para update
        public int NumeroIntegrante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Parentesco Parentesco { get; set; }
        public int AfiliadoId { get; set; }
        public DateTime Alta { get; set; }
        public DateTime? Baja { get; set; }

        // Relaciones opcionales (solo IDs si aplica)
        public List<int>? DocumentosIds { get; set; }
        public List<int>? TelefonosIds { get; set; }
        public List<int>? EmailsIds { get; set; }
        public List<int>? DireccionesIds { get; set; }
        public List<int>? SituacionesTerapeuticasIds { get; set; }
    }
}
