using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.CandidaturasHistorico;

namespace OpenDoors.Api.Repositories.CandidaturaHistoricos
{
    public class CandidaturaHistoricoRepositorySupabase : ICandidaturaHistoricoRepository
    {
        private readonly Supabase.Client _supabase;

        public CandidaturaHistoricoRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<CandidaturaHistorico>> ListarTodos()
        {
            var resultado = await _supabase.From<CandidaturaHistorico>().Get();
            return resultado.Models;
        }

        public async Task<List<CandidaturaHistorico>> ListarPorCandidatura(int candidaturaId)
        {
            var resultado = await _supabase
                .From<CandidaturaHistorico>()
                .Where(h => h.CandidaturaId == candidaturaId)
                .Order(h => h.CriadoEm!, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();
            return resultado.Models;
        }
    }
}