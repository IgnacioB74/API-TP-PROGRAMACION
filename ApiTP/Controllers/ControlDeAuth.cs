using AuthApi.DTOs.Auth;
using AuthApi.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlDeAuth : ControllerBase
    {
        private readonly Autorizo _auth;
        public ControlDeAuth(Autorizo auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Registro req)
        {
            try
            {
                var res = await _auth.RegisterAsync(req);
                return Created("", res);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login req)
        {
            try
            {
                var res = await _auth.LoginAsync(req);
                return Ok(res);
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
