using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Jooble;

namespace OpenDoors.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/jooble")]
    public class JoobleController : ControllerBase
    {
        private readonly IJoobleService _service;

        public JoobleController(IJoobleService service)
        {
            _service = service;
        }

        // GET /api/jooble/buscar?keywords=Engenharia&location=São Paulo&page=1
        // Busca vagas na Jooble e retorna direto pro frontend — SEM salvar no banco.
        // Útil para exibir vagas externas ao vivo na home do estudante.
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] JoobleBuscaDto filtros)
        {
            var resultado = await _service.BuscarVagasAsync(filtros);
            return Ok(resultado);
        }

        // POST /api/jooble/sincronizar
        // Busca vagas na Jooble e salva no banco (tabela "vagas").
        // Evita duplicatas pelo título + cidade.
        // Ideal para rodar uma vez ao dia via tarefa agendada (ou manualmente pelo admin).
        //
        // Body esperado:
        // {
        //   "keywords": "Engenharia de Software",
        //   "location": "Brasil",
        //   "page": 1,
        //   "resultsPerPage": 20
        // }
        [HttpPost("sincronizar")]
        public async Task<IActionResult> Sincronizar([FromBody] JoobleBuscaDto filtros)
        {
            var resultado = await _service.SincronizarVagasAsync(filtros);
            return Ok(resultado);
        }
    }
}
