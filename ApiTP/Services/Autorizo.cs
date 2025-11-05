using AuthApi.DTOs;
using AuthApi.DTOs.Auth;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthApi.Services
{
    public interface Autorizo
    {
        Task<Respuesta> RegisterAsync(Registro req);
        Task<Respuesta> LoginAsync(Login req);
        Task<DtoUsuario?> GetUserByIdAsync(int id);
    }
}
