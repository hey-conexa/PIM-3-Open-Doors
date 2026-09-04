using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudanteController : ControllerBase
    {
        private readonly Supabase.Client _supabase;
        private readonly IEstudanteService _service;

        public EstudanteController(Supabase.Client supabase, IEstudanteService service)
        {
            _supabase = supabase;
            _service = service;
        }

        // GET /api/estudantes
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/estudantes/ativos
        [HttpGet("ativos")]
        public async Task<IActionResult> ListarAtivos()
        {
            var resultado = await _service.ListarAtivos();
            return Ok(resultado);
        }

        // GET /api/estudantes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);
            return Ok(resultado);
        }

        // POST /api/estudantes
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateEstudanteDto dto)
        {
            var criado = await _service.Criar(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
        }

        // PUT /api/estudantes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CreateEstudanteDto dto)
        {
            var resultado = await _service.Atualizar(dto, id);
            return Ok(resultado);
        }

        // DELETE /api/estudantes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var usuarioDeletado = await _service.Deletar(id);
            return NoContent();
        }
    }
}