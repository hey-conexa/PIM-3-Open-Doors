using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.IA;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Models;
using OpenDoors.Api.Services;
using OpenDoors.Api.Exceptions;
using OpenDoors.Api.Interfaces.TestesVocacionais;

namespace OpenDoors.Api.Services.IA
{
    /// <summary>
    /// Serviço que coordena análises de IA.
    /// Encapsula lógica de negócio para análise de currículo, teste vocacional e geração de score.
    /// Utiliza services especializados e repositórios para persistência.
    /// </summary>
    public class AnalisarIaService : IAnalisarIaService
    {
        private readonly AnalisarCurriculoService _curriculoService;
        private readonly AnalisarTesteService _testeService;
        private readonly GerarScoreService _scoreService;
        private readonly ITesteVocacionalRepository _testeRepository;
        private readonly IEstudanteRepository _estudanteRepository;

        public AnalisarIaService(
            AnalisarCurriculoService curriculoService,
            AnalisarTesteService testeService,
            GerarScoreService scoreService,
            ITesteVocacionalRepository testeRepository,
            IEstudanteRepository estudanteRepository)
        {
            _curriculoService = curriculoService;
            _testeService = testeService;
            _scoreService = scoreService;
            _testeRepository = testeRepository;
            _estudanteRepository = estudanteRepository;
        }

        /// <summary>
        /// Analisa um currículo em PDF e extrai habilidades.
        /// Delega análise técnica para AnalisarCurriculoService.
        /// </summary>
        public async Task<dynamic> AnalisarCurriculoAsync(Guid estudanteId, Stream stream, string nomeArquivo)
        {
            if (estudanteId == Guid.Empty)
                throw new BadRequestException("ID do estudante é obrigatório");

            if (stream == null || stream.Length == 0)
                throw new BadRequestException("Stream do currículo é obrigatório e não pode estar vazio");

            // Delega análise ao serviço especializado
            // AnalisarCurriculoService lançará KeyNotFoundException ou InvalidOperationException conforme necessário
            var resultado = await _curriculoService.AnalisarAsync(estudanteId, stream, nomeArquivo);

            return resultado;
        }

        /// <summary>
        /// Analisa respostas do teste vocacional, gera perfil e persiste no banco.
        /// Orquestra: análise + salvamento + atualização de estudante.
        /// </summary>
        public async Task<PerfilVocacionalDto> AnalisarTesteVocacionalAsync(Guid estudanteId, List<RespostaVocacionalDto> respostas)
        {
            if (estudanteId == Guid.Empty)
                throw new BadRequestException("ID do estudante é obrigatório");

            if (respostas == null || respostas.Count == 0)
                throw new BadRequestException("Respostas do teste são obrigatórias");

            // Gera perfil vocacional (análise)
            PerfilVocacionalDto resultado;
            try
            {
                resultado = await _testeService.AnalisarAsync(respostas);
            }
            catch (Exception)
            {
                // Fallback: gera perfil genérico se análise falhar
                resultado = GerarPerfilFallback(respostas);
            }

            // Busca ou cria teste vocacional no banco
            var testeExistente = await _testeRepository.BuscarPorEstudanteId(estudanteId);

            if (testeExistente != null)
            {
                // Atualiza teste existente
                testeExistente.PerfilDominante = resultado.PerfilDominante;
                testeExistente.AreasSugeridas = resultado.AreasSugeridas;
                testeExistente.PontosFortes = resultado.PontosFortes;
                testeExistente.DescricaoPerfil = resultado.DescricaoPerfil;
                testeExistente.AnalisadoIa = true;
                testeExistente.ConcluidoEm = DateTime.UtcNow;

                await _testeRepository.Atualizar(testeExistente);
            }
            else
            {
                // Cria novo teste vocacional
                var novoTeste = new TesteVocacional
                {
                    EstudanteId = estudanteId,
                    PerfilDominante = resultado.PerfilDominante,
                    AreasSugeridas = resultado.AreasSugeridas,
                    PontosFortes = resultado.PontosFortes,
                    DescricaoPerfil = resultado.DescricaoPerfil,
                    AnalisadoIa = true,
                    ConcluidoEm = DateTime.UtcNow
                };

                await _testeRepository.Criar(novoTeste);
            }

            // Marca o estudante como tendo teste vocacional
            try
            {
                var estudante = await _estudanteRepository.BuscarPorId(estudanteId);
                if (estudante != null)
                {
                    estudante.TemTesteVocacional = true;
                    await _estudanteRepository.Atualizar(estudante);
                }
            }
            catch
            {
                // Log de erro mas não falha o fluxo
            }

            return resultado;
        }

