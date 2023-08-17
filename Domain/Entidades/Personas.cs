using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Domain.Entidades
{
    public class Personas
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(60)]
        [Display(Name = "Nombre Persona")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string CURP { get; set; }
        [DataType(DataType.EmailAddress)]
        public string AppUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}