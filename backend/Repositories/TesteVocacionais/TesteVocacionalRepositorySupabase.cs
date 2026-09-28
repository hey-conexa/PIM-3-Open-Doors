using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.TestesVocacionais;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Repositories.TesteVocacionais
{
    public class TesteVocacionalRepositorySupabase : ITesteVocacionalRepository
    {
        private readonly Supabase.Client _supabase;

        public TesteVocacionalRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<TesteVocacional>> ListarTodos()
        {
            var resultado = await _supabase.From<TesteVocacional>().Get();
            return resultado.Models;
        }

        public async Task<TesteVocacional?> BuscarPorEstudanteId(Guid estudanteId)
        {
            var resultado = await _supabase
                .From<TesteVocacional>()
                .Where(t => t.EstudanteId == estudanteId)
                .Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new NotFoundException("Estudante ainda não fez o teste vocacional");

            return resultado.Model;
        }

        public async Task<List<TesteVocacional>> ListarAnalisados()
        {
            var resultado = await _supabase
                .From<TesteVocacional>()
                .Where(t => t.AnalisadoIa == true)
                .Get();
            return resultado.Models;
        }

        public async Task<TesteVocacional> Criar(TesteVocacional novoTesteVocacional)
        {
            var resultado = await _supabase.From<TesteVocacional>().Insert(novoTesteVocacional);
            // Supondo que o Insert retorna um objeto com a lista de modelos inseridos
            // Retorne o primeiro item inserido, ou lance exceção se não houver retorno
            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new Exception("Falha ao criar TesteVocacional.");
            return resultado.Models[0];
        }

        public async Task<TesteVocacional> Atualizar(TesteVocacional teste)
        {
            var resultado = await _supabase.From<TesteVocacional>().Update(teste);
            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new Exception("Falha ao atualizar TesteVocacional.");
            return resultado.Models[0];
        }
    }
}