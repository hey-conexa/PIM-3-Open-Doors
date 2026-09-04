using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Notificacoes;

namespace OpenDoors.Api.Repositories
{
    public class NotificacaoRepositorySupabase : INotificacaoRepository
    {
        private readonly Supabase.Client _supabase;

        public NotificacaoRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Notificacao>> ListarTodos()
        {
            var resultado = await _supabase.From<Notificacao>().Get();
            return resultado.Models;
        }

        public async Task<List<Notificacao>> ListarPorEstudante(Guid estudanteId)
        {
            var resultado = await _supabase
                .From<Notificacao>()
                .Where(n => n.EstudanteId == estudanteId)
                .Order(n => n.CriadoEm!, Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();
            return resultado.Models;
        }

        public async Task<List<Notificacao>> ListarPorEmpresa(Guid empresaId)
        {
            var resultado = await _supabase
                .From<Notificacao>()
                .Where(n => n.EmpresaId == empresaId)
                .Order(n => n.CriadoEm!, Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();
            return resultado.Models;
        }

        public async Task<List<Notificacao>> NaoLidasEstudante(Guid estudanteId)
        {
            var resultado = await _supabase
                .From<Notificacao>()
                .Where(n => n.EstudanteId == estudanteId && n.Lida == false)
                .Get();
            return resultado.Models;
        }
    }
}