using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("vw_ListadoUsuarios")]
    public class ViewListadoUsuarios
    {
        [Key]
        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("nombre_archivo")]
        public string? NombreArchivoFoto { get; set; }

        [Column("nombreUsuario")]
        public string Nombre { get; set; }

        [Column("ContenidosVistos")]
        public int ContenidosVistos { get; set; }

        [Column("NumeroListas")]
        public int NumeroListas { get; set; }

        [Column("NumeroAmigos")]
        public int NumeroAmigos { get; set; }
    }
}
