using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.TestesRespostas;

namespace OpenDoors.Api.Services
{
    public class TesteRespostaService : ITesteRespostaService
    {
        private readonly ITesteRespostaRepository _repository;

        public TesteRespostaService(ITesteRespostaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<TesteRespostaDto>> ListarTodos()
        {
            var respostas = await _repository.ListarTodos();
            return respostas.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<TesteRespostaDto>> ListarPorTeste(int testeId)
        {
            var respostas = await _repository.ListarPorTeste(testeId);
            return respostas.Select(MapearParaDto).ToList();
        }

        private static TesteRespostaDto MapearParaDto(TesteResposta r)
        {
            return new TesteRespostaDto
            {
                Id = r.Id,
                TesteId = r.TesteId,
                PerguntaId = r.PerguntaId,
                Pergunta = r.Pergunta,
                Resposta = r.Resposta
            };
        }
    }
}