using Microsoft.AspNetCore.Mvc;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _personaService;

        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }

        [HttpPost("addMember/{afiliadoID}")]
        public async Task<IActionResult> Create([FromBody] PersonaRequest request)
        {
            ActionResult result;
            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var persona = await _personaService.AddPersonAsync(request);
                    result = CreatedAtAction(nameof(GetById), new { id = persona.Id }, persona);
                }
                catch (Exception ex)
                {
                    result = BadRequest(new
                    {
                        message = ex.Message,
                        detalle = ex.InnerException?.Message
                    });
                }
            }
            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([Required] int id, [FromBody] PersonaRequest request)
        {
            ActionResult result;
            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var persona = await _personaService.UpdatePersonAsync(id, request);
                    result = Ok(persona);
                }
                catch (Exception ex)
                {
                    result = BadRequest(new
                    {
                        message = ex.Message,
                        detalle = ex.InnerException?.Message
                    });
                }
            }
            return result;
        }

        [HttpPatch("toggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus([Required] int id, [FromQuery] DateTime? fecha)
        {
            ActionResult result;
            try
            {
                var toggled = await _personaService.ToggleStatusAsync(id, fecha);
                result = Ok(toggled);
            }
            catch (Exception ex)
            {
                result = BadRequest(new
                {
                    message = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
            return result;
        }
    }
}
