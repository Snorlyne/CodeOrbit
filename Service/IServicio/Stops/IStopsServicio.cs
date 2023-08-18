using Domain.Entidades.ViewModels;
using Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServicio.Stops
{
    public interface IStopsServicio
    {
        Task<ResponseHelper> Crear(StopsCreacionVM stops);
        Task<ResponseHelper> Editar(StopsVM stops, int? id);
        Task<ResponseHelper> Eliminar(int? id);
        Task<List<StopsVM>> ObtenerLista();
        Task<StopsVM> ObtenerPorId(int? id);
    }
}
