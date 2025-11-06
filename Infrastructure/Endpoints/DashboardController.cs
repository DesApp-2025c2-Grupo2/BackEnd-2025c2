using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardResponse>> GetEstadisticas()
        {
            try
            {
                var estadisticas = await _dashboardService.GetEstadisticasAsync();
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las estadísticas del dashboard: {ex.Message}");
            }
        }
    }
}