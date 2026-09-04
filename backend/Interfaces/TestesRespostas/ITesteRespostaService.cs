using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.TestesRespostas
{
    public interface ITesteRespostaService
    {
        public Task<IReadOnlyList<TesteRespostaDto>> ListarTodos();
        public Task<IReadOnlyList<TesteRespostaDto>> ListarPorTeste(int testeId);
    }
}