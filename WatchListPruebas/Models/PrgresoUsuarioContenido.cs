using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("vw_ProgresoUsuarioContenido")]
    public class ProgresoUsuarioContenido
    {
        [Key]
        [Column("idProgreso")]
        public int IdProgreso { get; set; }

        [Column("idCapitulo")]
        public int IdCapitulo { get; set; }

        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("idContenido")]
        public int IdContenido { get; set; }

        [Column("idTemporada")]
        public int IdTemporada { get; set; }

        [Column("numeroTemporada")]
        public int NumeroTemporada { get; set; }
    }
}
