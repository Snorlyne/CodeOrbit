using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    [Table("route_stops")]
    public class RouteStop
    {
        [Key]
        public int Id { get; set; }

        public int id_route { get; set; }

        public int stop_id { get; set; }

        [ForeignKey("id_route")]
        public Routes Route { get; set; }

        [ForeignKey("stop_id")]
        public Stops Stop { get; set; }
    }
}
