using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OpenDoors.Api.Models;

namespace OpenDoors.Api.Services
{
    /// <summary>
    /// Responsável pela criação de tokens JWT.
    /// A chave secreta fica em appsettings.Development.json → "JWT:Secret"
    /// </summary>
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly string _jwtSecret;
        private const int ExpirationMinutes = 15;

        public JwtService(IConfiguration config)
        {
            _config = config;
            _jwtSecret = config["JWT:Secret"]
                ?? throw new InvalidOperationException("JWT:Secret não configurada no appsettings.Development.json");
        }

        /// <summary>
        /// Cria um token JWT contendo as informações de ContaId e TipoUsuario.
        /// </summary>
        /// <param name="login">Dados do login contendo ContaId e TipoUsuario</param>
        /// <returns>Token JWT em formato string</returns>
        public string CriarToken(Login login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
                new Claim("conta_id", login.ContaId.ToString()),
                new Claim("tipo_usuario", login.TipoUsuario.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}