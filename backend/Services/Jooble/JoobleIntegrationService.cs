using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Jooble;
using OpenDoors.Api.Models;
using OpenDoors.Api.Services;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services.Jooble
{
    /// <summary>
    /// Serviço de integração com a API Jooble.
    /// Orquestra busca de vagas externas e sincronização com banco.
    /// </summary>
    public class JoobleIntegrationService : IJoobleService
    {
        private readonly JoobleService _joobleService;
        private readonly IJoobleRepository _repository;

        public JoobleIntegrationService(JoobleService joobleService, IJoobleRepository repository)
        {
            _joobleService = joobleService;
            _repository = repository;
        }

        /// <summary>
        /// Busca vagas na API Jooble com os filtros fornecidos.
        /// Retorna as vagas sem salvar no banco.
        /// Útil para exibir vagas ao vivo no frontend.
        /// </summary>
        public async Task<dynamic> BuscarVagasAsync(JoobleBuscaDto filtros)
        {
            ValidarJoobleBuscaDto(filtros);

            var resultado = await _joobleService.BuscarVagasAsync(filtros);

            return new
            {
                total = resultado.TotalCount,
                vagas = resultado.Jobs
            };
        }

        /// <summary>
        /// Busca vagas na API Jooble e sincroniza com o banco.
        /// Evita duplicatas usando título + cidade como chave.
        /// Retorna estatísticas de importação (salvas, ignoradas, total).
        /// </summary>
        public async Task<dynamic> SincronizarVagasAsync(JoobleBuscaDto filtros)
        {
            ValidarJoobleBuscaDto(filtros);

            var resultado = await _joobleService.BuscarVagasAsync(filtros);

            if (resultado.Jobs == null || resultado.Jobs.Count == 0)
            {
                return new
                {
                    mensagem = "Nenhuma vaga retornada pela Jooble.",
                    salvas = 0,
                    ignoradas = 0,
                    total = 0
                };
            }

            int salvas = 0;
            int ignoradas = 0;

            foreach (var job in resultado.Jobs)
            {
                // Converte para modelo Vaga
                var vaga = JoobleService.ConverterParaVaga(job);

                // Verifica se já existe
                var existe = await _repository.VagaExiste(vaga.Titulo, vaga.Cidade);

                if (existe)
                {
                    ignoradas++;
                    continue;
                }

                try
                {
                    await _repository.ImportarVaga(vaga);
                    salvas++;
                }
                catch
                {
                    // Log de erro mas continua com a próxima
                    ignoradas++;
                }
            }

            return new
            {
                mensagem = "Sincronização concluída.",
                salvas,
                ignoradas,
                total = resultado.TotalCount
            };
        }

        /// <summary>
        /// Valida os dados de busca da Jooble.
        /// </summary>
        private static void ValidarJoobleBuscaDto(JoobleBuscaDto filtros)
        {
            if (filtros == null)
                throw new BadRequestException("Filtros de busca são obrigatórios");

            if (string.IsNullOrWhiteSpace(filtros.Keywords))
                throw new BadRequestException("Palavras-chave de busca são obrigatórias");
        }
    }
}