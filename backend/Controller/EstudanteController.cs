using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudantesController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public EstudantesController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // GET /api/estudantes
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var resultado = await _supabase.From<Estudante>().Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // GET /api/estudantes/ativos
        [HttpGet("ativos")]
        public async Task<IActionResult> ListarAtivos()
        {
            try
            {
                var resultado = await _supabase
                    .From<Estudante>()
                    .Where(e => e.Status == "ativo")
                    .Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // GET /api/estudantes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var resultado = await _supabase
                    .From<Estudante>()
                    .Where(e => e.Id == id)
                    .Single();

                if (resultado == null)
                    return NotFound(new { mensagem = "Estudante não encontrado" });

                return Ok(MapearParaDto(resultado));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        private static EstudanteDto MapearParaDto(Estudante e)
        {
            return new EstudanteDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Email = e.Email,
                Telefone = e.Telefone,
                Cpf = e.Cpf,
                DataNascimento = e.DataNascimento,
                Cidade = e.Cidade,
                Estado = e.Estado,
                FotoPerfilUrl = e.FotoPerfilUrl,
                Instituicao = e.Instituicao,
                Curso = e.Curso,
                Semestre = e.Semestre,
                Turno = e.Turno,
                PrevisaoConclusao = e.PrevisaoConclusao,
                CurriculoUrl = e.CurriculoUrl,
                HabilidadesExtraidas = e.HabilidadesExtraidas,
                TemCurriculo = e.TemCurriculo,
                TemTesteVocacional = e.TemTesteVocacional,
                Status = e.Status,
                CriadoEm = e.CriadoEm,
                AtualizadoEm = e.AtualizadoEm
            };
        }
    }
}