using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SysPet.Models
{
    public class InternamientosViewModel
    {
        public int Id { get; set; }
        [DisplayName("Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        public string Medicamento { get; set; }
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]

        public string Antecedentes { get; set; }
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        public string Tratamiento { get; set; }
        public bool Estado { get; set; }
        [DisplayName("Estado")]
        public string State { get { return Estado ? "Activo" : "Inactivo"; } }
        public int IdPaciente { get; set; }
        public int IdPersona { get; set; }
        public int IdDoctor { get; set; }
        public string Paciente { get; set; }
        public string Propietario { get; set; }
        [DisplayName("Atendió")]
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,#-ñÑ]+$", ErrorMessage = "El nombre no es válido.")]
        public string Atendio { get; set; }
        public string FullName { get; set;}

        public byte[] Imagen { get; set; }
        public string TipoContenido { get; set; }

        public List<SelectListItem> Personas { get; set; }
        public List<SelectListItem> Pacientes { get; set; }
        public List<SelectListItem> Doctores { get; set; }

        public DateTime Fecha { get; set; }
        public int CantidadRegistros { get; set; }
        public int? UserId { get; set; }
    }
}
