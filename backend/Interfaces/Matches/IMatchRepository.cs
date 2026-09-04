using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Matches
{
    public interface IMatchRepository
    {
        public Task<List<Match>> ListarTodos();
        public Task<Match?> BuscarPorId(int id);
        public Task<List<Match>> ListarPorEstudante(Guid estudanteId);
        public Task<List<Match>> ListarPorVaga(int vagaId);
        public Task<List<Match>> TopMatchesEstudante(Guid estudanteId);
        public Task<List<Match>> ListarExcelentes();
        public Task Criar(Match novoMatch);
        public Task Deletar(Match match);
    }
}