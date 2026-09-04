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

        // GET /api/vagas — Lista TODAS as vagas
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/vagas/abertas — Lista só as vagas ABERTAS
        [HttpGet("abertas")]
        public async Task<IActionResult> ListarAbertas()
        {
            var resultado = await _service.ListarAbertas();
            return Ok(resultado);
        }

        // GET /api/vagas/{id} — Busca UMA vaga específica
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var resultado = await _service.BuscarPorId(id);
            return Ok(resultado);
        }

        // POST /api/vagas — Cria uma nova vaga
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateVagaDto dto)
        {
            var vagaCriada = await _service.Criar(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = vagaCriada.Id }, vagaCriada);
        }

        // PUT /api/vagas/{id} — Atualiza uma vaga existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CreateVagaDto dto)
        {
            var vagaAtualizada = await _service.Atualizar(dto, id);
            return Ok(vagaAtualizada);
        }

        // DELETE /api/vagas/{id} — Deleta uma vaga
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _service.Deletar(id);
            return NoContent();
        }
    }
}