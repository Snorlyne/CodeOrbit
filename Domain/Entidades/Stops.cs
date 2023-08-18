using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    [Table("Stops")]
    public class Stops
    {
        [Key]
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal longitude { get; set; }
        public string stop_name { get; set; }
        public string description { get; set; }
        public int delay_time { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //
        public virtual ICollection<RouteStop> RouteStops { get; set; }
        public bool IsDeleted { get; set; }
    }
}
