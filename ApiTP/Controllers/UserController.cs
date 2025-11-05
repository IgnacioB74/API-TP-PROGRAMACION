using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthApi.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using AuthApi.Data;
using AuthApi.DTOs;
using AuthApi.DTOs.Auth;
using AuthApi.Entities;

namespace AuthApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Autorizo _auth;
        private readonly AppDbContext _context;

        public UserController(Autorizo auth, AppDbContext context)
        {
            _auth = auth;
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            int id;
            if (!int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out id))
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id))
                    return Unauthorized();

            var user = await _auth.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // AGREGAR USUARIO 
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AgregarUsuario([FromBody] Registro dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            
            var nuevoUsuario = new Usuario
            {
                Username = dto.Username,
                Mail = dto.Mail,
                Password = dto.Contrasenia
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            var salida = new DtoUsuario
            {
                Id = nuevoUsuario.Id,
                Username = nuevoUsuario.Username,
                Mail = nuevoUsuario.Mail,
                Rol = nuevoUsuario.Rol,
                Creacion = nuevoUsuario.Creacion
            };

            return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = nuevoUsuario.Id }, salida);
        }

        //  OBTENER USUARIO POR ID 
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            var dto = new DtoUsuario
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Mail = usuario.Mail,
                Rol = usuario.Rol,
                Creacion = usuario.Creacion
            };

            return Ok(dto);
        }

        //  MODIFICAR USUARIO 
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ModificarUsuario(int id, [FromBody] DtoUsuario dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound($"No se encontró el usuario con ID {id}.");

           
            usuario.Username = dto.Username;
            usuario.Mail = dto.Mail;
            usuario.Rol = dto.Rol;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            var salida = new DtoUsuario
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Mail = usuario.Mail,
                Rol = usuario.Rol,
                Creacion = usuario.Creacion
            };

            return Ok(salida);
        }

        //  ELIMINAR USUARIO 
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound($"No se encontró el usuario con ID {id}.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
