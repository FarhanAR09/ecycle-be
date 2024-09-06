using ecycle_be.Models;
using ecycle_be.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace ecycle_be.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(AuthService authService) : Controller
    {
        private readonly AuthService authService = authService;

        [HttpPatch("login")]
        public async Task<IActionResult> Login([FromBody] Pengguna pengguna)
        {
            try
            {
                Pengguna first = await authService.Login(pengguna);
                return Ok(first);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Pengguna pengguna)
        {
            return Ok(pengguna);
        }
    }
}
