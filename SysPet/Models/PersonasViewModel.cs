using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SysPet.Models
{
    public class PersonasViewModel
    {
        public int IdPersona { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        [DisplayName("Apellido Materno")]
        public string ApellidoMaterno { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚ.,-]+$", ErrorMessage = "La dirección contiene caracteres no permitidos.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El campo debe tener entre 3 y 25 caracteres.")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚ.,-]+$", ErrorMessage = "La dirección contiene caracteres no permitidos.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El campo debe tener entre 3 y 25 caracteres.")]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [DisplayName("Código Postal")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "El código debe tener 10 dígitos numéricos.")]
        public int CodigoPostal { get; set; }
        [DisplayName("Teléfono")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "El teléfono debe tener 10 dígitos y solo puede contener números.")]
        [StringLength(10, ErrorMessage = "El teléfono debe tener exactamente 10 dígitos.")]
        public string Telefono { get; set; }

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos numéricos.")]
        public ulong PhoneNumber { get; set; }
        public bool Estado { get; set; }
        [DisplayName("Estado")]
        public string State { get { return Estado ? "Activo" : "Inactivo"; } }
        public int IdTipoPersona { get; set; }
        public List<MascotasViewModel> Mascotas { get; set; }
        public UsuariosViewModel Usuario { get; set; }
        public int? UserId { get; set; }
    }
}
