using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;


[ApiController]
[Route("[controller]")]
public class PrestadorController : ControllerBase
{
    private readonly IProjectLogger logger;
    public PrestadorController(IProjectLogger logger)
    {
        this.logger = logger;
    }
    [HttpPost("save")]
    public Task<IActionResult> SaveAsync([FromBody] PrestadorRequest prestadorReq, [FromQuery] int id = 0)
    {
        try
        {
            if (id == 0)
            {
                //var result = await service.AddAsync(especialidadRequest);
                PrestadorResponse prestador = new PrestadorResponse
                {
                    Id = 1,
                    NombreCompleto = prestadorReq.NombreCompleto,
                    Rol = prestadorReq.Rol,
                    CentroMedico = prestadorReq.CentroMedico,
                    Especialidades = new List<int> { 1, 2 }, // Ejemplo estático
                    Documentacion = new DocumentacionDTO { id = 1, tipoDocumento = 6, numero = prestadorReq.Documentacion },
                    Telefonos = prestadorReq.Telefonos.Select((t, index) => new TelefonoDTO { Id = index + 1, Numero = t }).ToList(),
                    Emails = prestadorReq.Emails.Select((e, index) => new EmailDTO { Id = index + 1, Correo = e }).ToList(),
                    Direcciones = prestadorReq.Direcciones.Select((d, index) => new DireccionDTO { Id = index + 1, Calle = d, Altura = string.Empty }).ToList()

                };
                logger.LogSuccess("Prestador agregado exitosamente.");
                return Task.FromResult<IActionResult>(Ok(prestador));
            }
            else
            {
                //var result = await service.UpdateAsync(id, especialidadRequest);
                PrestadorResponse prestadorUPD = new PrestadorResponse
                {
                    Id = id,
                    NombreCompleto = prestadorReq.NombreCompleto,
                    Rol = prestadorReq.Rol,
                    CentroMedico = prestadorReq.CentroMedico,
                    Especialidades = new List<int> { 1, 2 }, // Ejemplo estático
                    Documentacion = new DocumentacionDTO { id = 1, tipoDocumento = 6, numero = prestadorReq.Documentacion },
                    Telefonos = prestadorReq.Telefonos.Select((t, index) => new TelefonoDTO { Id = index + 1, Numero = t }).ToList(),
                    Emails = prestadorReq.Emails.Select((e, index) => new EmailDTO { Id = index + 1, Correo = e }).ToList(),
                    Direcciones = prestadorReq.Direcciones.Select((d, index) => new DireccionDTO { Id = index + 1, Calle = d, Altura = string.Empty }).ToList()
                };
                logger.LogSuccess("Prestador actualizado exitosamente.");
                return Task.FromResult<IActionResult>(Ok(prestadorUPD));
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error al guardar el Prestador.", ex);
            return Task.FromResult<IActionResult>(StatusCode(500, "Error interno del servidor."));
        }
    }

    [HttpGet("getAll")]
    public Task<IActionResult> GetAllAsync()
    {
        try
        {
            PrestadoresResponse prestadores = new PrestadoresResponse
            {
                new PrestadorResponse
                {
                    Id = 1,
                    NombreCompleto = "Dr. Juan Pérez",
                    Rol = 0,
                    CentroMedico = "Clínica Central",
                    Especialidades = new List<int> { 1, 2 },
                    Documentacion = new DocumentacionDTO { id = 1, tipoDocumento = 6, numero = "30-12345678-9" },
                    Telefonos = new List<TelefonoDTO> { new TelefonoDTO { Id = 1, Numero = "1234-5678" } },
                    Emails = new List<EmailDTO> { new EmailDTO { Id = 1, Correo = "emailfalso@email.com" } },
                    Direcciones = new List<DireccionDTO> { new DireccionDTO { Id = 1, Calle = "Calle Falsa 123", Altura = string.Empty } }
                },
                new PrestadorResponse
                {
                    Id = 2,
                    NombreCompleto = "Dra. María Gómez",
                    Rol = 0,
                    CentroMedico = "Hospital Norte",
                    Especialidades = new List<int> { 3 },
                    Documentacion = new DocumentacionDTO { id = 2, tipoDocumento = 6, numero = "30-87654321-0" },
                    Telefonos = new List<TelefonoDTO> { new TelefonoDTO { Id = 2, Numero = "8765-4321" } },
                    Emails = new List<EmailDTO> { new EmailDTO { Id = 2, Correo = "emailtrucho@hnorte.com" } },
                    Direcciones = new List<DireccionDTO> { new DireccionDTO { Id = 2, Calle = "Avenida Siempre Viva 742", Altura = string.Empty } }
                }
            };
            logger.LogSuccess("Prestadores obtenidos exitosamente.");
            return Task.FromResult<IActionResult>(Ok(prestadores));
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener los Prestadores.", ex);
            return Task.FromResult<IActionResult>(StatusCode(500, "Error interno del servidor."));
        }
    }
}
