using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.Interfaces.TestesRespostas;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/testes-respostas")]
    public class TestesRespostasController : ControllerBase
    {
        private readonly ITesteRespostaService _service;

        public TestesRespostasController(ITesteRespostaService service)
        {
            _service = service;
        }

        // GET /api/testes-respostas
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/testes-respostas/teste/{testeId}
        // Todas as respostas de um teste específico
        [HttpGet("teste/{testeId}")]
        public async Task<IActionResult> ListarPorTeste(int testeId)
        {
            var resultado = await _service.ListarPorTeste(testeId);
            return Ok(resultado);
        }
    }
}