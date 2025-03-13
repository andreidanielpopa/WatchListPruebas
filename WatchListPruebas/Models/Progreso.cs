using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("PROGRESO")]
    public class Progreso
    {
        [Key]
        [Column("idProgreso")]
        public int IdProgreso { get; set; }

        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("idCapitulo")]
        public int IdCapitulo { get; set; }
    }
}
