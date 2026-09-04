using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Matches;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _service;

        public MatchesController(IMatchService service)
        {
            _service = service;
        }

        // GET /api/matches — Lista TODOS os matches
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/matches/{id} — Busca um match específico
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var resultado = await _service.BuscarPorId(id);
            return Ok(resultado);
        }

        // GET /api/matches/estudante/{estudanteId}
        // Matches que o estudante recebeu (ordenado por score do maior pro menor)
        // É a "lista de recomendações" pro estudante
        [HttpGet("estudante/{estudanteId}")]
        public async Task<IActionResult> ListarPorEstudante(Guid estudanteId)
        {
            var resultado = await _service.ListarPorEstudante(estudanteId);
            return Ok(resultado);
        }

        // GET /api/matches/vaga/{vagaId}
        // Matches gerados pra uma vaga (a empresa vê os melhores candidatos)
        [HttpGet("vaga/{vagaId}")]
        public async Task<IActionResult> ListarPorVaga(int vagaId)
        {
            var resultado = await _service.ListarPorVaga(vagaId);
            return Ok(resultado);
        }

        // GET /api/matches/top/{estudanteId}
        // Top 5 melhores matches pro estudante (página inicial dele!)
        [HttpGet("top/{estudanteId}")]
        public async Task<IActionResult> TopMatchesEstudante(Guid estudanteId)
        {
            var resultado = await _service.TopMatchesEstudante(estudanteId);
            return Ok(resultado);
        }

        // GET /api/matches/excelentes
        // Matches com score acima de 80 (recomendações fortíssimas)
        [HttpGet("excelentes")]
        public async Task<IActionResult> ListarExcelentes()
        {
            var resultado = await _service.ListarExcelentes();
            return Ok(resultado);
        }

        // POST /api/matches
        // Endpoint usado pela IA (próxima etapa) pra gravar resultados de análise
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateMatchDto dto)
        {
            var criado = await _service.Criar(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
        }

        // DELETE /api/matches/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _service.Deletar(id);
            return NoContent();
        }
    }
}