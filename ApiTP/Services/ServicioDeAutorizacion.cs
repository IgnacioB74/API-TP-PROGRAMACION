using AuthApi.Data;
using AuthApi.DTOs;
using AuthApi.DTOs.Auth;
using AuthApi.Entities;
using AuthApi.Helpers;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Services
{
    public class ServicioDeAutorizacion : Autorizo
    {
        private readonly AppDbContext _db;
        private readonly JwtSettings _jwt;

        public ServicioDeAutorizacion(AppDbContext db, IOptions<JwtSettings> jwtOptions)
        {
            _db = db;
            _jwt = jwtOptions.Value;
        }

        public async Task<Respuesta> RegisterAsync(Registro req)
        {
            req.Mail = req.Mail.Trim().ToLower();
            if (await _db.Usuarios.AnyAsync(u => u.Mail == req.Mail))
                throw new ApplicationException("El Mail ya está en uso.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Contrasenia);

            var user = new Usuario
            {
                Username = req.Username,
                Nombre = req.Username, 
                Mail = req.Mail,
                Password = passwordHash,
                Rol = "user"
            };
            _db.Usuarios.Add(user);
            await _db.SaveChangesAsync();

            var token = GenerateToken(user);

            return new Respuesta
            {
                Token = token.token,
                ExpiraEn = token.expires,
                Rol = user.Rol
            };
        }

        public async Task<Respuesta> LoginAsync(Login req)
        {
            req.Mail = req.Mail.Trim().ToLower();
            var user = await _db.Usuarios.SingleOrDefaultAsync(u => u.Mail == req.Mail);
            if (user == null) throw new ApplicationException("Credenciales inválidas.");

            bool ok = BCrypt.Net.BCrypt.Verify(req.Contrasenia, user.Password);
            if (!ok) throw new ApplicationException("Credenciales inválidas.");

            var token = GenerateToken(user);

            return new Respuesta
            {
                Token = token.token,
                ExpiraEn = token.expires,
                Rol = user.Rol
            };
        }

        public async Task<DtoUsuario?> GetUserByIdAsync(int id)
        {
            var u = await _db.Usuarios.FindAsync(id);
            if (u == null) return null;
            return new DtoUsuario
            {
                Id = u.Id,
                Username = u.Nombre,
                Mail = u.Mail,
                Rol = u.Rol,
                Creacion = u.Creacion
            };
        }

        private (string token, DateTime expires) GenerateToken(Usuario user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiresMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public Task<Respuesta> RegisterAsync(RegisterRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
