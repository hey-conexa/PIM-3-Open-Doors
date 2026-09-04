using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Vagas;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Repositories.Vagas
{
    public class VagaRepositorySupabase : IVagaRepository
    {
        private readonly Supabase.Client _supabase;

        public VagaRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Vaga>> ListarTodos()
        {
            var resultado = await _supabase.From<Vaga>().Get();
            return resultado.Models;
        }

        public async Task<List<Vaga>> ListarAbertas()
        {
            var resultado = await _supabase
                .From<Vaga>()
                .Where(v => v.Status == "aberta")
                .Get();
            return resultado.Models;
        }

        public async Task<Vaga?> BuscarPorId(int id)
        {
            var resultado = await _supabase.From<Vaga>().Where(v => v.Id == id).Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new ServerErrorException("Falha ao buscar vaga");

            return resultado.Model;
        }

        public async Task Criar(Vaga novaVaga)
        {
            await _supabase.From<Vaga>().Insert(novaVaga);
        }

        public async Task Atualizar(Vaga vagaAtualizada)
        {
            await _supabase.From<Vaga>().Update(vagaAtualizada);
        }

        public async Task Deletar(Vaga vaga)
        {
            await _supabase.From<Vaga>().Delete(vaga);
        }
    }
}