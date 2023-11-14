using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace SysPet.Models
{
    public class HistorialesViewModel
    {
        public int Id { get; set; }
        [DisplayName("Fecha de Visita")]
        public DateTime FechaVisita { get; set; } = DateTime.Now.Date;
        public string Motivo { get; set; }
        [DisplayName("Diagnóstico")]
        public string Diagnostico { get; set; }
        public int IdPaciente { get; set; }
        public string Paciente { get; set; }
        [DisplayName("Propietario")]
        public string FullName { get; set; }
        public byte[] Imagen { get; set; }
        public string TipoContenido { get; set; }
        public List<SelectListItem> Pacientes { get; set; }
        public int? UserId { get; set; }
    }
}
