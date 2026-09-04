using OpenDoors.Api.Exceptions;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Repositories.Estudantes
{
    public class EstudanteRepositorySupabase : IEstudanteRepository
    {
        private readonly Supabase.Client _supabase;

        public EstudanteRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Estudante>> ListarTodos()
        {
            var resultado = await _supabase.From<Estudante>().Get();
            return resultado.Models;
        }

        public async Task<List<Estudante>> ListarAtivos()
        {
            var resultado = await _supabase.From<Estudante>().Where(e => e.Status == "ativo").Get();
            return resultado.Models;
        }

        public async Task<Estudante?> BuscarPorId(Guid id)
        {
            var resultado = await _supabase.From<Estudante>().Where(e => e.Id.Equals(id)).Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new ServerErrorException("Falha ao criar estudante");

            return resultado.Model;
        }

        public async Task Criar(Estudante novoEstudante)
        {
            await _supabase.From<Estudante>().Insert(novoEstudante);
        }

        public async Task Atualizar(Estudante estudanteAtualizado)
        {
            await _supabase.From<Estudante>().Update(estudanteAtualizado);
        }

        public async Task Deletar(Estudante estudante)
        {
            await _supabase.From<Estudante>().Delete(estudante);
        }
    }
}
