using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.Interfaces.CandidaturasHistorico;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/candidaturas-historico")]
    public class CandidaturasHistoricoController : ControllerBase
    {
        private readonly ICandidaturaHistoricoService _service;

        public CandidaturasHistoricoController(ICandidaturaHistoricoService service)
        {
            _service = service;
        }

        // GET /api/candidaturas-historico
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/candidaturas-historico/candidatura/{candidaturaId}
        // Timeline de uma candidatura específica (pra mostrar evolução)
        [HttpGet("candidatura/{candidaturaId}")]
        public async Task<IActionResult> ListarPorCandidatura(int candidaturaId)
        {
            var resultado = await _service.ListarPorCandidatura(candidaturaId);
            return Ok(resultado);
        }
    }
}