using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.PerguntasTeste;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/perguntas-teste")]
    public class PerguntasTesteController : ControllerBase
    {
        private readonly IPerguntaTesteService _service;

        public PerguntasTesteController(IPerguntaTesteService service)
        {
            _service = service;
        }

        // GET /api/perguntas-teste
        // Retorna perguntas ativas em ordem.
        // Parâmetros opcionais:
        //   ?mes=2025-06        → só perguntas mensais daquele mês
        //   ?tipo=fixa          → só perguntas fixas (RIASEC / Big Five)
        //   ?tipo=mensal        → só perguntas mensais
        //   (sem parâmetros)    → todas as ativas (fixas + mês atual)
        [HttpGet]
        public async Task<IActionResult> ListarAtivas([FromQuery] string? mes, [FromQuery] string? tipo)
        {
            var resultado = await _service.ListarAtivas(mes, tipo);
            return Ok(resultado);
        }

        // GET /api/perguntas-teste/todas
        // Todas as perguntas (admin)
        [HttpGet("todas")]
        public async Task<IActionResult> ListarTodas()
        {
            var resultado = await _service.ListarTodas();
            return Ok(resultado);
        }

        // POST /api/perguntas-teste
        // Cria nova pergunta manualmente
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreatePerguntaTesteDto body)
        {
            var perguntaCriada = await _service.Criar(body);
            return CreatedAtAction(nameof(ListarAtivas), perguntaCriada);
        }

        // PUT /api/perguntas-teste/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CreatePerguntaTesteDto body)
        {
            var resultado = await _service.Atualizar(id, body);
            return Ok(resultado);
        }

        // DELETE /api/perguntas-teste/{id}
        // Soft delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(int id)
        {
            var resultado = await _service.Desativar(id);
            return Ok(resultado);
        }
    }
}
