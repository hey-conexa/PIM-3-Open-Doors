using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.TestesVocacionais
{
    public interface ITesteVocacionalRepository
    {
        public Task<List<TesteVocacional>> ListarTodos();
        public Task<TesteVocacional?> BuscarPorEstudante(Guid estudanteId);
        public Task<List<TesteVocacional>> ListarAnalisados();
        public Task Criar(TesteVocacional novoTesteVocacional);
    }
}