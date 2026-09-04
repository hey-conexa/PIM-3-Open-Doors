using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Vagas;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services
{
    public class VagaService : IVagaService
    {
        private readonly IVagaRepository _repository;

        public VagaService(IVagaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<VagaDto>> ListarTodos()
        {
            var vagas = await _repository.ListarTodos();
            return vagas.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<VagaDto>> ListarAbertas()
        {
            var vagasAbertas = await _repository.ListarAbertas();
            return vagasAbertas.Select(MapearParaDto).ToList();
        }

        private async Task<Vaga> ObterVagaModelPorId(int id)
        {
            var vaga = await _repository.BuscarPorId(id);
            if (vaga == null)
                throw new NotFoundException("Vaga não encontrada");

            return vaga;
        }

        public async Task<VagaDto> BuscarPorId(int id)
        {
            var vaga = await ObterVagaModelPorId(id);
            return MapearParaDto(vaga);
        }

        public async Task<VagaDto> Criar(CreateVagaDto novaVaga)
        {
            if (string.IsNullOrWhiteSpace(novaVaga.Titulo))
                throw new BadRequestException("O título da vaga é obrigatório");

            if (novaVaga.EmpresaId == Guid.Empty)
                throw new BadRequestException("A empresa é obrigatória");

            var vaga = new Vaga
            {
                EmpresaId = novaVaga.EmpresaId,
                Titulo = novaVaga.Titulo,
                Descricao = novaVaga.Descricao,
                Area = novaVaga.Area,
                Nivel = novaVaga.Nivel,
                CursosAceitos = novaVaga.CursosAceitos,
                SemestreMinimo = novaVaga.SemestreMinimo,
                HabilidadesRequeridas = novaVaga.HabilidadesRequeridas,
                HabilidadesDiferenciais = novaVaga.HabilidadesDiferenciais,
                CargaHoraria = novaVaga.CargaHoraria,
                Modalidade = novaVaga.Modalidade,
                Cidade = novaVaga.Cidade,
                Estado = novaVaga.Estado,
                Bolsa = novaVaga.Bolsa,
                Beneficios = novaVaga.Beneficios,
                VagasDisponiveis = novaVaga.VagasDisponiveis,
                CandidaturasRecebidas = 0,
                Status = novaVaga.Status ?? "aberta",
                ExpiraEm = novaVaga.ExpiraEm
            };

            await _repository.Criar(vaga);

            return MapearParaDto(vaga);
        }

        public async Task<VagaDto> Atualizar(CreateVagaDto vagaAtualizada, int id)
        {
            var existente = await ObterVagaModelPorId(id);

            existente.Titulo = vagaAtualizada.Titulo;
            existente.Descricao = vagaAtualizada.Descricao;
            existente.Area = vagaAtualizada.Area;
            existente.Nivel = vagaAtualizada.Nivel;
            existente.CursosAceitos = vagaAtualizada.CursosAceitos;
            existente.SemestreMinimo = vagaAtualizada.SemestreMinimo;
            existente.HabilidadesRequeridas = vagaAtualizada.HabilidadesRequeridas;
            existente.HabilidadesDiferenciais = vagaAtualizada.HabilidadesDiferenciais;
            existente.CargaHoraria = vagaAtualizada.CargaHoraria;
            existente.Modalidade = vagaAtualizada.Modalidade;
            existente.Cidade = vagaAtualizada.Cidade;
            existente.Estado = vagaAtualizada.Estado;
            existente.Bolsa = vagaAtualizada.Bolsa;
            existente.Beneficios = vagaAtualizada.Beneficios;
            existente.VagasDisponiveis = vagaAtualizada.VagasDisponiveis;
            existente.Status = vagaAtualizada.Status;
            existente.ExpiraEm = vagaAtualizada.ExpiraEm;

            await _repository.Atualizar(existente);
            return MapearParaDto(existente);
        }

        public async Task<VagaDto> Deletar(int id)
        {
            var vaga = await ObterVagaModelPorId(id);
            await _repository.Deletar(vaga);
            return MapearParaDto(vaga);
        }

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