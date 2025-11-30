namespace Application.Contracts.DTOs.Internal
{
    public class DireccionDTO
    {
        public int? Id { get; set; }
        public required string Calle { get; set; }
        public required string Altura { get; set; }
        public string? Piso { get; set; } = string.Empty;
        public string? Departamento { get; set; } = string.Empty;
        public required string ProvinciaCiudad { get; set; }
        public required int CodigoPostal { get; set; }
    }
}