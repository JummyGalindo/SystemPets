﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SysPet.Models
{
    public class UsuariosViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Este campo debe contener solo letras.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Ingresa una dirección de correo electrónico válida.")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo de contraseña es requerido.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        [Compare("ConfirmPassword")]
        public string Contrasenia { get; set; }
        [Required(ErrorMessage = "El campo es requerido.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar contraseña")]
        public string ConfirmPassword { get; set; }
        public int IdPersona { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public bool Estado { get; set; }
        public PersonasViewModel Persona { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
