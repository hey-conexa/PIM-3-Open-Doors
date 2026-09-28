using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.Jooble
{
    /// <summary>
    /// Interface para operações com a API Jooble.
    /// Orquestra busca de vagas externas e sincronização com banco.
    /// </summary>
    public interface IJoobleService
    {
        /// <summary>
        /// Busca vagas na API Jooble com os filtros fornecidos.
        /// Retorna as vagas sem salvar no banco.
        /// Útil para exibir vagas ao vivo no frontend.
        /// </summary>
        Task<dynamic> BuscarVagasAsync(JoobleBuscaDto filtros);

        /// <summary>
        /// Busca vagas na API Jooble e sincroniza com o banco.
        /// Evita duplicatas usando título + cidade como chave.
        /// Retorna estatísticas de importação (salvas, ignoradas, total).
        /// </summary>
        Task<dynamic> SincronizarVagasAsync(JoobleBuscaDto filtros);
    }
}