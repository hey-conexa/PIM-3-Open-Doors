using OpenDoors.Api.DTOs;
using OpenDoors.Api.Exceptions;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Services
{
    /// <summary>
    /// Serviço responsável pela autenticação de usuários.
    /// Busca credenciais no banco de dados e gera tokens JWT.
    /// </summary>
    public class LoginService
    {
        private readonly Supabase.Client _supabase;
        private readonly JwtService _jwtService;

        public LoginService(Supabase.Client supabase, JwtService jwtService)
        {
            _supabase = supabase;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Autentica um usuário validando email e senha contra o banco de dados.
        /// </summary>
        /// <param name="dto">DTO contendo ContaId, Email e Senha</param>
        /// <returns>DTO com o token JWT gerado</returns>
        /// <exception cref="MissingRequiredFieldException">Quando campos obrigatórios estão ausentes</exception>
        /// <exception cref="InvalidCredentialsException">Quando email ou senha são inválidos</exception>
        public async Task<LoginResponseDto> AutenticarAsync(LoginDto dto)
        {
            // Validação de campos obrigatórios
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new MissingRequiredFieldException("Email");

            if (string.IsNullOrWhiteSpace(dto.Senha))
                throw new MissingRequiredFieldException("Senha");

            if (dto.ContaId == Guid.Empty)
                throw new MissingRequiredFieldException("ContaId");

            // Busca a conta no banco de dados pela chave primária ContaId
            var loginResult = await _supabase
                .From<Login>()
                .Where(l => l.ContaId == dto.ContaId)
                .Single();

            if (loginResult == null)
                throw new InvalidCredentialsException();

            // Compara o email informado com o armazenado
            if (!loginResult.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase))
                throw new InvalidCredentialsException();

            // Compara a senha informada com a armazenada
            // NOTA: Se a senha estiver com hash, utilize BCrypt.Net.BCrypt.Verify() aqui
            if (!loginResult.Senha.Equals(dto.Senha, StringComparison.Ordinal))
                throw new InvalidCredentialsException();

            // Gera o token JWT
            var token = _jwtService.CriarToken(loginResult);

            // Retorna o DTO com o token
            return new LoginResponseDto { Token = token };
        }
    }
}