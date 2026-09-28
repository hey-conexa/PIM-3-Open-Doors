using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.PerguntasTeste;
using OpenDoors.Api.Models;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services.PerguntasTeste
{
    /// <summary>
    /// Serviço de negócio para operações com perguntas de teste vocacional.
    /// Coordena validações, lógica de negócio e persistência através do repositório.
    /// </summary>
    public class PerguntaTesteService : IPerguntaTesteService
    {
        private readonly IPerguntaTesteRepository _repository;

        public PerguntaTesteService(IPerguntaTesteRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista perguntas ativas.
        /// Se sem filtros: retorna fixas + perguntas do mês atual.
        /// Se com tipo: filtra por tipo (fixa/mensal).
        /// Se com mês: filtra apenas por esse mês.
        /// </summary>
        public async Task<List<PerguntaTesteDto>> ListarAtivas(string? mes = null, string? tipo = null)
        {
            var perguntas = await _repository.ListarAtivas(tipo, mes);
            return perguntas.Select(MapearParaDto).ToList();
        }

        /// <summary>
        /// Lista todas as perguntas (ativas e inativas).
        /// </summary>
        public async Task<List<PerguntaTesteDto>> ListarTodas()
        {
            var perguntas = await _repository.ListarTodas();
            return perguntas.Select(MapearParaDto).ToList();
        }

        /// <summary>
        /// Busca uma pergunta específica por ID.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        public async Task<PerguntaTesteDto> BuscarPorId(int id)
        {
            if (id <= 0)
                throw new BadRequestException("ID da pergunta deve ser maior que 0");

            var pergunta = await _repository.BuscarPorId(id);
            if (pergunta == null)
                throw new NotFoundException("Pergunta não encontrada");

            return MapearParaDto(pergunta);
        }

        /// <summary>
        /// Cria uma nova pergunta de teste.
        /// Valida dados e determina tipo (fixa/mensal) baseado em MesReferencia.
        /// Lança BadRequestException se inválido.
        /// </summary>
        public async Task<PerguntaTesteDto> Criar(CreatePerguntaTesteDto dto)
        {
            ValidarCreatePerguntaTesteDto(dto);

            var novaPergununta = new PerguntaTeste
            {
                Pergunta = dto.Pergunta,
                Ordem = dto.Ordem,
                Ativa = true,
                Categoria = dto.Categoria ?? "geral",
                MesReferencia = dto.MesReferencia,
                Tipo = string.IsNullOrEmpty(dto.MesReferencia) ? "fixa" : "mensal",
                CriadoEm = DateTime.UtcNow
            };

            var perguntaCriada = await _repository.Criar(novaPergununta);
            return MapearParaDto(perguntaCriada);
        }

        /// <summary>
        /// Atualiza uma pergunta existente.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        public async Task<PerguntaTesteDto> Atualizar(int id, CreatePerguntaTesteDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("ID da pergunta deve ser maior que 0");

            ValidarCreatePerguntaTesteDto(dto);

            var pergunta = await _repository.BuscarPorId(id);
            if (pergunta == null)
                throw new NotFoundException("Pergunta não encontrada");

            pergunta.Pergunta = dto.Pergunta;
            pergunta.Ordem = dto.Ordem;
            pergunta.Categoria = dto.Categoria ?? pergunta.Categoria;
            pergunta.MesReferencia = dto.MesReferencia;

            var perguntaAtualizada = await _repository.Atualizar(pergunta);
            return MapearParaDto(perguntaAtualizada);
        }

        /// <summary>
        /// Desativa uma pergunta (soft delete).
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        public async Task<PerguntaTesteDto> Desativar(int id)
        {
            if (id <= 0)
                throw new BadRequestException("ID da pergunta deve ser maior que 0");

            try
            {
                var pergunta = await _repository.Desativar(id);
                return MapearParaDto(pergunta);
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException("Pergunta não encontrada");
            }
        }

        /// <summary>
        /// Valida os campos do DTO de criação de pergunta.
        /// </summary>
        private static void ValidarCreatePerguntaTesteDto(CreatePerguntaTesteDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Dados da pergunta são obrigatórios");

            if (string.IsNullOrWhiteSpace(dto.Pergunta))
                throw new BadRequestException("O campo 'pergunta' é obrigatório");
        }

        /// <summary>
        /// Converte uma Model PerguntaTeste em DTO PerguntaTesteDto.
        /// </summary>
        private static PerguntaTesteDto MapearParaDto(PerguntaTeste p) => new()
        {
            Id = p.Id,
            Pergunta = p.Pergunta,
            Ordem = p.Ordem,
            Categoria = p.Categoria,
            MesReferencia = p.MesReferencia,
            Tipo = p.Tipo
        };
    }
}