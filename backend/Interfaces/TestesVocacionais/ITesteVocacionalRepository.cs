using OpenDoors.Api.Models;

namespace OpenDoors.Api.Interfaces.TestesVocacionais
{
    /// <summary>
    /// Interface para persistência de testes vocacionais.
    /// Encapsula operações de banco de dados para TesteVocacional.
    /// </summary>
    public interface ITesteVocacionalRepository
    {
        /// <summary>
        /// Busca um teste vocacional por ID do estudante.
        /// Cada estudante tem no máximo 1 teste.
        /// </summary>
        Task<TesteVocacional?> BuscarPorEstudanteId(Guid estudanteId);

        /// <summary>
        /// Cria um novo teste vocacional.
        /// </summary>
        Task<TesteVocacional> Criar(TesteVocacional teste);

        /// <summary>
        /// Atualiza um teste vocacional existente.
        /// </summary>
        Task<TesteVocacional> Atualizar(TesteVocacional teste);

        /// <summary>
        /// Busca todos os testes vocacionais.
        /// </summary>
        Task<List<TesteVocacional>> ListarTodos();

        /// <summary>
        /// Busca testes que já foram analisados pela IA.
        /// </summary>
        Task<List<TesteVocacional>> ListarAnalisados();
    }
}