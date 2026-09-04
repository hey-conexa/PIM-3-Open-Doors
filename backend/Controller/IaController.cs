using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.IA;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/ia")]
    public class IaController : ControllerBase
    {
        private readonly IIAService _iaService;

        public IaController(IIAService IaService)
        {
            _iaService = IaService;
        }

        // ===========================================
        // POST /api/ia/analisar-curriculo
        // Recebe PDF via form-data, extrai habilidades
        // ===========================================
        [HttpPost("analisar-curriculo")]
        public async Task<IActionResult> AnalisarCurriculo(
            [FromForm] Guid estudanteId,
            IFormFile curriculo)
        {
            var resultado = await _iaService.AnalisarCurriculo(estudanteId, curriculo);
            return Ok(resultado);
        }

        // ===========================================
        // POST /api/ia/analisar-teste
        // Recebe respostas do teste vocacional, gera perfil
        // ===========================================
        [HttpPost("analisar-teste")]
        public async Task<IActionResult> AnalisarTeste([FromBody] AnalisarTesteRequestDto body)
        {
            var resultado = await _iaService.AnalisarTeste(body);
            return Ok(resultado);
        }

        // ===========================================
        // POST /api/ia/gerar-score
        // Calcula compatibilidade Estudante x Vaga
        // ===========================================
        [HttpPost("gerar-score")]
        public async Task<IActionResult> GerarScore([FromBody] GerarScoreRequestDto body)
        {
            var resultado = await _iaService.GerarScore(body);
            return Ok(resultado);
        }
    }
}