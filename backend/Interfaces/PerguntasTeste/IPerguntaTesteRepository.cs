using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.PerguntasTeste
{
    /// <summary>
    /// Interface para persistência de perguntas de teste vocacional.
    /// Encapsula operações de banco de dados para PerguntaTeste.
    /// </summary>
    public interface IPerguntaTesteRepository
    {
        /// <summary>
        /// Lista todas as perguntas ativas.
        /// Pode filtrar por tipo (fixa/mensal) e mês de referência.
        /// </summary>
        Task<List<PerguntaTeste>> ListarAtivas(string? tipo = null, string? mes = null);

        /// <summary>
        /// Lista todas as perguntas (ativas e inativas).
        /// </summary>
        Task<List<PerguntaTeste>> ListarTodas();

        /// <summary>
        /// Busca uma pergunta específica por ID.
        /// </summary>
        Task<PerguntaTeste?> BuscarPorId(int id);

        /// <summary>
        /// Cria uma nova pergunta de teste.
        /// </summary>
        Task<PerguntaTeste> Criar(PerguntaTeste pergunta);

        /// <summary>
        /// Atualiza uma pergunta existente.
        /// </summary>
        Task<PerguntaTeste> Atualizar(PerguntaTeste pergunta);

        /// <summary>
        /// Desativa uma pergunta (soft delete).
        /// </summary>
        Task<PerguntaTeste> Desativar(int id);
    }
}