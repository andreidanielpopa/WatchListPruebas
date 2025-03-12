using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WatchListPruebas.Models
{
    [Table("CONTENIDOS")]
    public class Contenido
    {
        [Key]
        [Column("idContenido")]
        public int IdContenido { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

    }
}
