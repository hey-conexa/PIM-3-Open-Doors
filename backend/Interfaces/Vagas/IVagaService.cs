using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Vagas
{
    /// <summary>
    /// Interface para operações de negócio com vagas.
    /// Encapsula validações e lógica de negócio.
    /// </summary>
    public interface IVagaService
    {
        /// <summary>
        /// Lista todas as vagas.
        /// </summary>
        Task<List<VagaDto>> ListarTodas();

        /// <summary>
        /// Lista apenas as vagas abertas.
        /// </summary>
        Task<List<VagaDto>> ListarAbertas();

        /// <summary>
        /// Lista vagas de uma empresa específica.
        /// </summary>
        Task<List<VagaDto>> ListarPorEmpresa(Guid empresaId);

        /// <summary>
        /// Busca uma vaga específica por ID.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        Task<VagaDto> BuscarPorId(int id);

        /// <summary>
        /// Cria uma nova vaga.
        /// Valida dados e lança BadRequestException se inválidos.
        /// </summary>
        Task<VagaDto> Criar(CreateVagaDto dto);

        /// <summary>
        /// Atualiza uma vaga existente.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        Task<VagaDto> Atualizar(int id, CreateVagaDto dto);

        /// <summary>
        /// Atualiza apenas o status de uma vaga.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        Task<VagaDto> AtualizarStatus(int id, string novoStatus);

        /// <summary>
        /// Deleta uma vaga.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        Task Deletar(int id);
    }
}