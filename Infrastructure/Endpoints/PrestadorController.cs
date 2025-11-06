using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Utilities;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;


[ApiController]
[Route("[controller]")]
public class PrestadorController : ControllerBase
{
	private readonly IProjectLogger logger;
	private readonly IPrestadorService prestadorService;
	public PrestadorController(IProjectLogger logger, IPrestadorService prestadorService)
	{
		this.logger = logger;
		this.prestadorService = prestadorService;
	}
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] PrestadorRequest request)
	{
		var result = await prestadorService.CreateAsync(request);
		return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] PrestadorRequest request)
	{
		var result = await prestadorService.UpdateAsync(id, request);
		var response = new
		{
			id = result.Id,
			nombreCompleto = result.NombreCompleto,
			rol = result.Rol,
			activo = result.Activo,
			especialidadesIds = result.Especialidades ?? new List<int>(),
			telefonos = result.Telefonos?.Select(t => t.Numero).Where(n => !string.IsNullOrWhiteSpace(n)).ToList() ?? new List<string>(),
			emails = result.Emails?.Select(e => e.Correo).Where(c => !string.IsNullOrWhiteSpace(c)).ToList() ?? new List<string>(),
			direcciones = result.Direcciones?.Select(d => string.IsNullOrWhiteSpace(d.Altura) || d.Altura == "S/N" ? d.Calle : $"{d.Calle} {d.Altura}").Where(s => !string.IsNullOrWhiteSpace(s)).ToList() ?? new List<string>(),
			centroMedico = result.CentroMedico
		};
		return Ok(response);
	}



	[HttpPut("{id}/estado")]
	public async Task<IActionResult> UpdateEstadoAsync([FromRoute] int id, [FromBody] PrestadorEstadoRequest request)
	{
		try
		{
			var result = await prestadorService.UpdateEstadoAsync(id, request);
			return Ok(result);
		}
		catch (Exception ex)
		{
			// Reglas de negocio para 409 en el futuro (turnos futuros, etc.)
			if (ex.Message.Contains("conflicto", StringComparison.OrdinalIgnoreCase))
				return Conflict(new { message = ex.Message });
			return NotFound();
		}
	}

	[HttpGet("getAll")]
	public async Task<IActionResult> GetAllAsync()
	{
		var result = await prestadorService.GetAllAsync();
		return Ok(result);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
	{
		var result = await prestadorService.GetByIdAsync(id);
		return Ok(result);
	}
}