        /// <summary>
        /// Gera score de compatibilidade entre estudante e vaga.
        /// Delega análise técnica para GerarScoreService.
        /// </summary>
        public async Task<dynamic> GerarScoreAsync(Guid estudanteId, int vagaId)
        {
            if (estudanteId == Guid.Empty)
                throw new BadRequestException("ID do estudante é obrigatório");

            if (vagaId <= 0)
                throw new BadRequestException("ID da vaga é obrigatório e deve ser maior que 0");

            // Delega análise ao serviço especializado
            // GerarScoreService lançará KeyNotFoundException conforme necessário
            var resultado = await _scoreService.GerarAsync(estudanteId, vagaId);

            return resultado;
        }

        /// <summary>
        /// Gera um perfil vocacional fallback quando a análise principal falha.
        /// Utiliza lógica simples baseada em pesos para categorias.
        /// </summary>
        private static PerfilVocacionalDto GerarPerfilFallback(List<RespostaVocacionalDto> respostas)
        {
            var pesos = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["Realista"] = 0,
                ["Investigativo"] = 0,
                ["Artistico"] = 0,
                ["Social"] = 0,
                ["Empreendedor"] = 0,
                ["Convencional"] = 0,
            };

            foreach (var r in respostas)
            {
                if (!int.TryParse(r.Resposta, out var valorLikert)) continue;
                var afinidade = Math.Clamp(8 - valorLikert, 1, 7);
                var categoria = (r.Categoria ?? "").ToLowerInvariant();

                if (categoria.Contains("realista")) pesos["Realista"] += afinidade;
                else if (categoria.Contains("investigativo")) pesos["Investigativo"] += afinidade;
                else if (categoria.Contains("artistico") || categoria.Contains("artístico")) pesos["Artistico"] += afinidade;
                else if (categoria.Contains("social")) pesos["Social"] += afinidade;
                else if (categoria.Contains("empreendedor")) pesos["Empreendedor"] += afinidade;
                else if (categoria.Contains("convencional")) pesos["Convencional"] += afinidade;
            }

            var ordenado = pesos.OrderByDescending(p => p.Value).Select(p => p.Key).ToList();
            var topo = ordenado.Take(3).ToList();
            var dominante = topo.FirstOrDefault() ?? "Versatil";

            var mapaAreas = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["Realista"] = ["Logistica", "Operacoes", "Manutencao"],
                ["Investigativo"] = ["Pesquisa", "Analise de Dados", "Qualidade"],
                ["Artistico"] = ["Design", "Comunicacao", "Conteudo"],
                ["Social"] = ["Educacao", "Recursos Humanos", "Atendimento"],
                ["Empreendedor"] = ["Gestao Comercial", "Lideranca", "Negocios"],
                ["Convencional"] = ["Administrativo", "Financeiro", "Processos"],
            };

            var areas = topo
                .SelectMany(t => mapaAreas.TryGetValue(t, out var arr) ? arr : ["Areas diversas"])
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(3)
                .ToList();

            var pontos = new List<string>
            {
                "Capacidade de adaptacao",
                "Potencial para atuar em diferentes contextos",
                "Boa base para explorar trilhas profissionais"
            };

            return new PerfilVocacionalDto
            {
                PerfilDominante = dominante,
                AreasSugeridas = areas,
                PontosFortes = pontos,
                DescricaoPerfil = "Analise gerada em modo de contingencia para manter o teste funcional. O perfil indica interesses predominantes e sugere exploracao de areas compativeis."
            };
        }
    }
}