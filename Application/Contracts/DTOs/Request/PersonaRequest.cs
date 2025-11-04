
using Application.Contracts.DTOs.Internal;
using Domain.Entities;
using Domain.Enums;

namespace Application.Contracts.DTOs.Request
{
    public class PersonaRequest
    {
        public int? Id { get; set; } 
        public int NumeroIntegrante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Parentesco { get; set; }
        public int AfiliadoId { get; set; }
        public DateTime Alta { get; set; }
        public DateTime? Baja { get; set; }

        public DocumentacionDTO Documentacion { get; set; }
        public List<TelefonoDTO>? Telefonos { get; set; }
        public List<EmailDTO>? Emails { get; set; }
        public List<DireccionDTO>? Direcciones { get; set; }
        public Dictionary<int,DateTime?> SituacionesTerapeuticas{ get; set; }
    }
}
