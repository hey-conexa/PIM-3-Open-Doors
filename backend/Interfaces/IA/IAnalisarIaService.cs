using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Interfaces.IA
{
    /// <summary>
    /// Interface para coordenar análises de IA.
    /// Orquestra a análise de currículo, teste vocacional e geração de score.
    /// </summary>
    public interface IAnalisarIaService
    {
        /// <summary>
        /// Analisa um currículo em PDF e extrai habilidades do estudante.
        /// Persiste as habilidades encontradas.
        /// </summary>
        /// <param name="estudanteId">ID do estudante</param>
        /// <param name="stream">Stream do arquivo PDF</param>
        /// <param name="nomeArquivo">Nome do arquivo PDF</param>
        /// <returns>DTO com habilidades extraídas</returns>
        Task<dynamic> AnalisarCurriculoAsync(Guid estudanteId, Stream stream, string nomeArquivo);

        /// <summary>
        /// Analisa respostas do teste vocacional e gera perfil do estudante.
        /// Persiste o perfil vocacional gerado no banco.
        /// </summary>
        /// <param name="estudanteId">ID do estudante</param>
        /// <param name="respostas">Lista de respostas do teste</param>
        /// <returns>DTO com perfil vocacional gerado</returns>
        Task<PerfilVocacionalDto> AnalisarTesteVocacionalAsync(Guid estudanteId, List<RespostaVocacionalDto> respostas);

        /// <summary>
        /// Gera score de compatibilidade entre estudante e vaga.
        /// Realiza análise baseada em perfil, habilidades e requisitos.
        /// </summary>
        /// <param name="estudanteId">ID do estudante</param>
        /// <param name="vagaId">ID da vaga</param>
        /// <returns>Score de compatibilidade</returns>
        Task<dynamic> GerarScoreAsync(Guid estudanteId, int vagaId);
    }
}