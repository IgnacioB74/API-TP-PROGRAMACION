namespace AuthApi.DTOs.Auth
{
    public class Respuesta
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiraEn { get; set; }
        public string? Rol { get; set; }
    }
}
