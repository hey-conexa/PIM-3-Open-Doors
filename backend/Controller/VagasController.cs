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
        // POST /api/vagas — Cria uma nova vaga
        // ===========================================
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateVagaDto dto)
        {
            try
            {
                // Validação básica
                if (string.IsNullOrWhiteSpace(dto.Titulo))
                    return BadRequest(new { erro = "O título da vaga é obrigatório" });

                if (dto.EmpresaId == Guid.Empty)
                    return BadRequest(new { erro = "A empresa é obrigatória" });

                // Cria a Model a partir do DTO de entrada
                var novaVaga = new Vaga
                {
                    EmpresaId = dto.EmpresaId,
                    Titulo = dto.Titulo,
                    Descricao = dto.Descricao,
                    Area = dto.Area,
                    Nivel = dto.Nivel,
                    CursosAceitos = dto.CursosAceitos,
                    SemestreMinimo = dto.SemestreMinimo,
                    HabilidadesRequeridas = dto.HabilidadesRequeridas,
                    HabilidadesDiferenciais = dto.HabilidadesDiferenciais,
                    CargaHoraria = dto.CargaHoraria,
                    Modalidade = dto.Modalidade,
                    Cidade = dto.Cidade,
                    Estado = dto.Estado,
                    Bolsa = dto.Bolsa,
                    Beneficios = dto.Beneficios,
                    VagasDisponiveis = dto.VagasDisponiveis,
                    CandidaturasRecebidas = 0, // Sempre começa em zero
                    Status = dto.Status ?? "aberta", // Default "aberta"
                    ExpiraEm = dto.ExpiraEm
                };

                // Insere no Supabase
                var resultado = await _supabase.From<Vaga>().Insert(novaVaga);

                if (resultado.Models == null || resultado.Models.Count == 0)
                    return StatusCode(500, new { erro = "Falha ao criar vaga" });

                // Retorna 201 Created com a vaga criada
                var vagaCriada = MapearParaDto(resultado.Models[0]);
                return CreatedAtAction(nameof(BuscarPorId), new { id = vagaCriada.Id }, vagaCriada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // PUT /api/vagas/{id} — Atualiza uma vaga existente
        // ===========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CreateVagaDto dto)
        {
            try
            {
                // Verifica se a vaga existe
                var existente = await _supabase
                    .From<Vaga>()
                    .Where(v => v.Id == id)
                    .Single();

                if (existente == null)
                    return NotFound(new { mensagem = "Vaga não encontrada" });

                // Atualiza os campos
                existente.Titulo = dto.Titulo;
                existente.Descricao = dto.Descricao;
                existente.Area = dto.Area;
                existente.Nivel = dto.Nivel;
                existente.CursosAceitos = dto.CursosAceitos;
                existente.SemestreMinimo = dto.SemestreMinimo;
                existente.HabilidadesRequeridas = dto.HabilidadesRequeridas;
                existente.HabilidadesDiferenciais = dto.HabilidadesDiferenciais;
                existente.CargaHoraria = dto.CargaHoraria;
                existente.Modalidade = dto.Modalidade;
                existente.Cidade = dto.Cidade;
                existente.Estado = dto.Estado;
                existente.Bolsa = dto.Bolsa;
                existente.Beneficios = dto.Beneficios;
                existente.VagasDisponiveis = dto.VagasDisponiveis;
                existente.Status = dto.Status;
                existente.ExpiraEm = dto.ExpiraEm;

                var resultado = await existente.Update<Vaga>();

                return Ok(MapearParaDto(resultado.Models[0]));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ===========================================
        // DELETE /api/vagas/{id} — Deleta uma vaga
        // ===========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var existente = await _supabase
                    .From<Vaga>()
                    .Where(v => v.Id == id)
                    .Single();

                if (existente == null)
                    return NotFound(new { mensagem = "Vaga não encontrada" });

                await _supabase
                    .From<Vaga>()
                    .Where(v => v.Id == id)
                    .Delete();

                return NoContent(); // HTTP 204
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