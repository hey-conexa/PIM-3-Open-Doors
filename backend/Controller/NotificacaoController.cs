using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.Interfaces.Notificacoes;

namespace OpenDoors.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacoesController : ControllerBase
    {
        private readonly INotificacaoService _service;

        public NotificacoesController(INotificacaoService service)
        {
            _service = service;
        }

        // GET /api/notificacoes
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var resultado = await _service.ListarTodos();
            return Ok(resultado);
        }

        // GET /api/notificacoes/estudante/{estudanteId}
        [HttpGet("estudante/{estudanteId}")]
        public async Task<IActionResult> ListarPorEstudante(Guid estudanteId)
        {
            var resultado = await _service.ListarPorEstudante(estudanteId);
            return Ok(resultado);
        }

        // GET /api/notificacoes/empresa/{empresaId}
        [HttpGet("empresa/{empresaId}")]
        public async Task<IActionResult> ListarPorEmpresa(Guid empresaId)
        {
            var resultado = await _service.ListarPorEmpresa(empresaId);
            return Ok(resultado);
        }

        // GET /api/notificacoes/nao-lidas/estudante/{estudanteId}
        // Útil pro badge "você tem X notificações novas"
        [HttpGet("nao-lidas/estudante/{estudanteId}")]
        public async Task<IActionResult> NaoLidasEstudante(Guid estudanteId)
        {
            var resultado = await _service.NaoLidasEstudante(estudanteId);
            return Ok(resultado);
        }
    }
}