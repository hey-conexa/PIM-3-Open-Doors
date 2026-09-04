using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Notificacoes
{
    public interface INotificacaoService
    {
        public Task<IReadOnlyList<NotificacaoDto>> ListarTodos();
        public Task<IReadOnlyList<NotificacaoDto>> ListarPorEstudante(Guid estudanteId);
        public Task<IReadOnlyList<NotificacaoDto>> ListarPorEmpresa(Guid empresaId);
        public Task<IReadOnlyList<NotificacaoDto>> NaoLidasEstudante(Guid estudanteId);
    }
}