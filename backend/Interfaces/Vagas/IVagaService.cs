using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Vagas
{
    public interface IVagaService
    {
        public Task<IReadOnlyList<VagaDto>> ListarTodos();
        public Task<IReadOnlyList<VagaDto>> ListarAbertas();
        public Task<VagaDto> BuscarPorId(int id);
        public Task<VagaDto> Criar(CreateVagaDto novaVaga);
        public Task<VagaDto> Atualizar(CreateVagaDto vagaAtualizada, int id);
        public Task<VagaDto> Deletar(int id);
    }
}