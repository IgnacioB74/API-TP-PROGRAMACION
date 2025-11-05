using System;

namespace AuthApi.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Apellido { get; set; }
        public string Mail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Rol { get; set; } = "user";
        public DateTime Creacion { get; set; } = DateTime.UtcNow;
    }
}
