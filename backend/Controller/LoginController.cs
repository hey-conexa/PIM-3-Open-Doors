using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Exceptions;
using OpenDoors.Api.Services;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // POST /api/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Autenticar([FromBody] LoginDto dto)
        {
            try
            {
                var response = await _loginService.AutenticarAsync(dto);
                return Ok(response);
            }
            catch (MissingRequiredFieldException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }
}