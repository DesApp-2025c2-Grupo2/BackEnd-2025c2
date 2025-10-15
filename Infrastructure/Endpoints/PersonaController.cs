using Microsoft.AspNetCore.Mvc;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _personaService;

        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaResponse>> GetById([Required] int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }

        [HttpPost]
        public async Task<ActionResult<PersonaResponse>> Create([FromBody] PersonaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var persona = await _personaService.CrearPersonaAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = persona.id }, persona);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PersonaResponse>> Update([Required] int id, [FromBody] PersonaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var persona = await _personaService.ActualizarPersonaAsync(id, request);
                return Ok(persona);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
