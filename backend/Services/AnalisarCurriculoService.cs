using System.Text;
using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using UglyToad.PdfPig;

namespace OpenDoors.Api.Services
{
    /// <summary>
    /// Analisa currículo (PDF) usando IA, extrai habilidades e salva no Supabase.
    /// </summary>
    public class AnalisarCurriculoService
    {
        private readonly GroqService _groq;
        private readonly Supabase.Client _supabase;

        public AnalisarCurriculoService(GroqService groq, Supabase.Client supabase)
        {
            _groq = groq;
            _supabase = supabase;
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

            // Atualiza o estudante no banco (usando SUA Model existente)
            var estudante = await _supabase
                .From<Estudante>()
                .Where(e => e.Id == estudanteId)
                .Single();

            if (estudante == null)
                throw new KeyNotFoundException($"Estudante não encontrado: {estudanteId}");

            estudante.HabilidadesExtraidas = dados.Habilidades;
            estudante.TemCurriculo = true;
            await estudante.Update<Estudante>();

            return dados;
        }
    }
}