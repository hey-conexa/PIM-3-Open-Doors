using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Vagas
{
    /// <summary>
    /// Interface para persistência de vagas.
    /// Encapsula operações de banco de dados para Vaga.
    /// </summary>
    public interface IVagaRepository
    {
        /// <summary>
        /// Lista todas as vagas.
        /// </summary>
        Task<List<Vaga>> ListarTodas();

        /// <summary>
        /// Lista apenas as vagas com status "aberta".
        /// </summary>
        Task<List<Vaga>> ListarAbertas();

        /// <summary>
        /// Lista vagas de uma empresa específica, ordenadas por data de criação decrescente.
        /// </summary>
        Task<List<Vaga>> ListarPorEmpresa(Guid empresaId);

        /// <summary>
        /// Busca uma vaga específica por ID.
        /// </summary>
        Task<Vaga?> BuscarPorId(int id);

        /// <summary>
        /// Cria uma nova vaga.
        /// </summary>
        Task<Vaga> Criar(Vaga vaga);

        /// <summary>
        /// Atualiza uma vaga existente.
        /// </summary>
        Task<Vaga> Atualizar(Vaga vaga);

        /// <summary>
        /// Atualiza apenas o status de uma vaga.
        /// </summary>
        Task<Vaga> AtualizarStatus(int vagaId, string novoStatus);

        /// <summary>
        /// Deleta uma vaga por ID.
        /// </summary>
        Task Deletar(int id);
    }
}