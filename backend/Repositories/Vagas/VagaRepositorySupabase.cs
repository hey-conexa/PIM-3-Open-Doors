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

        public async Task<List<Vaga>> ListarTodas()
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

        public async Task<Vaga> Criar(Vaga novaVaga)
        {
            var response = await _supabase.From<Vaga>().Insert(novaVaga);
            // Supondo que o Insert retorna o(s) modelo(s) inserido(s)
            if (response.Models == null || response.Models.Count == 0)
                throw new ServerErrorException("Falha ao criar vaga");

            return response.Models.First();
        }

        public async Task<Vaga> Atualizar(Vaga vagaAtualizada)
        {
            var response = await _supabase.From<Vaga>().Update(vagaAtualizada);
            if (response.Models == null || response.Models.Count == 0)
                throw new ServerErrorException("Falha ao atualizar vaga");

            return response.Models.First();
        }

        public async Task Deletar(int id)
        {
            var resultado = await _supabase.From<Vaga>().Where(v => v.Id == id).Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new ServerErrorException("Vaga não encontrada para deletar");

            var vaga = resultado.Models.First();
            await _supabase.From<Vaga>().Delete(vaga);
        }

        public async Task<List<Vaga>> ListarPorEmpresa(Guid empresaId)
        {
            var resultado = await _supabase
                .From<Vaga>()
                .Where(v => v.EmpresaId == empresaId)
                .Get();
            return resultado.Models;
        }

        public async Task<Vaga> AtualizarStatus(int vagaId, string novoStatus)
        {
            var resultado = await _supabase.From<Vaga>().Where(v => v.Id == vagaId).Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new ServerErrorException("Vaga não encontrada para atualizar status");

            var vaga = resultado.Models.First();
            vaga.Status = novoStatus;
            await _supabase.From<Vaga>().Update(vaga);

            return vaga;
        }
    }
}