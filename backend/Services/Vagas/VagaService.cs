using OpenDoors.Api.DTOs;
using OpenDoors.Api.Interfaces.Vagas;
using OpenDoors.Api.Models;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services.Vagas
{
    /// <summary>
    /// Serviço de negócio para operações com vagas.
    /// Coordena validações, lógica de negócio e persistência através do repositório.
    /// </summary>
    public class VagaService : IVagaService
    {
        private readonly IVagaRepository _repository;

        public VagaService(IVagaRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista todas as vagas.
        /// </summary>
        public async Task<List<VagaDto>> ListarTodas()
        {
            var vagas = await _repository.ListarTodas();
            return vagas.Select(MapearParaDto).ToList();
        }

        /// <summary>
        /// Lista apenas as vagas abertas.
        /// </summary>
        public async Task<List<VagaDto>> ListarAbertas()
        {
            var vagas = await _repository.ListarAbertas();
            return vagas.Select(MapearParaDto).ToList();
        }

        /// <summary>
        /// Lista vagas de uma empresa específica.
        /// </summary>
        public async Task<List<VagaDto>> ListarPorEmpresa(Guid empresaId)
        {
            if (empresaId == Guid.Empty)
                throw new BadRequestException("ID da empresa é obrigatório");

            var vagas = await _repository.ListarPorEmpresa(empresaId);
            return vagas.Select(MapearParaDto).ToList();
        }

        /// <summary>
        /// Busca uma vaga específica por ID.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        public async Task<VagaDto> BuscarPorId(int id)
        {
            if (id <= 0)
                throw new BadRequestException("ID da vaga deve ser maior que 0");

            var vaga = await _repository.BuscarPorId(id);
            if (vaga == null)
                throw new NotFoundException("Vaga não encontrada");

            return MapearParaDto(vaga);
        }

        /// <summary>
        /// Cria uma nova vaga.
        /// Valida dados e lança BadRequestException se inválidos.
        /// </summary>
        public async Task<VagaDto> Criar(CreateVagaDto dto)
        {
            ValidarCreateVagaDto(dto);

            var novaVaga = new Vaga
            {
                EmpresaId = dto.EmpresaId,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Area = dto.Area,
                Nivel = dto.Nivel,
                CursosAceitos = dto.CursosAceitos,
                SemestreMinimo = dto.SemestreMinimo,
                HabilidadesRequeridas = dto.HabilidadesRequeridas,
                HabilidadesDiferenciais = dto.HabilidadesDiferenciais,
                CargaHoraria = dto.CargaHoraria,
                Modalidade = dto.Modalidade,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Bolsa = dto.Bolsa,
                Beneficios = dto.Beneficios,
                VagasDisponiveis = dto.VagasDisponiveis,
                CandidaturasRecebidas = 0,
                Status = dto.Status ?? "aberta",
                ExpiraEm = dto.ExpiraEm
            };

            var vagaCriada = await _repository.Criar(novaVaga);
            return MapearParaDto(vagaCriada);
        }

        /// <summary>
        /// Atualiza uma vaga existente.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        public async Task<VagaDto> Atualizar(int id, CreateVagaDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("ID da vaga deve ser maior que 0");

            ValidarCreateVagaDto(dto);

            var vaga = await _repository.BuscarPorId(id);
            if (vaga == null)
                throw new NotFoundException("Vaga não encontrada");

            // Atualiza os campos
            vaga.Titulo = dto.Titulo;
            vaga.Descricao = dto.Descricao;
            vaga.Area = dto.Area;
            vaga.Nivel = dto.Nivel;
            vaga.CursosAceitos = dto.CursosAceitos;
            vaga.SemestreMinimo = dto.SemestreMinimo;
            vaga.HabilidadesRequeridas = dto.HabilidadesRequeridas;
            vaga.HabilidadesDiferenciais = dto.HabilidadesDiferenciais;
            vaga.CargaHoraria = dto.CargaHoraria;
            vaga.Modalidade = dto.Modalidade;
            vaga.Cidade = dto.Cidade;
            vaga.Estado = dto.Estado;
            vaga.Bolsa = dto.Bolsa;
            vaga.Beneficios = dto.Beneficios;
            vaga.VagasDisponiveis = dto.VagasDisponiveis;
            vaga.Status = dto.Status ?? vaga.Status;
            vaga.ExpiraEm = dto.ExpiraEm;

            var vagaAtualizada = await _repository.Atualizar(vaga);
            return MapearParaDto(vagaAtualizada);
        }

        /// <summary>
        /// Atualiza apenas o status de uma vaga.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        public async Task<VagaDto> AtualizarStatus(int id, string novoStatus)
        {
            if (id <= 0)
                throw new BadRequestException("ID da vaga deve ser maior que 0");

            if (string.IsNullOrWhiteSpace(novoStatus))
                throw new BadRequestException("Status não pode estar vazio");

            try
            {
                var vaga = await _repository.AtualizarStatus(id, novoStatus);
                return MapearParaDto(vaga);
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException("Vaga não encontrada");
            }
        }

        /// <summary>
        /// Deleta uma vaga.
        /// Lança NotFoundException se não encontrada.
        /// </summary>
        public async Task Deletar(int id)
        {
            if (id <= 0)
                throw new BadRequestException("ID da vaga deve ser maior que 0");

            var vaga = await _repository.BuscarPorId(id);
            if (vaga == null)
                throw new NotFoundException("Vaga não encontrada");

            await _repository.Deletar(id);
        }

        /// <summary>
        /// Valida os campos do DTO de criação de vaga.
        /// </summary>
        private static void ValidarCreateVagaDto(CreateVagaDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Dados da vaga são obrigatórios");

            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new BadRequestException("O título da vaga é obrigatório");

            if (dto.EmpresaId == Guid.Empty)
                throw new BadRequestException("A empresa é obrigatória");
        }

        /// <summary>
        /// Converte uma Model Vaga em DTO VagaDto.
        /// </summary>
        private static VagaDto MapearParaDto(Vaga vaga)
        {
            return new VagaDto
            {
                Id = vaga.Id,
                EmpresaId = vaga.EmpresaId,
                Titulo = vaga.Titulo,
                Descricao = vaga.Descricao,
                Area = vaga.Area,
                Nivel = vaga.Nivel,
                CursosAceitos = vaga.CursosAceitos,
                SemestreMinimo = vaga.SemestreMinimo,
                HabilidadesRequeridas = vaga.HabilidadesRequeridas,
                HabilidadesDiferenciais = vaga.HabilidadesDiferenciais,
                CargaHoraria = vaga.CargaHoraria,
                Modalidade = vaga.Modalidade,
                Cidade = vaga.Cidade,
                Estado = vaga.Estado,
                Bolsa = vaga.Bolsa,
                Beneficios = vaga.Beneficios,
                VagasDisponiveis = vaga.VagasDisponiveis,
                CandidaturasRecebidas = vaga.CandidaturasRecebidas,
                Status = vaga.Status,
                CriadoEm = vaga.CriadoEm,
                ExpiraEm = vaga.ExpiraEm,
                AtualizadoEm = vaga.AtualizadoEm
            };
        }
    }
}