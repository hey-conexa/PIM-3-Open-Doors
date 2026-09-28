using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.PerguntasTeste
{
    /// <summary>
    /// Interface para operações de negócio com perguntas de teste.
    /// Encapsula validações e lógica de negócio.
    /// </summary>
    public interface IPerguntaTesteService
    {
        /// <summary>
        /// Lista perguntas ativas.
        /// Se sem filtros: retorna fixas + perguntas do mês atual.
        /// Se com tipo: filtra por tipo (fixa/mensal).
        /// Se com mês: filtra apenas por esse mês.
        /// </summary>
        Task<List<PerguntaTesteDto>> ListarAtivas(string? mes = null, string? tipo = null);

        /// <summary>
        /// Lista todas as perguntas (ativas e inativas).
        /// </summary>
        Task<List<PerguntaTesteDto>> ListarTodas();

        /// <summary>
        /// Busca uma pergunta específica por ID.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        Task<PerguntaTesteDto> BuscarPorId(int id);

        /// <summary>
        /// Cria uma nova pergunta de teste.
        /// Valida dados e determina tipo (fixa/mensal) baseado em MesReferencia.
        /// Lança BadRequestException se inválido.
        /// </summary>
        Task<PerguntaTesteDto> Criar(CreatePerguntaTesteDto dto);

        /// <summary>
        /// Atualiza uma pergunta existente.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        Task<PerguntaTesteDto> Atualizar(int id, CreatePerguntaTesteDto dto);

        /// <summary>
        /// Desativa uma pergunta (soft delete).
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        Task<PerguntaTesteDto> Desativar(int id);
    }
}