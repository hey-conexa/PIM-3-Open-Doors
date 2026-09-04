using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.IA;

namespace OpenDoors.Api.Services.IA
{
    /// <summary>
    /// Analisa respostas do teste vocacional via IA.
    /// A persistência fica no Controller (separação de responsabilidades).
    /// </summary>
    public class AnalisarTesteService : IAnalisarTesteService
    {
        private readonly IChatIAService _groq;

        public AnalisarTesteService(IChatIAService groq) => _groq = groq;

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