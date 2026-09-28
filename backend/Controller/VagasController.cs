using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Vagas;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VagasController : ControllerBase
    {
        private readonly IVagaService _service;

        public VagasController(IVagaService service)
        {
            _service = service;
        }

        // ===========================================
        // GET /api/vagas — Lista TODAS as vagas
        // ===========================================
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var resultado = await _service.ListarTodas();
            return Ok(resultado);
        }

        // ===========================================
        // GET /api/vagas/abertas — Lista só as vagas ABERTAS
        // ===========================================
        [HttpGet("abertas")]
        public async Task<IActionResult> ListarAbertas()
        {
            var resultado = await _service.ListarAbertas();
            return Ok(resultado);
        }

        // ===========================================
        // GET /api/vagas/empresa/{empresaId} — Vagas de UMA empresa
        // ===========================================
        [HttpGet("empresa/{empresaId}")]
        public async Task<IActionResult> ListarPorEmpresa(Guid empresaId)
        {
            var resultado = await _service.ListarPorEmpresa(empresaId);
            return Ok(resultado);
        }

        // ===========================================
        // GET /api/vagas/{id} — Busca UMA vaga específica
        // ===========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var resultado = await _service.BuscarPorId(id);
            return Ok(resultado);
        }

        // ===========================================
        // POST /api/vagas — Cria uma nova vaga
        // ===========================================
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateVagaDto dto)
        {
            var vagaCriada = await _service.Criar(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = vagaCriada.Id }, vagaCriada);
        }

        // ===========================================
        // PUT /api/vagas/{id} — Atualiza uma vaga existente
        // ===========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CreateVagaDto dto)
        {
            var resultado = await _service.Atualizar(id, dto);
            return Ok(resultado);
        }

        // ===========================================
        // PATCH /api/vagas/{id}/status — Atualiza só o status
        // ===========================================
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] string novoStatus)
        {
            var resultado = await _service.AtualizarStatus(id, novoStatus);
            return Ok(resultado);
        }

        // ===========================================
        // DELETE /api/vagas/{id} — Deleta uma vaga
        // ===========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _service.Deletar(id);
            return NoContent();
        }
    }
}
