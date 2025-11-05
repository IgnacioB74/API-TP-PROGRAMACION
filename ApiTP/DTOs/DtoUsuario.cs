namespace AuthApi.DTOs
{
    public class DtoUsuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public DateTime Creacion { get; set; }
    }
}
