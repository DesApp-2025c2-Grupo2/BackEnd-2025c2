using Application.Contracts.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetEstadisticasAsync();
    }
}
