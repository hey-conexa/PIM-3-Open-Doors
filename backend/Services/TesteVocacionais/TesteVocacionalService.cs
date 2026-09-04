using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.TestesVocacionais;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services.TesteVocacionais
{
    public class TesteVocacionalService : ITesteVocacionalService
    {
        private readonly ITesteVocacionalRepository _repository;

        public TesteVocacionalService(ITesteVocacionalRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<TesteVocacionalDto>> ListarTodos()
        {
            var testes = await _repository.ListarTodos();
            return testes.Select(MapearParaDto).ToList();
        }

        public async Task<TesteVocacionalDto> BuscarPorEstudante(Guid estudanteId)
        {
            var teste = await _repository.BuscarPorEstudante(estudanteId);
            if (teste == null)
                throw new NotFoundException("Estudante não encontrado.");
            return MapearParaDto(teste);
        }

        public async Task<IReadOnlyList<TesteVocacionalDto>> ListarAnalisados()
        {
            var testes = await _repository.ListarAnalisados();
            return testes.Select(MapearParaDto).ToList();
        }

        private static TesteVocacionalDto MapearParaDto(TesteVocacional t)
        {
            return new TesteVocacionalDto
            {
                Id = t.Id,
                EstudanteId = t.EstudanteId,
                PerfilDominante = t.PerfilDominante,
                AreasSugeridas = t.AreasSugeridas,
                PontosFortes = t.PontosFortes,
                DescricaoPerfil = t.DescricaoPerfil,
                AnalisadoIa = t.AnalisadoIa,
                ConcluidoEm = t.ConcluidoEm
            };
        }
    }
}