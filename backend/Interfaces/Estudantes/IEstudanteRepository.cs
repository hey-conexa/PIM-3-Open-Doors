using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Estudantes
{
    public interface IEstudanteRepository
    {
        public Task<List<Estudante>> ListarTodos();
        public Task<List<Estudante>> ListarAtivos();
        public Task<Estudante?> BuscarPorId(Guid id);
        public Task Criar(Estudante novoEstudante);
        public Task Atualizar(Estudante estudanteAtualizado);
        public Task Deletar(Estudante estudante);
    }
}
