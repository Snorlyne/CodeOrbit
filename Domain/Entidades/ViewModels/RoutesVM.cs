using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ViewModels
{
    public class RoutesVM
    {
        public int Id { get; set; }
        public string bus_type { get; set; }
        public string route_name { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public int fare { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<StopsListaVM> stops { get; set; }
    }
    public class RoutesCreacionVM
    {
        public string bus_type { get; set; }
        public string route_name { get; set; }
        public int fare { get; set; }
        public IFormFile GeoJSON { get; set; }
    }
}
