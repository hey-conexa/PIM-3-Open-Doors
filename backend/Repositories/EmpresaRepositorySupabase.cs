using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Empresas;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Repositories
{
    public class EmpresaRepositorySupabase : IEmpresaRepository
    {
        private readonly Supabase.Client _supabase;

        public EmpresaRepositorySupabase(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Empresa>> ListarTodos()
        {
            var resultado = await _supabase.From<Empresa>().Get();
            return resultado.Models;
        }

        public async Task<List<Empresa>> ListarAtivos()
        {
            var resultado = await _supabase.From<Empresa>().Where(e => e.Status == "ativa").Get();
            return resultado.Models;
        }

        public async Task<Empresa?> BuscarPorId(Guid id)
        {
            var resultado = await _supabase.From<Empresa>().Where(e => e.Id.Equals(id)).Get();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new ServerErrorException("Falha ao buscar empresa");

            return resultado.Model;
        }

        public async Task Criar(Empresa novaEmpresa)
        {
            await _supabase.From<Empresa>().Insert(novaEmpresa);
        }

        public async Task Atualizar(Empresa empresaAtualizada)
        {
            await _supabase.From<Empresa>().Update(empresaAtualizada);
        }

        public async Task Deletar(Empresa empresa)
        {
            await _supabase.From<Empresa>().Delete(empresa);
        }
    }
}