using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WatchListPruebas.Models
{
    [Table("vw_ContenidoValorado")]
    public class ViewContenidoValorado
    {

        [Key]
        [Column("idContenido")]
        public int Id_Contenido { get; set; }

        [Column("nombreArchivoContenido")]
        public string Imagen { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

        [Column("NotaMedia")]
        public double NotaMedia { get; set; }

    }
}
