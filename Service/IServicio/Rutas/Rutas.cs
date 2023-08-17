using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServicio.Rutas
{
    public interface Rutas
    {
        Task<Rutas> GetRutas();
    }
}
