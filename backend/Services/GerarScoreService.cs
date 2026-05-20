using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Services
{
    /// <summary>
    /// Calcula score de compatibilidade Estudante x Vaga via IA,
    /// salva o match e atualiza ranking automaticamente.
    /// </summary>
    public class GerarScoreService
    {
        private readonly GroqService _groq;
        private readonly Supabase.Client _supabase;

        public GerarScoreService(GroqService groq, Supabase.Client supabase)
        {
            _groq = groq;
            _supabase = supabase;
        }

        public async Task<ScoreCompatibilidadeDto> GerarAsync(Guid estudanteId, int vagaId)
        {
            // Busca dados no Supabase
            var estudante = await _supabase
                .From<Estudante>()
                .Where(e => e.Id == estudanteId)
                .Single();

            if (estudante == null)
                throw new KeyNotFoundException($"Estudante não encontrado: {estudanteId}");

            var vaga = await _supabase
                .From<Vaga>()
                .Where(v => v.Id == vagaId)
                .Single();

            if (vaga == null)
                throw new KeyNotFoundException($"Vaga não encontrada: {vagaId}");

            var testesResultado = await _supabase
                .From<TesteVocacional>()
                .Where(t => t.EstudanteId == estudanteId)
                .Get();
            var teste = testesResultado.Models.FirstOrDefault();

            // Monta contextos
            var habilidades = estudante.HabilidadesExtraidas ?? new List<string>();
            var contextoEstudante = $"""
                Estudante:
                - Curso: {estudante.Curso}
                - Semestre: {estudante.Semestre}
                - Cidade: {estudante.Cidade} / {estudante.Estado}
                - Habilidades: {string.Join(", ", habilidades)}
                """;

            var contextoTeste = "";
            if (teste != null)
            {
                var areas = string.Join(", ", teste.AreasSugeridas ?? new());
                var pontos = string.Join(", ", teste.PontosFortes ?? new());
                contextoTeste = $"""
                    Perfil vocacional:
                    - Perfil dominante: {teste.PerfilDominante}
                    - Áreas sugeridas: {areas}
                    - Pontos fortes: {pontos}
                    """;
            }

            var cursosAceitos = string.Join(", ", vaga.CursosAceitos ?? new());
            var habilidadesReq = string.Join(", ", vaga.HabilidadesRequeridas ?? new());
            var habilidadesDif = string.Join(", ", vaga.HabilidadesDiferenciais ?? new());
            var contextoVaga = $"""
                Vaga:
                - Título: {vaga.Titulo}
                - Área: {vaga.Area}
                - Cursos aceitos: {cursosAceitos}
                - Semestre mínimo: {vaga.SemestreMinimo}
                - Habilidades requeridas: {habilidadesReq}
                - Habilidades diferenciais: {habilidadesDif}
                - Modalidade: {vaga.Modalidade}
                - Cidade: {vaga.Cidade} / {vaga.Estado}
                """;

            // Chama a IA
            const string system =
                "Você é um especialista em recrutamento e seleção. " +
                "Analise a compatibilidade entre o estudante e a vaga. " +
                "Responda APENAS com JSON válido, sem texto adicional, sem markdown, sem ```json.";

            var user = $$"""
                Analise a compatibilidade e retorne um JSON com:
                {
                  "scoreTotal": 0-100,
                  "scoreCurriculo": 0-100,
                  "scoreVocacional": 0-100,
                  "scoreHabilidades": 0-100,
                  "pontosFortes": ["ponto1", "ponto2"],
                  "pontosFracos": ["ponto1", "ponto2"],
                  "justificativa": "explicação resumida do score em 2-3 frases"
                }

                {{contextoEstudante}}
                {{contextoTeste}}
                {{contextoVaga}}
                """;

            var dados = await _groq.ChatJsonAsync<ScoreCompatibilidadeDto>(system, user);

            // Salva ou atualiza o match (usando SUAS Models)
            var matchesExistentes = await _supabase
                .From<Match>()
                .Where(m => m.EstudanteId == estudanteId && m.VagaId == vagaId)
                .Get();

            var matchExistente = matchesExistentes.Models.FirstOrDefault();

            if (matchExistente != null)
            {
                matchExistente.ScoreTotal = dados.ScoreTotal;
                matchExistente.ScoreCurriculo = dados.ScoreCurriculo;
                matchExistente.ScoreVocacional = dados.ScoreVocacional;
                matchExistente.ScoreHabilidades = dados.ScoreHabilidades;
                matchExistente.PontosFortes = dados.PontosFortes;
                matchExistente.PontosFracos = dados.PontosFracos;
                matchExistente.Justificativa = dados.Justificativa;
                await matchExistente.Update<Match>();
            }
            else
            {
                await _supabase.From<Match>().Insert(new Match
                {
                    EstudanteId = estudanteId,
                    VagaId = vagaId,
                    EmpresaId = vaga.EmpresaId,
                    ScoreTotal = dados.ScoreTotal,
                    ScoreCurriculo = dados.ScoreCurriculo,
                    ScoreVocacional = dados.ScoreVocacional,
                    ScoreHabilidades = dados.ScoreHabilidades,
                    PontosFortes = dados.PontosFortes,
                    PontosFracos = dados.PontosFracos,
                    Justificativa = dados.Justificativa,
                });
            }

            await AtualizarRankingAsync(vagaId);
            return dados;
        }

        private async Task AtualizarRankingAsync(int vagaId)
        {
            var matches = await _supabase
                .From<Match>()
                .Where(m => m.VagaId == vagaId)
                .Order(m => m.ScoreTotal, Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();

            int posicao = 1;
            foreach (var match in matches.Models)
            {
                var candidaturas = await _supabase
                    .From<Candidatura>()
                    .Where(c => c.EstudanteId == match.EstudanteId && c.VagaId == vagaId)
                    .Get();

                foreach (var candidatura in candidaturas.Models)
                {
                    candidatura.PosicaoRanking = posicao;
                    await candidatura.Update<Candidatura>();
                }
                posicao++;
            }
        }
    }
}