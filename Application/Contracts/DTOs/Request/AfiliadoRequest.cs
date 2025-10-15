
namespace Application.Contracts.DTOs.Request
{
    public class AfiliadoRequest
    {
        public int NumeroAfiliado { get; set; }
        public int TitularID { get; set; }
        public int PlanMedicoId { get; set; }
        public DateTime Alta { get; set; }
        public DateTime? Baja { get; set; }

        // Lista opcional de integrantes (por Id o request anidados)
        public List<PersonaRequest>? Integrantes { get; set; }
    }
}
