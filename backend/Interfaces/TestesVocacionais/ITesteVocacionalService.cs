using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.TestesVocacionais
{
    public interface ITesteVocacionalService
    {
        public Task<IReadOnlyList<TesteVocacionalDto>> ListarTodos();
        public Task<TesteVocacionalDto> BuscarPorEstudante(Guid estudanteId);
        public Task<IReadOnlyList<TesteVocacionalDto>> ListarAnalisados();
    }
}