using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Matches;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _repository;

        public MatchService(IMatchRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<MatchDto>> ListarTodos()
        {
            var matches = await _repository.ListarTodos();
            return matches.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<MatchDto>> ListarPorEstudante(Guid estudanteId)
        {
            var matches = await _repository.ListarPorEstudante(estudanteId);
            return matches.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<MatchDto>> ListarPorVaga(int vagaId)
        {
            var matches = await _repository.ListarPorVaga(vagaId);
            return matches.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<MatchDto>> TopMatchesEstudante(Guid estudanteId)
        {
            var matches = await _repository.TopMatchesEstudante(estudanteId);
            return matches.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<MatchDto>> ListarExcelentes()
        {
            var matches = await _repository.ListarExcelentes();
            return matches.Select(MapearParaDto).ToList();
        }

        private async Task<Match> ObterMatchModelPorId(int id)
        {
            var match = await _repository.BuscarPorId(id);
            if (match == null)
                throw new NotFoundException("Match não encontrado");

            return match;
        }

        public async Task<MatchDto> BuscarPorId(int id)
        {
            var match = await ObterMatchModelPorId(id);
            return MapearParaDto(match);
        }

        public async Task<MatchDto> Criar(CreateMatchDto novoMatch)
        {
            if (novoMatch.EstudanteId == Guid.Empty || novoMatch.EmpresaId == Guid.Empty || novoMatch.VagaId <= 0)
                throw new BadRequestException("EstudanteId, VagaId e EmpresaId são obrigatórios");

            if (novoMatch.ScoreTotal < 0 || novoMatch.ScoreTotal > 100)
                throw new BadRequestException("ScoreTotal deve estar entre 0 e 100");

            var novo = new Match
            {
                EstudanteId = novoMatch.EstudanteId,
                VagaId = novoMatch.VagaId,
                EmpresaId = novoMatch.EmpresaId,
                ScoreTotal = novoMatch.ScoreTotal,
                ScoreCurriculo = novoMatch.ScoreCurriculo,
                ScoreVocacional = novoMatch.ScoreVocacional,
                ScoreHabilidades = novoMatch.ScoreHabilidades,
                PontosFortes = novoMatch.PontosFortes,
                PontosFracos = novoMatch.PontosFracos,
                Justificativa = novoMatch.Justificativa
            };

            await _repository.Criar(novo);

            return MapearParaDto(novo);
        }

        public async Task<MatchDto> Deletar(int id)
        {
            var match = await ObterMatchModelPorId(id);
            await _repository.Deletar(match);
            return MapearParaDto(match);
        }

        private static MatchDto MapearParaDto(Match m)
        {
            return new MatchDto
            {
                Id = m.Id,
                EstudanteId = m.EstudanteId,
                VagaId = m.VagaId,
                EmpresaId = m.EmpresaId,
                ScoreTotal = m.ScoreTotal,
                ScoreCurriculo = m.ScoreCurriculo,
                ScoreVocacional = m.ScoreVocacional,
                ScoreHabilidades = m.ScoreHabilidades,
                PontosFortes = m.PontosFortes,
                PontosFracos = m.PontosFracos,
                Justificativa = m.Justificativa,
                GeradoEm = m.GeradoEm,
                AtualizadoEm = m.AtualizadoEm
            };
        }
    }
}