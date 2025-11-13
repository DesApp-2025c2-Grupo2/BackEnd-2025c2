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
    }
}
