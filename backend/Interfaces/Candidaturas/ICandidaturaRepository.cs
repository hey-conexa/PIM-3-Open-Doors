using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Candidaturas
{
    public interface ICandidaturaRepository
    {
        public Task<List<Candidatura>> ListarTodos();
        public Task<Candidatura?> BuscarPorId(int id);
        public Task<List<Candidatura>> ListarPorEstudante(Guid estudanteId);
        public Task<List<Candidatura>> ListarPorVaga(int vagaId);
        public Task<List<Candidatura>> ListarPorEmpresa(Guid empresaId);
        public Task<List<Candidatura>> ListarPorIdEstudanteIdVaga(Guid estudanteId, int vagaId);
        public Task Criar(Candidatura novaCandidatura);
        public Task AtualizarStatus(Candidatura candidaturaAtualizada);
        public Task Deletar(Candidatura candidatura);
    }
}