using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public LoginController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // POST /api/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Autenticar([FromBody] LoginDto dto)
        {
            try
            {
                // Lógica de login será implementada aqui
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }
}