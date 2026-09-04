using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Candidaturas
{
    public interface ICandidaturaService
    {
        public Task<IReadOnlyList<CandidaturaDto>> ListarTodos();
        public Task<CandidaturaDto> BuscarPorId(int id);
        public Task<IReadOnlyList<CandidaturaDto>> ListarPorEstudante(Guid estudanteId);
        public Task<IReadOnlyList<CandidaturaDto>> ListarPorVaga(int vagaId);
        public Task<IReadOnlyList<CandidaturaDto>> ListarPorEmpresa(Guid empresaId);
        public Task<CandidaturaDto> Criar(CreateCandidaturaDto novaCandidatura);
        public Task<CandidaturaDto> AtualizarStatus(int id, string novoStatus);
        public Task<CandidaturaDto> Deletar(int id);
    }
}