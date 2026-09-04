using OpenDoors.Api.DTOs;
using OpenDoors.Api.Exceptions;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Services.Estudantes
{
    public class EstudanteService : IEstudanteService
    {
        private readonly IEstudanteRepository _repository;
        public EstudanteService(IEstudanteRepository repository)
        {
            _repository = repository;
        }
        public async Task<IReadOnlyList<EstudanteDto>> ListarTodos()
        {
            var estudantes = await _repository.ListarTodos();
            return estudantes.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<EstudanteDto>> ListarAtivos()
        {
            var estudantesAtivos = await _repository.ListarAtivos();
            return estudantesAtivos.Select(MapearParaDto).ToList();
        }

        private async Task<Estudante> ObterEstudanteModelPorId(Guid id)
        {
            var estudante = await _repository.BuscarPorId(id);
            if (estudante == null)
                throw new NotFoundException("Usuário não encontrado.");

            return estudante;
        }
        public async Task<EstudanteDto> BuscarPorId(Guid id)
        {
            var estudante = await ObterEstudanteModelPorId(id);

            return MapearParaDto(estudante);
        }

        public async Task<EstudanteDto> Criar(CreateEstudanteDto novoEstudante)
        {
            if (novoEstudante.Id == Guid.Empty)
                throw new BadRequestException("O Id (UUID do Supabase Auth) é obrigatório");

            if (string.IsNullOrWhiteSpace(novoEstudante.Nome))
                throw new BadRequestException("Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(novoEstudante.Email))
                throw new BadRequestException("Email é obrigatório");

            var novo = new Estudante
            {
                Id = novoEstudante.Id,
                Nome = novoEstudante.Nome,
                Email = novoEstudante.Email,
                Telefone = novoEstudante.Telefone,
                Cpf = novoEstudante.Cpf,
                DataNascimento = novoEstudante.DataNascimento,
                Cidade = novoEstudante.Cidade,
                Estado = novoEstudante.Estado,
                FotoPerfilUrl = novoEstudante.FotoPerfilUrl,
                Instituicao = novoEstudante.Instituicao,
                Curso = novoEstudante.Curso,
                Semestre = novoEstudante.Semestre,
                Turno = novoEstudante.Turno,
                PrevisaoConclusao = novoEstudante.PrevisaoConclusao,
                CurriculoUrl = novoEstudante.CurriculoUrl,
                HabilidadesExtraidas = novoEstudante.HabilidadesExtraidas,
                TemCurriculo = novoEstudante.TemCurriculo ?? false,
                TemTesteVocacional = novoEstudante.TemTesteVocacional ?? false,
                Status = novoEstudante.Status ?? "ativo"
            };

            await _repository.Criar(novo);

            return MapearParaDto(novo);
        }

        public async Task<EstudanteDto> Atualizar(CreateEstudanteDto estudanteAtualizado, Guid id)
        {
            var existente = await ObterEstudanteModelPorId(id);

            existente.Nome = estudanteAtualizado.Nome;
            existente.Email = estudanteAtualizado.Email;
            existente.Telefone = estudanteAtualizado.Telefone;
            existente.Cpf = estudanteAtualizado.Cpf;
            existente.DataNascimento = estudanteAtualizado.DataNascimento;
            existente.Cidade = estudanteAtualizado.Cidade;
            existente.Estado = estudanteAtualizado.Estado;
            existente.FotoPerfilUrl = estudanteAtualizado.FotoPerfilUrl;
            existente.Instituicao = estudanteAtualizado.Instituicao;
            existente.Curso = estudanteAtualizado.Curso;
            existente.Semestre = estudanteAtualizado.Semestre;
            existente.Turno = estudanteAtualizado.Turno;
            existente.PrevisaoConclusao = estudanteAtualizado.PrevisaoConclusao;
            existente.CurriculoUrl = estudanteAtualizado.CurriculoUrl;
            existente.HabilidadesExtraidas = estudanteAtualizado.HabilidadesExtraidas;
            existente.TemCurriculo = estudanteAtualizado.TemCurriculo;
            existente.TemTesteVocacional = estudanteAtualizado.TemTesteVocacional;
            existente.Status = estudanteAtualizado.Status;

            await _repository.Atualizar(existente);
            return MapearParaDto(existente);
        }

        public async Task<EstudanteDto> Deletar(Guid id)
        {
            var estudante = await ObterEstudanteModelPorId(id);
            await _repository.Deletar(estudante);
            return MapearParaDto(estudante);
        }

        private static EstudanteDto MapearParaDto(Estudante e)
        {
            return new EstudanteDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Email = e.Email,
                Telefone = e.Telefone,
                Cpf = e.Cpf,
                DataNascimento = e.DataNascimento,
                Cidade = e.Cidade,
                Estado = e.Estado,
                FotoPerfilUrl = e.FotoPerfilUrl,
                Instituicao = e.Instituicao,
                Curso = e.Curso,
                Semestre = e.Semestre,
                Turno = e.Turno,
                PrevisaoConclusao = e.PrevisaoConclusao,
                CurriculoUrl = e.CurriculoUrl,
                HabilidadesExtraidas = e.HabilidadesExtraidas,
                TemCurriculo = e.TemCurriculo,
                TemTesteVocacional = e.TemTesteVocacional,
                Status = e.Status,
                CriadoEm = e.CriadoEm,
                AtualizadoEm = e.AtualizadoEm
            };
        }
    }
}
