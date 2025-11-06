using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;

namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IAfiliadoService _afiliadoService;
        private readonly IPlanMedicoService _planMedicoService;

        public DashboardService(
            IAfiliadoService afiliadoService,
            IPlanMedicoService planMedicoService)
        {
            _afiliadoService = afiliadoService;
            _planMedicoService = planMedicoService;
        }

        public async Task<DashboardResponse> GetEstadisticasAsync()
        {
            var planesMedicosResponse = await _planMedicoService.GetAllAsync();
            var afiliadosResponse = await _afiliadoService.GetAllAsync();

            return new DashboardResponse
            {
                TotalPlanesMedicos = planesMedicosResponse.Count,
                TotalAfiliados = afiliadosResponse.Count,
                AfiliadosActivos = afiliadosResponse.Count(a => a.Baja == null)
            };
        }
    }
}