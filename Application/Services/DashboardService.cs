using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;

namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IAfiliadoService _afiliadoService;
        private readonly IPlanMedicoService _planMedicoService;
        private readonly IPrestadorService _prestadorService;

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
            //var prestadoresResponse = await _prestadorService.GetAllAsync();

            // Contar afiliados activos (baja es null O alta es más reciente que baja)
            var afiliadosActivos = afiliadosResponse.Count(a =>
                a.Baja == null || a.Alta > a.Baja
            );

            // Contar afiliados por plan médico
            var afiliadosPorPlan = afiliadosResponse
                .GroupBy(a => a.PlanMedicoId)
                .ToDictionary(g => g.Key, g => g.Count());

            // Contar prestadores activos (misma lógica que afiliados)
            /*var prestadoresActivos = prestadoresResponse.Count(p =>
                p.Baja == null || p.Alta > p.Baja
            );*/

            return new DashboardResponse
            {
                TotalPlanesMedicos = planesMedicosResponse.Count,
                TotalAfiliados = afiliadosResponse.Count,
                AfiliadosActivos = afiliadosActivos,
                AfiliadosPorPlan = afiliadosPorPlan,
                //TotalPrestadores = prestadoresResponse.Count,
                //PrestadoresActivos = prestadoresActivos
            };
        }
    }
}