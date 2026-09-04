using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudantesController : ControllerBase
    {
        private readonly Supabase.Client _supabase;
        private readonly IEstudanteService _service;

        public EstudantesController(Supabase.Client supabase, IEstudanteService service)
        {
            _supabase = supabase;
            _service = service;
        }

        // GET /api/estudantes
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var resultado = await _service.ListarTodos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // GET /api/estudantes/ativos
        [HttpGet("ativos")]
        public async Task<IActionResult> ListarAtivos()
        {
            try
            {
                var resultado = await _service.ListarAtivos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // GET /api/estudantes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var resultado = await _service.BuscarPorId(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // POST /api/estudantes
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateEstudanteDto dto)
        {
            try
            {
                var criado = await _service.Criar(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // PUT /api/estudantes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CreateEstudanteDto dto)
        {
            try
            {
                var resultado = await _service.Atualizar(dto, id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // DELETE /api/estudantes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                var usuarioDeletado = await _service.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }
}