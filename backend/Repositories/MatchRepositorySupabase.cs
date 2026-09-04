using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Matches;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Repositories
{
    public class MatchRepositorySupabase : IMatchRepository
    {
        private readonly Supabase.Client _supabase;

        public MatchRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Match>> ListarTodos()
        {
            var resultado = await _supabase.From<Match>().Get();
            return resultado.Models;
        }

        public async Task<Match?> BuscarPorId(int id)
        {
            var resultado = await _supabase.From<Match>().Where(m => m.Id == id).Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new ServerErrorException("Falha ao buscar match");

            return resultado.Model;
        }

        public async Task<List<Match>> ListarPorEstudante(Guid estudanteId)
        {
            var resultado = await _supabase
                .From<Match>()
                .Where(m => m.EstudanteId == estudanteId)
                .Order(m => m.ScoreTotal, Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();
            return resultado.Models;
        }

        public async Task<List<Match>> ListarPorVaga(int vagaId)
        {
            var resultado = await _supabase
                .From<Match>()
                .Where(m => m.VagaId == vagaId)
                .Order(m => m.ScoreTotal, Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();
            return resultado.Models;
        }

        public async Task<List<Match>> TopMatchesEstudante(Guid estudanteId)
        {
            var resultado = await _supabase
                .From<Match>()
                .Where(m => m.EstudanteId == estudanteId)
                .Order(m => m.ScoreTotal, Supabase.Postgrest.Constants.Ordering.Descending)
                .Limit(5)
                .Get();
            return resultado.Models;
        }

        public async Task<List<Match>> ListarExcelentes()
        {
            var resultado = await _supabase
                .From<Match>()
                .Where(m => m.ScoreTotal >= 80)
                .Order(m => m.ScoreTotal, Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();
            return resultado.Models;
        }

        public async Task Criar(Match novoMatch)
        {
            await _supabase.From<Match>().Insert(novoMatch);
        }

        public async Task Deletar(Match match)
        {
            await _supabase.From<Match>().Delete(match);
        }
    }
}