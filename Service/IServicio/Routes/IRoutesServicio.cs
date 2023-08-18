using Domain.Entidades.ViewModels;
using Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServicio.Routes
{
    public interface IRoutesServicio
    {
        Task<ResponseHelper> Crear(RoutesCreacionVM routes, string json);
        Task<ResponseHelper> Editar(RoutesVM routes, int? id);
        Task<ResponseHelper> Eliminar(int? id);
        Task<List<RoutesVM>> ObtenerLista();
        Task<RoutesVM> ObtenerPorId(int? id);
    }
}
