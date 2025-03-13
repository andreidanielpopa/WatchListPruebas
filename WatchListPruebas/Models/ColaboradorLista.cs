using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("COLABORADORES_LISTAS")]
    public class ColaboradorLista
    {
        [Column("idLista")]
        public int IdLista { get; set; }

        [Column("idUsuario")]
        public int IdUsuario { get; set; }
    }
}
