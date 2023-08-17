using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entidades.ViewModels
{
    public class PersonasVM
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string CURP { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }

}
