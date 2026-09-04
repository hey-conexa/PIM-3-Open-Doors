using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Notificacoes;

namespace OpenDoors.Api.Services.Notificacoes
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly INotificacaoRepository _repository;

        public NotificacaoService(INotificacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<NotificacaoDto>> ListarTodos()
        {
            var notificacoes = await _repository.ListarTodos();
            return notificacoes.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<NotificacaoDto>> ListarPorEstudante(Guid estudanteId)
        {
            var notificacoes = await _repository.ListarPorEstudante(estudanteId);
            return notificacoes.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<NotificacaoDto>> ListarPorEmpresa(Guid empresaId)
        {
            var notificacoes = await _repository.ListarPorEmpresa(empresaId);
            return notificacoes.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<NotificacaoDto>> NaoLidasEstudante(Guid estudanteId)
        {
            var notificacoes = await _repository.NaoLidasEstudante(estudanteId);
            return notificacoes.Select(MapearParaDto).ToList();
        }

        private static NotificacaoDto MapearParaDto(Notificacao n)
        {
            return new NotificacaoDto
            {
                Id = n.Id,
                DestinatarioTipo = n.DestinatarioTipo,
                EstudanteId = n.EstudanteId,
                EmpresaId = n.EmpresaId,
                Tipo = n.Tipo,
                Titulo = n.Titulo,
                Mensagem = n.Mensagem,
                ReferenciaTabela = n.ReferenciaTabela,
                ReferenciaId = n.ReferenciaId,
                Lida = n.Lida,
                DataLeitura = n.DataLeitura,
                CriadoEm = n.CriadoEm
            };
        }
    }
}