using OpenDoors.Api.DTOs;
using OpenDoors.Api.Models;
using OpenDoors.Api.Interfaces.Empresas;
using OpenDoors.Api.Exceptions;

namespace OpenDoors.Api.Services.Empresas
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _repository;

        public EmpresaService(IEmpresaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<EmpresaDto>> ListarTodos()
        {
            var empresas = await _repository.ListarTodos();
            return empresas.Select(MapearParaDto).ToList();
        }

        public async Task<IReadOnlyList<EmpresaDto>> ListarAtivos()
        {
            var empresasAtivas = await _repository.ListarAtivos();
            return empresasAtivas.Select(MapearParaDto).ToList();
        }

        private async Task<Empresa> ObterEmpresaModelPorId(Guid id)
        {
            var empresa = await _repository.BuscarPorId(id);
            if (empresa == null)
                throw new NotFoundException("Empresa não encontrada");

            return empresa;
        }

        public async Task<EmpresaDto> BuscarPorId(Guid id)
        {
            var empresa = await ObterEmpresaModelPorId(id);
            return MapearParaDto(empresa);
        }

        public async Task<EmpresaDto> Criar(CreateEmpresaDto novaEmpresa)
        {
            if (novaEmpresa.Id == Guid.Empty)
                throw new BadRequestException("O Id (UUID do Supabase Auth) é obrigatório");

            if (string.IsNullOrWhiteSpace(novaEmpresa.RazaoSocial))
                throw new BadRequestException("Razão social é obrigatória");

            if (string.IsNullOrWhiteSpace(novaEmpresa.Cnpj))
                throw new BadRequestException("CNPJ é obrigatório");

            if (string.IsNullOrWhiteSpace(novaEmpresa.Email))
                throw new BadRequestException("Email é obrigatório");

            var nova = new Empresa
            {
                Id = novaEmpresa.Id,
                RazaoSocial = novaEmpresa.RazaoSocial,
                NomeFantasia = novaEmpresa.NomeFantasia,
                Cnpj = novaEmpresa.Cnpj,
                Email = novaEmpresa.Email,
                Telefone = novaEmpresa.Telefone,
                Site = novaEmpresa.Site,
                LogoUrl = novaEmpresa.LogoUrl,
                Cidade = novaEmpresa.Cidade,
                Estado = novaEmpresa.Estado,
                Cep = novaEmpresa.Cep,
                Setor = novaEmpresa.Setor,
                Porte = novaEmpresa.Porte,
                Descricao = novaEmpresa.Descricao,
                ResponsavelNome = novaEmpresa.ResponsavelNome,
                ResponsavelCargo = novaEmpresa.ResponsavelCargo,
                ResponsavelEmail = novaEmpresa.ResponsavelEmail,
                VagasAtivas = 0,
                TotalContratacoes = 0,
                Status = novaEmpresa.Status ?? "ativa"
            };

            await _repository.Criar(nova);

            return MapearParaDto(nova);
        }

        public async Task<EmpresaDto> Atualizar(CreateEmpresaDto empresaAtualizada, Guid id)
        {
            var existente = await ObterEmpresaModelPorId(id);

            existente.RazaoSocial = empresaAtualizada.RazaoSocial;
            existente.NomeFantasia = empresaAtualizada.NomeFantasia;
            existente.Cnpj = empresaAtualizada.Cnpj;
            existente.Email = empresaAtualizada.Email;
            existente.Telefone = empresaAtualizada.Telefone;
            existente.Site = empresaAtualizada.Site;
            existente.LogoUrl = empresaAtualizada.LogoUrl;
            existente.Cidade = empresaAtualizada.Cidade;
            existente.Estado = empresaAtualizada.Estado;
            existente.Cep = empresaAtualizada.Cep;
            existente.Setor = empresaAtualizada.Setor;
            existente.Porte = empresaAtualizada.Porte;
            existente.Descricao = empresaAtualizada.Descricao;
            existente.ResponsavelNome = empresaAtualizada.ResponsavelNome;
            existente.ResponsavelCargo = empresaAtualizada.ResponsavelCargo;
            existente.ResponsavelEmail = empresaAtualizada.ResponsavelEmail;
            existente.Status = empresaAtualizada.Status;

            await _repository.Atualizar(existente);
            return MapearParaDto(existente);
        }

        public async Task<EmpresaDto> Deletar(Guid id)
        {
            var empresa = await ObterEmpresaModelPorId(id);
            await _repository.Deletar(empresa);
            return MapearParaDto(empresa);
        }

        private static EmpresaDto MapearParaDto(Empresa e)
        {
            return new EmpresaDto
            {
                Id = e.Id,
                RazaoSocial = e.RazaoSocial,
                NomeFantasia = e.NomeFantasia,
                Cnpj = e.Cnpj,
                Email = e.Email,
                Telefone = e.Telefone,
                Site = e.Site,
                LogoUrl = e.LogoUrl,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Cep = e.Cep,
                Setor = e.Setor,
                Porte = e.Porte,
                Descricao = e.Descricao,
                ResponsavelNome = e.ResponsavelNome,
                ResponsavelCargo = e.ResponsavelCargo,
                ResponsavelEmail = e.ResponsavelEmail,
                VagasAtivas = e.VagasAtivas,
                TotalContratacoes = e.TotalContratacoes,
                Status = e.Status,
                CriadoEm = e.CriadoEm,
                AtualizadoEm = e.AtualizadoEm
            };
        }
    }
}