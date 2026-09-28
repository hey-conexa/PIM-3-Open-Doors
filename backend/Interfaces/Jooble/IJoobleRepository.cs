using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.Jooble
{
    /// <summary>
    /// Interface para persistência de vagas importadas da Jooble.
    /// Encapsula operações de banco de dados para vagas externas.
    /// </summary>
    public interface IJoobleRepository
    {
        /// <summary>
        /// Verifica se uma vaga já existe no banco pelo título e cidade.
        /// </summary>
        Task<bool> VagaExiste(string titulo, string cidade);

        /// <summary>
        /// Importa uma vaga no banco.
        /// </summary>
        Task<Vaga> ImportarVaga(Vaga vaga);

        /// <summary>
        /// Importa múltiplas vagas de forma mais eficiente.
        /// </summary>
        Task<int> ImportarMultiplasVagas(List<Vaga> vagas);
    }
}