using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace SysPet.Models
{
    public class MascotasViewModel
    {
        public int IdPaciente { get; set; }
        [DisplayName("Paciente")]
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Este campo debe contener solo letras.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 50 caracteres.")]
        public string Raza { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Este campo debe contener solo letras.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 20 caracteres.")]
        public string Especie { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Este campo debe contener solo letras.")]

        [StringLength(10, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 10 caracteres.")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "El campo debe tener entre 1 y 10 caracteres.")]
        public string Edad { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Este campo debe contener solo letras.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 10 caracteres.")]
        public string Color { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Este campo debe contener solo letras y números.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 25 caracteres.")]
        public string Peso { get; set; }
        public bool Estado { get; set; }
        [DisplayName("Estado")]
        public string State { get { return Estado ? "Activo" : "Inactivo"; } }
        public string Propietario { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        [DisplayName("Propietario")]
        public string FullName { get { return $"{Propietario} {ApellidoPaterno} {ApellidoMaterno}"; } }
        [DisplayName("Foto")]
        public byte[] Imagen { get; set; }

        [DisplayName("Fecha de Registro")]
        public DateTime Fecha { get; set; }
        public int IdPersona { get; set; }
        public List<PersonasViewModel> Personas { get; set; }
        public List<SelectListItem> ListaPersonas { get; set; }
        public string NombreArchivo { get; set; }
        public string FileName { get; set; } 
        public string TipoContenido { get; set; }
        public IFormFile Image { get; set; }
        public MascotasViewModel()
        {
            FileName = Image!= null ? Image.FileName : "";
        }
        public int? UserId { get; set; }
    }
}
