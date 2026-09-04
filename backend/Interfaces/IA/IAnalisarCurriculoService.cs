using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.IA
{
    public interface IAnalisarCurriculoService
    {
        public Task<CurriculoAnalisadoDto> AnalisarAsync(Guid estudanteId, Stream pdfStream);
    }
}
