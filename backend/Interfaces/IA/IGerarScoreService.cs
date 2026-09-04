using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.IA
{
    public interface IGerarScoreService
    {
        public Task<ScoreCompatibilidadeDto> GerarAsync(Guid estudanteId, int vagaId);

    }
}
