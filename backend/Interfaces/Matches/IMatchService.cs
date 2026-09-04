using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Matches
{
    public interface IMatchService
    {
        public Task<IReadOnlyList<MatchDto>> ListarTodos();
        public Task<MatchDto> BuscarPorId(int id);
        public Task<IReadOnlyList<MatchDto>> ListarPorEstudante(Guid estudanteId);
        public Task<IReadOnlyList<MatchDto>> ListarPorVaga(int vagaId);
        public Task<IReadOnlyList<MatchDto>> TopMatchesEstudante(Guid estudanteId);
        public Task<IReadOnlyList<MatchDto>> ListarExcelentes();
        public Task<MatchDto> Criar(CreateMatchDto novoMatch);
        public Task<MatchDto> Deletar(int id);
    }
}