using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.Interfaces.TestesVocacionais;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/testes-vocacionais")]
    public class TestesVocacionaisController : ControllerBase
    {
        private readonly ITesteVocacionalService _service;

        public TestesVocacionaisController(ITesteVocacionalService service)
        {
            _service = service;
        }

        // GET /api/testes-vocacionais
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/testes-vocacionais/estudante/{estudanteId}
        // Busca o teste de um estudante (só tem 1 por estudante)
        [HttpGet("estudante/{estudanteId}")]
        public async Task<IActionResult> BuscarPorEstudante(Guid estudanteId)
        {
            var resultado = await _service.BuscarPorEstudante(estudanteId);
            return Ok(resultado);
        }

        // GET /api/testes-vocacionais/analisados
        // Testes que já passaram pela análise da IA
        [HttpGet("analisados")]
        public async Task<IActionResult> ListarAnalisados()
        {
            var resultado = await _service.ListarAnalisados();
            return Ok(resultado);
        }
    }
}