using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("vw_UsuarioCompleto")]
    public class Usuario
    {
        [Key]
        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("nombreCompleto")]
        public string NombreCompleto { get; set; }

        [Column("nombreUsuario")]
        public string NombreUsuario { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("contrasena")]
        public string Contrasena { get; set; }

        [Column("FotoPerfil")]
        public string? FotoPerfil { get; set; }
    }
}
