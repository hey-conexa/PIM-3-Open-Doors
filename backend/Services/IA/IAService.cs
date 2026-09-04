using OpenDoors.Api.DTOs;
using OpenDoors.Api.Exceptions;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Interfaces.IA;
using OpenDoors.Api.Interfaces.TestesVocacionais;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Services.IA
{
    public class IAService : IIAService
    {
        private readonly IAnalisarCurriculoService _curriculoService;
        private readonly IAnalisarTesteService _testeService;
        private readonly ITesteVocacionalRepository _testeVocacionalRepository;
        private readonly IEstudanteRepository _estudanteRepository;
        private readonly IGerarScoreService _scoreService;

        public IAService(
            IAnalisarCurriculoService curriculoService,
            IAnalisarTesteService analisarTeste,
            ITesteVocacionalRepository testeVocacionalRepository,
            IEstudanteRepository estudanteRepository,
            IGerarScoreService scoreService
            )
        {
            _curriculoService = curriculoService;
            _testeService = analisarTeste;
            _scoreService = scoreService;
            _estudanteRepository = estudanteRepository;
            _testeVocacionalRepository = testeVocacionalRepository;
        }

        public async Task<CurriculoAnalisadoDto> AnalisarCurriculo(Guid estudanteId, IFormFile curriculo)
        {
            if (estudanteId == Guid.Empty || curriculo == null)
                throw new BadRequestException("estudanteId e curriculo são obrigatórios");

            await using var stream = curriculo.OpenReadStream();
            var resultado = await _curriculoService.AnalisarAsync(estudanteId, stream);
            return resultado;
        }

        public async Task<PerfilVocacionalDto> AnalisarTeste(AnalisarTesteRequestDto request)
        {
            if (request.EstudanteId == Guid.Empty || request.Respostas.Count == 0)
                throw new BadRequestException("estudanteId e respostas são obrigatórios");

            var estudante = await _estudanteRepository.BuscarPorId(request.EstudanteId);
            if (estudante == null)
                throw new NotFoundException("Usuário não foi encontrado");

            var resultado = await _testeService.AnalisarAsync(request.Respostas);

            var testeExistente = await _testeVocacionalRepository.BuscarPorEstudante(request.EstudanteId);

            if (testeExistente != null)
            {
                testeExistente.PerfilDominante = resultado.PerfilDominante;
                testeExistente.AreasSugeridas = resultado.AreasSugeridas;
                testeExistente.PontosFortes = resultado.PontosFortes;
                testeExistente.DescricaoPerfil = resultado.DescricaoPerfil;
                testeExistente.AnalisadoIa = true;
                await testeExistente.Update<TesteVocacional>();
            }
            else
            {
                await _testeVocacionalRepository.Criar(new TesteVocacional
                {
                    EstudanteId = request.EstudanteId,
                    PerfilDominante = resultado.PerfilDominante,
                    AreasSugeridas = resultado.AreasSugeridas,
                    PontosFortes = resultado.PontosFortes,
                    DescricaoPerfil = resultado.DescricaoPerfil,
                    AnalisadoIa = true
                });
            }

            // Marca o estudante como tendo teste vocacional
            estudante.TemTesteVocacional = true;
            await _estudanteRepository.Atualizar(estudante);

            return resultado;
        }
        public async Task<ScoreCompatibilidadeDto> GerarScore(GerarScoreRequestDto body)
        {
            if (body.EstudanteId == Guid.Empty || body.VagaId == 0)
                throw new BadRequestException("estudanteId e vagaId são obrigatórios");

            var resultado = await _scoreService.GerarAsync(body.EstudanteId, body.VagaId);
            return resultado;
    }
}
