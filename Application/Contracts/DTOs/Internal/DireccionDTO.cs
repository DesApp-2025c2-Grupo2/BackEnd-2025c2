namespace Application.Contracts.DTOs.Internal
{
    public class DireccionDTO
    {
        public int? id { get; set; }
        public string calle { get; set; }
        public string altura { get; set; }
        public string piso { get; set; } = string.Empty;
        public string departamento { get; set; } = string.Empty;
        public string provinciaCiudad { get; set; }
    }
}
