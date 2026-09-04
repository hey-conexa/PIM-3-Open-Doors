using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Empresas
{
    public interface IEmpresaRepository
    {
        public Task<List<Empresa>> ListarTodos();
        public Task<List<Empresa>> ListarAtivos();
        public Task<Empresa?> BuscarPorId(Guid id);
        public Task Criar(Empresa novaEmpresa);
        public Task Atualizar(Empresa empresaAtualizada);
        public Task Deletar(Empresa empresa);
    }
}
