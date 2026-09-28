using OpenDoors.Api.Interfaces.PerguntasTeste;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Repositories.PerguntasTeste
{
    /// <summary>
    /// Repositório para persistência de perguntas de teste no Supabase.
    /// Gerencia operações CRUD e filtros específicos para PerguntaTeste.
    /// </summary>
    public class PerguntaTesteRepository : IPerguntaTesteRepository
    {
        private readonly Supabase.Client _supabase;

        public PerguntaTesteRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        /// <summary>
        /// Lista todas as perguntas ativas.
        /// Pode filtrar por tipo (fixa/mensal) e mês de referência.
        /// </summary>
        public async Task<List<PerguntaTeste>> ListarAtivas(string? tipo = null, string? mes = null)
        {
            try
            {
                var resultado = await _supabase
                    .From<PerguntaTeste>()
                    .Get();

                var perguntas = resultado.Models?
                    .Where(p => p.Ativa)
                    .AsEnumerable() ?? new List<PerguntaTeste>();

                if (!string.IsNullOrEmpty(tipo))
                    perguntas = perguntas.Where(p => p.Tipo == tipo);

                if (!string.IsNullOrEmpty(mes))
                    perguntas = perguntas.Where(p => p.MesReferencia == mes);
                else if (string.IsNullOrEmpty(tipo))
                    // Sem filtros: retorna fixas + perguntas do mês atual
                    perguntas = perguntas.Where(p =>
                        p.Tipo == "fixa" ||
                        p.MesReferencia == DateTime.UtcNow.ToString("yyyy-MM"));

                return perguntas.OrderBy(p => p.Ordem).ToList();
            }
            catch
            {
                return new List<PerguntaTeste>();
            }
        }

        /// <summary>
        /// Lista todas as perguntas (ativas e inativas).
        /// </summary>
        public async Task<List<PerguntaTeste>> ListarTodas()
        {
            try
            {
                var resultado = await _supabase
                    .From<PerguntaTeste>()
                    .Get();

                return resultado.Models?
                    .OrderBy(p => p.Ordem)
                    .ToList() ?? new List<PerguntaTeste>();
            }
            catch
            {
                return new List<PerguntaTeste>();
            }
        }

        /// <summary>
        /// Busca uma pergunta específica por ID.
        /// </summary>
        public async Task<PerguntaTeste?> BuscarPorId(int id)
        {
            try
            {
                var resultado = await _supabase
                    .From<PerguntaTeste>()
                    .Where(p => p.Id == id)
                    .Single();

                return resultado;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Cria uma nova pergunta de teste.
        /// </summary>
        public async Task<PerguntaTeste> Criar(PerguntaTeste pergunta)
        {
            if (pergunta == null)
                throw new ArgumentNullException(nameof(pergunta), "Pergunta não pode ser nula");

            var resultado = await _supabase
                .From<PerguntaTeste>()
                .Insert(pergunta);

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new InvalidOperationException("Falha ao inserir pergunta no banco");

            return resultado.Models[0];
        }

        /// <summary>
        /// Atualiza uma pergunta existente.
        /// </summary>
        public async Task<PerguntaTeste> Atualizar(PerguntaTeste pergunta)
        {
            if (pergunta == null)
                throw new ArgumentNullException(nameof(pergunta), "Pergunta não pode ser nula");

            var resultado = await pergunta.Update<PerguntaTeste>();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new InvalidOperationException("Falha ao atualizar pergunta no banco");

            return resultado.Models[0];
        }

        /// <summary>
        /// Desativa uma pergunta (soft delete).
        /// </summary>
        public async Task<PerguntaTeste> Desativar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID da pergunta deve ser maior que 0", nameof(id));

            var pergunta = await BuscarPorId(id);
            if (pergunta == null)
                throw new KeyNotFoundException($"Pergunta com ID {id} não encontrada");

            pergunta.Ativa = false;
            return await Atualizar(pergunta);
        }
    }
}