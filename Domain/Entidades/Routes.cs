using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    [Table("Routes")]
    public class Routes
    {
        [Key]
        public int Id { get; set; }
        public string bus_type { get; set; }
        public string route_name { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public int fare { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        //Auditoria
        //public virtual TipoPermiso TipoPermiso { get; set; }
        public virtual ICollection<RouteStop> RouteStops { get; set; }

        public bool IsDeleted { get; set; }
    }
}
