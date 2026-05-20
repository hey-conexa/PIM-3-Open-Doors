using OpenDoors.Api.DTOs;

namespace OpenDoors.Api.Services
{
    /// <summary>
    /// Analisa respostas do teste vocacional via IA.
    /// A persistência fica no Controller (separação de responsabilidades).
    /// </summary>
    public class AnalisarTesteService
    {
        private readonly GroqService _groq;

        public AnalisarTesteService(GroqService groq) => _groq = groq;

        public async Task<PerfilVocacionalDto> AnalisarAsync(List<RespostaVocacionalDto> respostas)
        {
            if (respostas.Count == 0)
                throw new ArgumentException("A lista de respostas não pode estar vazia.");

            var respostasFormatadas = string.Join("\n", respostas.Select(r =>
                $"Pergunta {r.PerguntaId}: {r.Pergunta}\nResposta: {r.Resposta}"));

            const string system =
                "Você é um especialista em psicologia vocacional. " +
                "Responda APENAS com JSON válido, sem texto adicional, sem markdown, sem ```json.";

            var user = $$"""
                Analise estas respostas e retorne um JSON com:
                {
                  "perfilDominante": "nome do perfil",
                  "areasSugeridas": ["área1", "área2", "área3"],
                  "pontosFortes": ["ponto1", "ponto2", "ponto3"],
                  "descricaoPerfil": "descrição em 2-3 frases"
                }

                Respostas:
                {{respostasFormatadas}}
                """;

            return await _groq.ChatJsonAsync<PerfilVocacionalDto>(system, user);
        }
    }
}