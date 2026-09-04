using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.IA
{
    public interface IAnalisarTesteService
    {
        public Task<PerfilVocacionalDto> AnalisarAsync(List<RespostaVocacionalDto> respostas);

    }
}
