using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Notificacoes
{
    public interface INotificacaoRepository
    {
        public Task<List<Notificacao>> ListarTodos();
        public Task<List<Notificacao>> ListarPorEstudante(Guid estudanteId);
        public Task<List<Notificacao>> ListarPorEmpresa(Guid empresaId);
        public Task<List<Notificacao>> NaoLidasEstudante(Guid estudanteId);
    }
}