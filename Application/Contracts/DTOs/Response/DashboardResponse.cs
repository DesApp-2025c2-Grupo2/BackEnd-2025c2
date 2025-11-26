using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Response
{
    public class DashboardResponse
    {
        public int TotalPlanesMedicos { get; set; }
        public int TotalAfiliados { get; set; }
        public int AfiliadosActivos { get; set; }

        public Dictionary<int, int> AfiliadosPorPlan { get; set; } = new Dictionary<int, int>();

        //public int TotalPrestadores { get; set; }

        //public int PrestadoresActivos { get; set; }
    }
}
