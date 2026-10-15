using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace OpenDoors.Api.Models
{
    [Table("logins")]
    public class Login : BaseModel
    {
        // ID auto-incremento
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        // FK — referência à conta (usuário)
        [Column("conta_id")]
        public Guid ContaId { get; set; }

        // Email do usuário
        [Column("email")]
        public string Email { get; set; }

        // Senha do usuário
        [Column("senha")]
        public string Senha { get; set; }

        [Column("tipo_usuario")]
        public TiposUsuario TipoUsuario { get; set; } = new TiposUsuario();
    }

    public enum TiposUsuario
    {
        Estudante,
        Empresa
    }
}
