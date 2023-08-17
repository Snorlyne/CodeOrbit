using Domain.Entidades;
using Domain.Entidades.ViewModels.Autorizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServicio
{
    public interface IAutorizacionServicio
    {
        Task<string> GenerarToken(ApplicationUser user);
        Task<string> GenerarRefreshToken();
    }
}
