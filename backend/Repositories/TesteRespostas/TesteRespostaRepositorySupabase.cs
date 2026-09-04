using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.TestesRespostas;

namespace OpenDoors.Api.Repositories.TesteRespostas
{
    public class TesteRespostaRepositorySupabase : ITesteRespostaRepository
    {
        private readonly Supabase.Client _supabase;

        public TesteRespostaRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<TesteResposta>> ListarTodos()
        {
            var resultado = await _supabase.From<TesteResposta>().Get();
            return resultado.Models;
        }

        public async Task<List<TesteResposta>> ListarPorTeste(int testeId)
        {
            var resultado = await _supabase
                .From<TesteResposta>()
                .Where(r => r.TesteId == testeId)
                .Order(r => r.PerguntaId, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();
            return resultado.Models;
        }
    }
}