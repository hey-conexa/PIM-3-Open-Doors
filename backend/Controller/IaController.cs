using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.IA;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/ia")]
    public class IaController : ControllerBase
    {
        private readonly IAnalisarIaService _analisarIaService;

        public IaController(IAnalisarIaService analisarIaService)
        {
            _analisarIaService = analisarIaService;
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
            if (estudanteId == Guid.Empty || curriculo == null)
                throw new BadRequestException("estudanteId e curriculo são obrigatórios");

            await using var stream = curriculo.OpenReadStream();
            var resultado = await _analisarIaService.AnalisarCurriculoAsync(estudanteId, stream, curriculo.FileName ?? "curriculo.pdf");
            
            return Ok(resultado);
        }

        // ===========================================
        // POST /api/ia/analisar-teste
        // Recebe respostas do teste vocacional, gera perfil
        // ===========================================
        [HttpPost("analisar-teste")]
        public async Task<IActionResult> AnalisarTeste([FromBody] AnalisarTesteRequestDto body)
        {
            if (body.EstudanteId == Guid.Empty || body.Respostas == null || body.Respostas.Count == 0)
                throw new BadRequestException("estudanteId e respostas são obrigatórios");

            var resultado = await _analisarIaService.AnalisarTesteVocacionalAsync(body.EstudanteId, body.Respostas);
            
            return Ok(resultado);
        }

        // ===========================================
        // POST /api/ia/gerar-score
        // Calcula score final do estudante
        // ===========================================
        [HttpPost("gerar-score")]
        public async Task<IActionResult> GerarScore([FromBody] GerarScoreRequestDto body)
        {
            if (body.EstudanteId == Guid.Empty)
                throw new BadRequestException("estudanteId é obrigatório");

            var resultado = await _analisarIaService.GerarScoreAsync(body.EstudanteId, body.VagaId);
            
            return Ok(resultado);
        }
    }
}
