using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.CandidaturasHistorico
{
    public interface ICandidaturaHistoricoRepository
    {
        public Task<List<CandidaturaHistorico>> ListarTodos();
        public Task<List<CandidaturaHistorico>> ListarPorCandidatura(int candidaturaId);
    }
}