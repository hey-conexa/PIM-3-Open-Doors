using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Estudantes
{
    public interface IEstudanteService
    {
        public Task<IReadOnlyList<EstudanteDto>> ListarTodos();
        public Task<IReadOnlyList<EstudanteDto>> ListarAtivos();
        public Task<EstudanteDto> BuscarPorId(Guid id);
        public Task<EstudanteDto> Criar(CreateEstudanteDto novoEstudante);
        public Task<EstudanteDto> Atualizar(CreateEstudanteDto estudanteAtualizado);
        public Task<EstudanteDto> Deletar(Guid id);
    }
}
