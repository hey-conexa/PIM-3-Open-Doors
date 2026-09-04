using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Vagas
{
    public interface IVagaRepository
    {
        public Task<List<Vaga>> ListarTodos();
        public Task<List<Vaga>> ListarAbertas();
        public Task<Vaga?> BuscarPorId(int id);
        public Task Criar(Vaga novaVaga);
        public Task Atualizar(Vaga vagaAtualizada);
        public Task Deletar(Vaga vaga);
    }
}