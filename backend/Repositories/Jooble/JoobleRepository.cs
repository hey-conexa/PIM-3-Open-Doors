using OpenDoors.Api.Interfaces.Jooble;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Repositories.Jooble
{
    /// <summary>
    /// Repositório para persistência de vagas importadas da Jooble no Supabase.
    /// Gerencia operações de importação e verificação de duplicatas.
    /// </summary>
    public class JoobleRepository : IJoobleRepository
    {
        private readonly Supabase.Client _supabase;

        public JoobleRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        /// <summary>
        /// Verifica se uma vaga já existe no banco pelo título e cidade.
        /// </summary>
        public async Task<bool> VagaExiste(string titulo, string cidade)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return false;

            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Where(v => v.Titulo == titulo && v.Cidade == cidade)
                    .Get();

                return resultado.Models?.Any() ?? false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Importa uma vaga no banco.
        /// </summary>
        public async Task<Vaga> ImportarVaga(Vaga vaga)
        {
            if (vaga == null)
                throw new ArgumentNullException(nameof(vaga), "Vaga não pode ser nula");

            var resultado = await _supabase
                .From<Vaga>()
                .Insert(vaga);

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new InvalidOperationException("Falha ao importar vaga no banco");

            return resultado.Models[0];
        }

        /// <summary>
        /// Importa múltiplas vagas de forma mais eficiente.
        /// Retorna a quantidade de vagas importadas com sucesso.
        /// </summary>
        public async Task<int> ImportarMultiplasVagas(List<Vaga> vagas)
        {
            if (vagas == null || vagas.Count == 0)
                return 0;

            int importadas = 0;

            foreach (var vaga in vagas)
            {
                try
                {
                    await ImportarVaga(vaga);
                    importadas++;
                }
                catch
                {
                    // Continua com a próxima vaga se uma falhar
                    continue;
                }
            }

            return importadas;
        }
    }
}