using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/testes-respostas")]
    public class TestesRespostasController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public TestesRespostasController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // GET /api/testes-respostas
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            try
            {
                var resultado = await _supabase.From<TesteResposta>().Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // GET /api/testes-respostas/teste/{testeId}
        // Todas as respostas de um teste específico
        [HttpGet("teste/{testeId}")]
        public async Task<IActionResult> ListarPorTeste(int testeId)
        {
            try
            {
                var resultado = await _supabase
                    .From<TesteResposta>()
                    .Where(r => r.TesteId == testeId)
                    .Order(r => r.PerguntaId, Supabase.Postgrest.Constants.Ordering.Ascending)
                    .Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        private static TesteRespostaDto MapearParaDto(TesteResposta r)
        {
            return new TesteRespostaDto
            {
                Id = r.Id,
                TesteId = r.TesteId,
                PerguntaId = r.PerguntaId,
                Pergunta = r.Pergunta,
                Resposta = r.Resposta
            };
        }
    }
}