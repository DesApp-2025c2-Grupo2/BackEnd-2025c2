using Microsoft.AspNetCore.Mvc;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AfiliadosController : ControllerBase
    {
        private readonly IAfiliadoService _afiliadoService;

        public AfiliadosController(IAfiliadoService afiliadoService)
        {
            _afiliadoService = afiliadoService;
        }

        [HttpGet]
        public async Task<ActionResult<AfiliadosResponse>> GetAll()
        {
            var afiliados = await _afiliadoService.GetAllAsync();
            return Ok(afiliados);
        }

        [HttpGet("{numeroAfiliado}")]
        public async Task<ActionResult<AfiliadoResponse>> GetByNumero([Required] int numeroAfiliado)
        {
            var afiliado = await _afiliadoService.GetByNumeroAsync(numeroAfiliado);
            if (afiliado == null)
            {
                return NotFound();
            }
            return Ok(afiliado);
        }

        [HttpPost]
        public async Task<ActionResult<AfiliadoResponse>> Create([FromBody] AfiliadoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var afiliado = await _afiliadoService.CreateAsync(request);
                return CreatedAtAction(nameof(GetByNumero), new { numeroAfiliado = afiliado.NumeroAfiliado }, afiliado);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([Required] int id, [FromBody] AfiliadoRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var actualizado = await _afiliadoService.UpdateAsync(id, request);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }
    }
}