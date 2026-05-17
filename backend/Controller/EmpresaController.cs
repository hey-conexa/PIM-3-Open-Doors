using Microsoft.AspNetCore.Mvc;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public EmpresasController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // GET /api/empresas
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            try
            {
                var resultado = await _supabase.From<Empresa>().Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // GET /api/empresas/ativas
        [HttpGet("ativas")]
        public async Task<IActionResult> ListarAtivas()
        {
            try
            {
                var resultado = await _supabase
                    .From<Empresa>()
                    .Where(e => e.Status == "ativa")
                    .Get();
                var dto = resultado.Models.Select(MapearParaDto).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // GET /api/empresas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var resultado = await _supabase
                    .From<Empresa>()
                    .Where(e => e.Id == id)
                    .Single();

                if (resultado == null)
                    return NotFound(new { mensagem = "Empresa não encontrada" });

                return Ok(MapearParaDto(resultado));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        private static EmpresaDto MapearParaDto(Empresa e)
        {
            return new EmpresaDto
            {
                Id = e.Id,
                RazaoSocial = e.RazaoSocial,
                NomeFantasia = e.NomeFantasia,
                Cnpj = e.Cnpj,
                Email = e.Email,
                Telefone = e.Telefone,
                Site = e.Site,
                LogoUrl = e.LogoUrl,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Cep = e.Cep,
                Setor = e.Setor,
                Porte = e.Porte,
                Descricao = e.Descricao,
                ResponsavelNome = e.ResponsavelNome,
                ResponsavelCargo = e.ResponsavelCargo,
                ResponsavelEmail = e.ResponsavelEmail,
                VagasAtivas = e.VagasAtivas,
                TotalContratacoes = e.TotalContratacoes,
                Status = e.Status,
                CriadoEm = e.CriadoEm,
                AtualizadoEm = e.AtualizadoEm
            };
        }
    }
}