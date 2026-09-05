using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Empresas;

namespace OpenDoors.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public EmpresasController(IEmpresaService service)
        {
            _service = service;
        }

        // GET /api/empresas
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/empresas/ativas
        [HttpGet("ativas")]
        public async Task<IActionResult> ListarAtivas()
        {
            var resultado = await _service.ListarAtivos();
            return Ok(resultado);
        }

        // GET /api/empresas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);
            return Ok(resultado);
        }

        // POST /api/empresas
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateEmpresaDto dto)
        {
            var criada = await _service.Criar(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criada.Id }, criada);
        }

        // PUT /api/empresas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CreateEmpresaDto dto)
        {
            var atualizada = await _service.Atualizar(dto, id);
            return Ok(atualizada);
        }

        // DELETE /api/empresas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _service.Deletar(id);
            return NoContent();
        }
    }
}