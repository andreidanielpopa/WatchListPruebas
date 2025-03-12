using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("AMIGOS")]
    public class AmigoTable
    {
        [Column("idUsuario1")]
        public int IdUsuario1 { get; set; }

        [Column("idUsuario2")]
        public int IdUsuario2 { get; set; }

        [Column("tipoSolicitud")]
        public int IdTipoDeSolicitud { get; set; }

        [Column("fechaSolicitud")]
        public DateTime FechaSolicitud { get; set; }
    }
}
