using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class RouteStop
    {
        public int RouteId { get; set; }
        public int StopId { get; set; }

        public virtual Routes Route { get; set; }
        public virtual Stops Stop { get; set; }
    }
}
