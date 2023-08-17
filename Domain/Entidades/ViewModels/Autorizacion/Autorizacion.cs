using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ViewModels.Autorizacion
{
        public class AutorizacionResponse
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public string Email { get; set; }
            public int AppUser { get; set; }
        }
        public class AutorizacionRequest
        {
            public string Email { get; set; }
            public string Clave { get; set; }
        }
    }
