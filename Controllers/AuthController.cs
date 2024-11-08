using ecycle_be.Models;
using ecycle_be.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static ecycle_be.Services.AuthService;

namespace ecycle_be.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        private readonly AuthService _authService = authService;

        [HttpPatch("login")]
        public async Task<IActionResult> Login([FromBody] Pengguna pengguna)
        {
            try
            {
                Pengguna first = await _authService.Login(pengguna);
                return Ok(first);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Pengguna pengguna)
        {
            try
            {
                Pengguna registered = await _authService.Register(pengguna);
                return Ok(registered);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update(UpdatingPengguna pengguna)
        {
            try
            {
                await _authService.UpdatePenggunaAsync(pengguna);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
