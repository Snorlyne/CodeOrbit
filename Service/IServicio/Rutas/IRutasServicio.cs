using Domain.Entidades.ViewModels;
using Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServicio.Rutas
{
    public interface IRutasServicio
    {
        Task<ResponseHelper> Crear(string json);
    }
}
