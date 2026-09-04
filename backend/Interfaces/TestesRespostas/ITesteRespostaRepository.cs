using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.TestesRespostas
{
    public interface ITesteRespostaRepository
    {
        public Task<List<TesteResposta>> ListarTodos();
        public Task<List<TesteResposta>> ListarPorTeste(int testeId);
    }
}