using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WatchListPruebas.Models
{
    [Table("LISTAS_REPRODUCCION")]
    public class ListaReproduccion
    {
        [Key]
        [Column("idLista")]
        public int IdLista { get; set; }

        // id_usuario
        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        // id_tipo_lista
        [Column("idTipoLista")]
        public int IdTipoLista { get; set; }

        // nombre
        [Column("nombre")]
        public string Nombre { get; set; }

        // fecha_creacion
        [Column("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        // es_publica
        [Column("esPublica")]
        public bool EsPublica { get; set; }
    }
}
