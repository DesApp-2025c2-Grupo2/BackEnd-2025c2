using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Persona
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int NumeroIntegrante { get; set; }
    [Required]
    [MaxLength(64)]
    public string Nombre { get; set; }
    [Required]
    [MaxLength(64)]
    public string Apellido { get; set; }
    [Required]
    public DateTime FechaNacimiento { get; set; }
    [Required]
    public Parentesco Parentesco { get; set; } // 1: Titular, 2: Cónyuge, 3: Hijo, 4: FamiliarACargo
    [Required]
    [ForeignKey(nameof(Afiliado))]
    public int AfiliadoId { get; set; }
    public Afiliado Afiliado { get; set; }
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }

    // Navegacion bidireccional
    public List<Documentacion> Documentos { get; set; } = new List<Documentacion>();
    public List<Telefono> Telefonos { get; set; } = new List<Telefono>();
    public List<Email> Emails { get; set; } = new List<Email>();
    public List<Direccion> Direcciones { get; set; } = new List<Direccion>();
    public List<SituacionTerapeutica> SituacionesTerapeuticas { get; set; } = new List<SituacionTerapeutica>();
}
