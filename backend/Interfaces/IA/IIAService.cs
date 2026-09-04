using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.IA
{
    public interface IIAService
    {
        public Task<CurriculoAnalisadoDto> AnalisarCurriculo(Guid estudanteId, IFormFile curriculo);
        public Task<PerfilVocacionalDto> AnalisarTeste(AnalisarTesteRequestDto request);
        public Task<ScoreCompatibilidadeDto> GerarScore(GerarScoreRequestDto body);
    }
}
