using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidaturasController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public CandidaturasController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // ===========================================
        // GET /api/candidaturas — Lista TODAS
        // ===========================================
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            try
            {
                var resultado = await _supabase.From<Candidatura>().Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // GET /api/candidaturas/{id} — Busca uma específica
        // ===========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var resultado = await _supabase
                    .From<Candidatura>()
                    .Where(c => c.Id == id)
                    .Single();

                if (resultado == null)
                    return NotFound(new { mensagem = "Candidatura não encontrada" });

                return Ok(MapearParaDto(resultado));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // GET /api/candidaturas/estudante/{estudanteId}
        // Lista todas as candidaturas de UM estudante específico
        // ===========================================
        [HttpGet("estudante/{estudanteId}")]
        public async Task<IActionResult> ListarPorEstudante(Guid estudanteId)
        {
            try
            {
                var resultado = await _supabase
                    .From<Candidatura>()
                    .Where(c => c.EstudanteId == estudanteId)
                    .Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // GET /api/candidaturas/vaga/{vagaId}
        // Lista todas as candidaturas de UMA vaga (empresa usa pra ver candidatos)
        // ===========================================
        [HttpGet("vaga/{vagaId}")]
        public async Task<IActionResult> ListarPorVaga(int vagaId)
        {
            try
            {
                var resultado = await _supabase
                    .From<Candidatura>()
                    .Where(c => c.VagaId == vagaId)
                    .Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // GET /api/candidaturas/empresa/{empresaId}
        // Todas as candidaturas que uma empresa recebeu (em todas as suas vagas)
        // ===========================================
        [HttpGet("empresa/{empresaId}")]
        public async Task<IActionResult> ListarPorEmpresa(Guid empresaId)
        {
            try
            {
                var resultado = await _supabase
                    .From<Candidatura>()
                    .Where(c => c.EmpresaId == empresaId)
                    .Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        private static CandidaturaDto MapearParaDto(Candidatura c)
        {
            return new CandidaturaDto
            {
                Id = c.Id,
                EstudanteId = c.EstudanteId,
                VagaId = c.VagaId,
                EmpresaId = c.EmpresaId,
                Status = c.Status,
                CartaApresentacao = c.CartaApresentacao,
                ScoreCompatibilidade = c.ScoreCompatibilidade,
                PosicaoRanking = c.PosicaoRanking,
                VisualizadoEmpresa = c.VisualizadoEmpresa,
                DataVisualizacao = c.DataVisualizacao,
                CriadoEm = c.CriadoEm,
                AtualizadoEm = c.AtualizadoEm
            };
        }
    }
}