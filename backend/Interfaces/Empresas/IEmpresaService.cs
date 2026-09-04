using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Empresas
{
    public interface IEmpresaService
    {
        public Task<IReadOnlyList<EmpresaDto>> ListarTodos();
        public Task<IReadOnlyList<EmpresaDto>> ListarAtivos();
        public Task<EmpresaDto> BuscarPorId(Guid id);
        public Task<EmpresaDto> Criar(CreateEmpresaDto novaEmpresa);
        public Task<EmpresaDto> Atualizar(CreateEmpresaDto empresaAtualizada, Guid id);
        public Task<EmpresaDto> Deletar(Guid id);
    }
}