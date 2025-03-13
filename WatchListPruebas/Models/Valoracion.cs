using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("VALORACIONES")]
    public class Valoracion
    {
        [Key]
        [Column("idValoracion")]
        public int IdValoracion { get; set; }

        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("idContenido")]
        public int IdContenido { get; set; }

        [Column("nota")]
        public int Nota { get; set; }

        [Column("opinion")]
        public string Opinion { get; set; }

        [Column("fechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
