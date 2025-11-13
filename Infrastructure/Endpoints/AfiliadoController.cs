using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AfiliadosController : ControllerBase
    {
        private readonly IProjectLogger logger;
        private readonly IAfiliadoService _afiliadoService;

        public AfiliadosController(IProjectLogger projectLogger, IAfiliadoService afiliadoService)
        {
            logger = projectLogger;
            _afiliadoService = afiliadoService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            ActionResult result;
            try
            {
                AfiliadosResponse afiliados = await _afiliadoService.GetAllAsync();
                result = Ok(afiliados);
            }
            catch (Exception ex)
            {
                logger.LogError("Error al obtener los Afiliados.", ex);
                result = StatusCode(500, new
                {
                    message = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
            return result;
        }

        [HttpGet("{numeroAfiliado}")]
        public async Task<IActionResult> GetByNumero([Required] int numeroAfiliado)
        {
            ActionResult result;
            try
            {
                var afiliado = await _afiliadoService.GetByNumeroAsync(numeroAfiliado);
                result = afiliado != null ? Ok(afiliado) : NotFound();
                
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
        

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AfiliadoRequest request)
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
                    var afiliado = await _afiliadoService.CreateAsync(request);
                    result = CreatedAtAction(nameof(GetByNumero), new { numeroAfiliado = afiliado.NumeroAfiliado }, afiliado);
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

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([Required] int id, [FromBody] AfiliadoRequest request)
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
                    var actualizado = await _afiliadoService.UpdateAsync(id, request);
                    result = Ok(actualizado);
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


        /// <summary>
        /// Activa o desactiva un afiliado según el número de afiliado proporcionado.
        /// Devuelve true si el cambio fue exitoso.
        /// </summary>
        /// <param name="numeroAfiliado"></param>
        /// <param name="activo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPatch("toggleStatus/{afiliadoID}")]
        public async Task<IActionResult> ToggleStatus([Required] int afiliadoID, [FromQuery] bool activo, [FromQuery] DateTime? fecha)
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
                    var toggled = await _afiliadoService.ToggleStatus(afiliadoID, activo, fecha);
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
            }
            return result;
        }

    }
}