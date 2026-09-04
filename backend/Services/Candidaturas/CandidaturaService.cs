using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Candidaturas;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services.Candidaturas
{
    public class CandidaturaService : ICandidaturaService
    {
        private readonly ICandidaturaRepository _repository;

        public CandidaturaService(ICandidaturaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<CandidaturaDto>> ListarTodos()
        {
            var candidaturas = await _repository.ListarTodos();
            return candidaturas.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<CandidaturaDto>> ListarPorEstudante(Guid estudanteId)
        {
            var candidaturas = await _repository.ListarPorEstudante(estudanteId);
            return candidaturas.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<CandidaturaDto>> ListarPorVaga(int vagaId)
        {
            var candidaturas = await _repository.ListarPorVaga(vagaId);
            return candidaturas.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<CandidaturaDto>> ListarPorEmpresa(Guid empresaId)
        {
            var candidaturas = await _repository.ListarPorEmpresa(empresaId);
            return candidaturas.Select(MapearParaDto).ToList();
        }

        private async Task<Candidatura> ObterCandidaturaModelPorId(int id)
        {
            var candidatura = await _repository.BuscarPorId(id);
            if (candidatura == null)
                throw new NotFoundException("Candidatura não encontrada");

            return candidatura;
        }

        public async Task<CandidaturaDto> BuscarPorId(int id)
        {
            var candidatura = await ObterCandidaturaModelPorId(id);
            return MapearParaDto(candidatura);
        }

        public async Task<CandidaturaDto> Criar(CreateCandidaturaDto novaCandidatura)
        {
            if (novaCandidatura.EstudanteId == Guid.Empty)
                throw new BadRequestException("EstudanteId é obrigatório");

            if (novaCandidatura.VagaId <= 0)
                throw new BadRequestException("VagaId é obrigatório");

            if (novaCandidatura.EmpresaId == Guid.Empty)
                throw new BadRequestException("EmpresaId é obrigatório");

            var nova = new Candidatura
            {
                EstudanteId = novaCandidatura.EstudanteId,
                VagaId = novaCandidatura.VagaId,
                EmpresaId = novaCandidatura.EmpresaId,
                Status = novaCandidatura.Status ?? "pendente",
                CartaApresentacao = novaCandidatura.CartaApresentacao,
                VisualizadoEmpresa = false
            };

            await _repository.Criar(nova);

            return MapearParaDto(nova);
        }

        public async Task<CandidaturaDto> AtualizarStatus(int id, string novoStatus)
        {
            var existente = await ObterCandidaturaModelPorId(id);

            existente.Status = novoStatus;
            await _repository.AtualizarStatus(existente);

            return MapearParaDto(existente);
        }

        public async Task<CandidaturaDto> Deletar(int id)
        {
            var candidatura = await ObterCandidaturaModelPorId(id);
            await _repository.Deletar(candidatura);
            return MapearParaDto(candidatura);
        }

        private static CandidaturaDto MapearParaDto(Candidatura c)
        {
            return new CandidaturaDto
            {
                Id = c.Id,
                EstudanteId = c.EstudanteId,
                VagaId = c.VagaId,
                EmpresaId = c.EmpresaId,
                Status = c.Status,
                CartaApresentacao = c.CartaApresentacao,
                ScoreCompatibilidade = c.ScoreCompatibilidade,
                PosicaoRanking = c.PosicaoRanking,
                VisualizadoEmpresa = c.VisualizadoEmpresa,
                DataVisualizacao = c.DataVisualizacao,
                CriadoEm = c.CriadoEm,
                AtualizadoEm = c.AtualizadoEm
            };
        }
    }
}