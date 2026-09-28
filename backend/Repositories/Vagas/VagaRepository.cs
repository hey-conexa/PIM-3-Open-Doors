using OpenDoors.Api.Interfaces.Vagas;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Repositories.Vagas
{
    /// <summary>
    /// Repositório para persistência de vagas no Supabase.
    /// Gerencia operações CRUD e filtros específicos para Vaga.
    /// </summary>
    public class VagaRepository : IVagaRepository
    {
        private readonly Supabase.Client _supabase;

        public VagaRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        /// <summary>
        /// Lista todas as vagas.
        /// </summary>
        public async Task<List<Vaga>> ListarTodas()
        {
            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Get();

                return resultado.Models ?? new List<Vaga>();
            }
            catch
            {
                return new List<Vaga>();
            }
        }

        /// <summary>
        /// Lista apenas as vagas com status "aberta".
        /// </summary>
        public async Task<List<Vaga>> ListarAbertas()
        {
            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Where(v => v.Status == "aberta")
                    .Get();

                return resultado.Models ?? new List<Vaga>();
            }
            catch
            {
                return new List<Vaga>();
            }
        }

        /// <summary>
        /// Lista vagas de uma empresa específica, ordenadas por data de criação decrescente.
        /// </summary>
        public async Task<List<Vaga>> ListarPorEmpresa(Guid empresaId)
        {
            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Where(v => v.EmpresaId == empresaId)
                    .Order("criado_em", Supabase.Postgrest.Constants.Ordering.Descending)
                    .Get();

                return resultado.Models ?? new List<Vaga>();
            }
            catch
            {
                return new List<Vaga>();
            }
        }

        /// <summary>
        /// Busca uma vaga específica por ID.
        /// </summary>
        public async Task<Vaga?> BuscarPorId(int id)
        {
            try
            {
                var resultado = await _supabase
                    .From<Vaga>()
                    .Where(v => v.Id == id)
                    .Single();

                return resultado;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Cria uma nova vaga.
        /// </summary>
        public async Task<Vaga> Criar(Vaga vaga)
        {
            if (vaga == null)
                throw new ArgumentNullException(nameof(vaga), "Vaga não pode ser nula");

            var resultado = await _supabase
                .From<Vaga>()
                .Insert(vaga);

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new InvalidOperationException("Falha ao inserir vaga no banco");

            return resultado.Models[0];
        }

        /// <summary>
        /// Atualiza uma vaga existente.
        /// </summary>
        public async Task<Vaga> Atualizar(Vaga vaga)
        {
            if (vaga == null)
                throw new ArgumentNullException(nameof(vaga), "Vaga não pode ser nula");

            var resultado = await vaga.Update<Vaga>();

            if (resultado.Models == null || resultado.Models.Count == 0)
                throw new InvalidOperationException("Falha ao atualizar vaga no banco");

            return resultado.Models[0];
        }

        /// <summary>
        /// Atualiza apenas o status de uma vaga.
        /// </summary>
        public async Task<Vaga> AtualizarStatus(int vagaId, string novoStatus)
        {
            if (string.IsNullOrWhiteSpace(novoStatus))
                throw new ArgumentException("Status não pode estar vazio", nameof(novoStatus));

            var vaga = await BuscarPorId(vagaId);
            if (vaga == null)
                throw new KeyNotFoundException($"Vaga com ID {vagaId} não encontrada");

            vaga.Status = novoStatus;
            return await Atualizar(vaga);
        }

        /// <summary>
        /// Deleta uma vaga por ID.
        /// </summary>
        public async Task Deletar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID da vaga deve ser maior que 0", nameof(id));

            await _supabase
                .From<Vaga>()
                .Where(v => v.Id == id)
                .Delete();
        }
    }
}