using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VagasController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public VagasController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // ===========================================
        // GET /api/vagas — Lista TODAS as vagas
        // ===========================================
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Get();

                // Converte cada Model em DTO (sem os campos internos do Supabase)
                var vagasDto = resultado.Models.Select(MapearParaDto).ToList();

                return Ok(vagasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // GET /api/vagas/abertas — Lista só as vagas ABERTAS
        // ===========================================
        [HttpGet("abertas")]
        public async Task<IActionResult> ListarAbertas()
        {
            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Where(v => v.Status == "aberta")
                    .Get();

                var vagasDto = resultado.Models.Select(MapearParaDto).ToList();

                return Ok(vagasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // GET /api/vagas/{id} — Busca UMA vaga específica
        // ===========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Where(v => v.Id == id)
                    .Single();

                if (resultado == null)
                    return NotFound(new { mensagem = "Vaga não encontrada" });

                return Ok(MapearParaDto(resultado));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // MÉTODO PRIVADO — Converte Model em DTO
        // ===========================================
        // Esse método "limpa" os campos internos do Supabase
        // e devolve só o que interessa pro cliente
        private static VagaDto MapearParaDto(Vaga vaga)
        {
            return new VagaDto
            {
                Id = vaga.Id,
                EmpresaId = vaga.EmpresaId,
                Titulo = vaga.Titulo,
                Descricao = vaga.Descricao,
                Area = vaga.Area,
                Nivel = vaga.Nivel,
                CursosAceitos = vaga.CursosAceitos,
                SemestreMinimo = vaga.SemestreMinimo,
                HabilidadesRequeridas = vaga.HabilidadesRequeridas,
                HabilidadesDiferenciais = vaga.HabilidadesDiferenciais,
                CargaHoraria = vaga.CargaHoraria,
                Modalidade = vaga.Modalidade,
                Cidade = vaga.Cidade,
                Estado = vaga.Estado,
                Bolsa = vaga.Bolsa,
                Beneficios = vaga.Beneficios,
                VagasDisponiveis = vaga.VagasDisponiveis,
                CandidaturasRecebidas = vaga.CandidaturasRecebidas,
                Status = vaga.Status,
                CriadoEm = vaga.CriadoEm,
                ExpiraEm = vaga.ExpiraEm,
                AtualizadoEm = vaga.AtualizadoEm
            };
        }
    }
}