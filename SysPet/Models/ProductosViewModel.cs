﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SysPet.Models
{
    public class ProductosViewModel
    {
        public int IdProducto { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Este campo debe contener solo letras.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }
        [DisplayName("Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Este campo debe contener solo letras.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 50 caracteres.")]
        public string Proveedor { get; set; }
        public int Cantidad { get; set; }
        public int Stock { get; set; }
        [DisplayName("Precio Unitario")]
        public decimal PrecioUnitario { get; set; }
        [DisplayName("Precio Sugerido")]
        public decimal PrecioSugerido { get; set; }
        [DisplayName("Descripción")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        public string Descripcion { get; set; }
        [DisplayName("Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }
        public bool Estado { get; set; }
    }
}
