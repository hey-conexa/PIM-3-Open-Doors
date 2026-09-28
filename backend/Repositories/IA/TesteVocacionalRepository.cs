using OpenDoors.Api.Interfaces.IA;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Repositories.IA
{
    /// <summary>
    /// Repositório para persistência de testes vocacionais no Supabase.
    /// Gerencia operações CRUD e filtros específicos para TesteVocacional.
    /// </summary>
    public class TesteVocacionalRepository : ITesteVocacionalRepository
    {
        private readonly Supabase.Client _supabase;

        public TesteVocacionalRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        /// <summary>
        /// Busca um teste vocacional específico de um estudante.
        /// </summary>
        public async Task<TesteVocacional?> BuscarPorEstudanteId(Guid estudanteId)
        {
            try
            {
                var resultado = await _supabase
                    .From<TesteVocacional>()
                    .Where(t => t.EstudanteId == estudanteId)
                    .Get();

                return resultado.Models.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Cria um novo teste vocacional no banco.
        /// </summary>
        public async Task<TesteVocacional> Criar(TesteVocacional teste)
        {
            if (teste == null)
                throw new ArgumentNullException(nameof(teste), "Teste vocacional não pode ser nulo");

            var resultado = await _supabase
                .From<TesteVocacional>()
                .Insert(teste);

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new InvalidOperationException("Falha ao inserir teste vocacional no banco");

            return resultado.Models[0];
        }

        /// <summary>
        /// Atualiza um teste vocacional existente.
        /// </summary>
        public async Task<TesteVocacional> Atualizar(TesteVocacional teste)
        {
            if (teste == null)
                throw new ArgumentNullException(nameof(teste), "Teste vocacional não pode ser nulo");

            var resultado = await teste.Update<TesteVocacional>();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new InvalidOperationException("Falha ao atualizar teste vocacional no banco");

            return resultado.Models[0];
        }

        /// <summary>
        /// Lista todos os testes vocacionais.
        /// </summary>
        public async Task<List<TesteVocacional>> ListarTodos()
        {
            try
            {
                var resultado = await _supabase
                    .From<TesteVocacional>()
                    .Get();

                return resultado.Models ?? new List<TesteVocacional>();
            }
            catch
            {
                return new List<TesteVocacional>();
            }
        }

        /// <summary>
        /// Lista testes vocacionais que já foram analisados pela IA.
        /// </summary>
        public async Task<List<TesteVocacional>> ListarAnalisados()
        {
            try
            {
                var resultado = await _supabase
                    .From<TesteVocacional>()
                    .Where(t => t.AnalisadoIa == true)
                    .Get();

                return resultado.Models ?? new List<TesteVocacional>();
            }
            catch
            {
                return new List<TesteVocacional>();
            }
        }
    }
}