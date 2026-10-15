namespace OpenDoors.Api.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public Guid ContaId { get; set; }
    }
}