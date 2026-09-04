using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.CandidaturasHistorico
{
    public interface ICandidaturaHistoricoService
    {
        public Task<IReadOnlyList<CandidaturaHistoricoDto>> ListarTodos();
        public Task<IReadOnlyList<CandidaturaHistoricoDto>> ListarPorCandidatura(int candidaturaId);
    }
}