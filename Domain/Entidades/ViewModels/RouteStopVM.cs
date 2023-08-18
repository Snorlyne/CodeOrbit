using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ViewModels
{
    public class RouteStopVM
    {
        public int Id { get; set; }

        public int id_route { get; set; }

        public List<Stops> stops { get; set; }
    }
}
