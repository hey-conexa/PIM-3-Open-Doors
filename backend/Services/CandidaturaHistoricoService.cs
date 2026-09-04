using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.CandidaturasHistorico;

namespace OpenDoors.Api.Services
{
    public class CandidaturaHistoricoService : ICandidaturaHistoricoService
    {
        private readonly ICandidaturaHistoricoRepository _repository;

        public CandidaturaHistoricoService(ICandidaturaHistoricoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<CandidaturaHistoricoDto>> ListarTodos()
        {
            var historicos = await _repository.ListarTodos();
            return historicos.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<CandidaturaHistoricoDto>> ListarPorCandidatura(int candidaturaId)
        {
            var historicos = await _repository.ListarPorCandidatura(candidaturaId);
            return historicos.Select(MapearParaDto).ToList();
        }

        private static CandidaturaHistoricoDto MapearParaDto(CandidaturaHistorico h)
        {
            return new CandidaturaHistoricoDto
            {
                Id = h.Id,
                CandidaturaId = h.CandidaturaId,
                Status = h.Status,
                Observacao = h.Observacao,
                CriadoEm = h.CriadoEm
            };
        }
    }
}