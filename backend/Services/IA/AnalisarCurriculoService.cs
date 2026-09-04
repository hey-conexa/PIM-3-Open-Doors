using System.Text;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Exceptions;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Interfaces.IA;
using OpenDoors.Api.Models;
using UglyToad.PdfPig;

namespace OpenDoors.Api.Services.IA
{
    /// <summary>
    /// Analisa currículo (PDF) usando IA, extrai habilidades e salva no Supabase.
    /// </summary>
    public class AnalisarCurriculoService : IAnalisarCurriculoService
    {
        private readonly IChatIAService _groq;
        private readonly IEstudanteRepository _estudanteRepository;

        public AnalisarCurriculoService(IChatIAService groq, IEstudanteRepository estudanteRepository)
        {
            _groq = groq;
            _estudanteRepository = estudanteRepository;
        }

        /// <summary>
        /// Extrai texto de todas as páginas do PDF.
        /// </summary>
        public static string ExtrairTextoPdf(Stream pdfStream)
        {
            var sb = new StringBuilder();
            using var doc = PdfDocument.Open(pdfStream);
            foreach (var pagina in doc.GetPages())
            {
                foreach (var palavra in pagina.GetWords())
                    sb.Append(palavra.Text).Append(' ');
                sb.AppendLine();
            }
            return sb.ToString().Trim();
        }

        public async Task<CurriculoAnalisadoDto> AnalisarAsync(Guid estudanteId, Stream pdfStream)
        {
            var textoCurriculo = ExtrairTextoPdf(pdfStream);

            if (string.IsNullOrWhiteSpace(textoCurriculo))
                throw new InvalidOperationException("O PDF enviado não contém texto legível.");

            const string system =
                "Você é um especialista em RH e análise de currículos. " +
                "Analise o currículo fornecido e extraia as informações em JSON. " +
                "Responda APENAS com JSON válido, sem texto adicional, sem markdown, sem ```json.";

            var user = $$"""
                Analise este currículo e retorne um JSON com:
                {
                  "habilidades": ["lista", "de", "habilidades", "técnicas"],
                  "experiencias": [
                    {
                      "cargo": "nome do cargo",
                      "empresa": "nome da empresa",
                      "periodo": "período",
                      "descricao": "descrição resumida"
                    }
                  ],
                  "nivelExperiencia": "junior/pleno/senior",
                  "areasAtuacao": ["área1", "área2"]
                }

                Currículo:
                {{textoCurriculo}}
                """;

            var dados = await _groq.ChatJsonAsync<CurriculoAnalisadoDto>(system, user);
            var estudante = await _estudanteRepository.BuscarPorId(estudanteId);

            if (estudante == null)
                throw new NotFoundException("Não foi possível encontrar o estudante.");

            estudante.HabilidadesExtraidas = dados.Habilidades;
            estudante.TemCurriculo = true;

            await _estudanteRepository.Atualizar(estudante);

            return dados;
        }
    }
}