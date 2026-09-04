using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Candidaturas;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidaturasController : ControllerBase
    {
        private readonly ICandidaturaService _service;

        public CandidaturasController(ICandidaturaService service)
        {
            _service = service;
        }

        // GET /api/candidaturas — Lista TODAS
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/candidaturas/{id} — Busca uma específica
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var resultado = await _service.BuscarPorId(id);
            return Ok(resultado);
        }

        // GET /api/candidaturas/estudante/{estudanteId}
        // Lista todas as candidaturas de UM estudante específico
        [HttpGet("estudante/{estudanteId}")]
        public async Task<IActionResult> ListarPorEstudante(Guid estudanteId)
        {
            var resultado = await _service.ListarPorEstudante(estudanteId);
            return Ok(resultado);
        }

        // GET /api/candidaturas/vaga/{vagaId}
        // Lista todas as candidaturas de UMA vaga (empresa usa pra ver candidatos)
        [HttpGet("vaga/{vagaId}")]
        public async Task<IActionResult> ListarPorVaga(int vagaId)
        {
            var resultado = await _service.ListarPorVaga(vagaId);
            return Ok(resultado);
        }

        // GET /api/candidaturas/empresa/{empresaId}
        // Todas as candidaturas que uma empresa recebeu (em todas as suas vagas)
        [HttpGet("empresa/{empresaId}")]
        public async Task<IActionResult> ListarPorEmpresa(Guid empresaId)
        {
            var resultado = await _service.ListarPorEmpresa(empresaId);
            return Ok(resultado);
        }

        // POST /api/candidaturas
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateCandidaturaDto dto)
        {
            var criada = await _service.Criar(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criada.Id }, criada);
        }

        // PUT /api/candidaturas/{id}/status — atualiza só o status (caso de uso comum)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] string novoStatus)
        {
            var atualizada = await _service.AtualizarStatus(id, novoStatus);
            return Ok(atualizada);
        }

        // DELETE /api/candidaturas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _service.Deletar(id);
            return NoContent();
        }
    }
}