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
                    Telefonos = prestadorReq.Telefonos.Select((t, index) => new TelefonoDTO { id = index + 1, numero = t }).ToList(),
                    Emails = prestadorReq.Emails.Select((e, index) => new EmailDTO { id = index + 1, correo = e }).ToList(),
                    Direcciones = prestadorReq.Direcciones.Select((d, index) => new DireccionDTO { id = index + 1, calle = d }).ToList()

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
                    Telefonos = prestadorReq.Telefonos.Select((t, index) => new TelefonoDTO { id = index + 1, numero = t }).ToList(),
                    Emails = prestadorReq.Emails.Select((e, index) => new EmailDTO { id = index + 1, correo = e }).ToList(),
                    Direcciones = prestadorReq.Direcciones.Select((d, index) => new DireccionDTO { id = index + 1, calle = d }).ToList()
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
                    Telefonos = new List<TelefonoDTO> { new TelefonoDTO { id = 1, numero = "1234-5678" } },
                    Emails = new List<EmailDTO> { new EmailDTO { id = 1, correo = "emailfalso@email.com" } },
                    Direcciones = new List<DireccionDTO> { new DireccionDTO { id = 1, calle = "Calle Falsa 123" } }
                },
                new PrestadorResponse
                {
                    Id = 2,
                    NombreCompleto = "Dra. María Gómez",
                    Rol = 0,
                    CentroMedico = "Hospital Norte",
                    Especialidades = new List<int> { 3 },
                    Documentacion = new DocumentacionDTO { id = 2, tipoDocumento = 6, numero = "30-87654321-0" },
                    Telefonos = new List<TelefonoDTO> { new TelefonoDTO { id = 2, numero = "8765-4321" } },
                    Emails = new List<EmailDTO> { new EmailDTO { id = 2, correo = "emailtrucho@hnorte.com" } },
                    Direcciones = new List<DireccionDTO> { new DireccionDTO { id = 2, calle = "Avenida Siempre Viva 742" } }
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
