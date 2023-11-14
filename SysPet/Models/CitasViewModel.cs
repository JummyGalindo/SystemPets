using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace SysPet.Models
{
    public class CitasViewModel
    {
        [DisplayName("Cita No.")]
        public int Id { get; set; }
        [DisplayName("Fecha de Cita")]
        public DateTime FechaCita { get; set; } = DateTime.Now.Date;
        public string AppointmentDate { get; set; }
        public string Motivo { get; set; }
        public int IdPersona { get; set; }
        public int IdEstado { get; set; }
        [DisplayName("Clíente")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        [DisplayName("Apellido Materno")]
        public string ApellidoMaterno { get; set; }
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get { return $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}";  } }
        public string Estado { get; set; }
        public List<SelectListItem> Personas { get; set; }
        public List<SelectListItem> EstadoCitas { get; set; }
        public List<CitasViewModel> ExpiredDates { get; set; }
        public List<CitasViewModel> DatesToExpired { get; set; }

        public DateTime Fecha { get; set; }
        public int CantidadRegistros { get; set; }
        public int? UserId { get; set; }

    }
}
