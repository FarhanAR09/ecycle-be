using ecycle_be.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ecycle_be.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpPatch("login")]
        public IActionResult Login([FromBody] Pengguna pengguna)
        {
            return Ok(new
            {
                token = "token",
                user = pengguna.Nama,
                role = "admin",
                pass = pengguna.Password
            });
        }

        [HttpPost("register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
