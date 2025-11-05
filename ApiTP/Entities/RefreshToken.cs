using AuthApi.Entities;
using System;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string JwtId { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
    public DateTime Creado { get; set; }
    public DateTime Expira { get; set; }
    public bool Revoked { get; set; }
    public string? Reemplazado { get; set; }
}
