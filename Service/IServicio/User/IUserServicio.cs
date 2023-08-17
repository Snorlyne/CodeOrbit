using Domain.Entidades.ViewModels;
using Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServicio.User
{
    public interface IUserServicio
    {
        Task<ResponseHelper> Crear(PersonasVM model);
        Task<List<PersonasVM>> ObtenerLista();
    }
}
