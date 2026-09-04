using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Candidaturas;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Repositories
{
    public class CandidaturaRepositorySupabase : ICandidaturaRepository
    {
        private readonly Supabase.Client _supabase;

        public CandidaturaRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Candidatura>> ListarTodos()
        {
            var resultado = await _supabase.From<Candidatura>().Get();
            return resultado.Models;
        }

        public async Task<Candidatura?> BuscarPorId(int id)
        {
            var resultado = await _supabase.From<Candidatura>().Where(c => c.Id == id).Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new ServerErrorException("Falha ao buscar candidatura");

            return resultado.Model;
        }

        public async Task<List<Candidatura>> ListarPorEstudante(Guid estudanteId)
        {
            var resultado = await _supabase
                .From<Candidatura>()
                .Where(c => c.EstudanteId == estudanteId)
                .Get();
            return resultado.Models;
        }

        public async Task<List<Candidatura>> ListarPorVaga(int vagaId)
        {
            var resultado = await _supabase
                .From<Candidatura>()
                .Where(c => c.VagaId == vagaId)
                .Get();
            return resultado.Models;
        }

        public async Task<List<Candidatura>> ListarPorEmpresa(Guid empresaId)
        {
            var resultado = await _supabase
                .From<Candidatura>()
                .Where(c => c.EmpresaId == empresaId)
                .Get();
            return resultado.Models;
        }

        public async Task Criar(Candidatura novaCandidatura)
        {
            await _supabase.From<Candidatura>().Insert(novaCandidatura);
        }

        public async Task AtualizarStatus(Candidatura candidaturaAtualizada)
        {
            await _supabase.From<Candidatura>().Update(candidaturaAtualizada);
        }

        public async Task Deletar(Candidatura candidatura)
        {
            await _supabase.From<Candidatura>().Delete(candidatura);
        }
    }
}