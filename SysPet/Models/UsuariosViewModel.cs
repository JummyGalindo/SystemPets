﻿namespace SysPet.Models
{
    public class UsuariosViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public int IdPersona { get; set; }
        public int IdRol { get; set; }
        public bool Estado { get; set; }
    }
}
