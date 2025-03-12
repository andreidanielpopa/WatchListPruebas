using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("vw_ListasConContenidosYLikes")]
    public class ViewListaReproduccion
    {
        [Key]
        [Column("idLista")]
        public int IdLista { get; set; }

        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("NombreLista")]
        public string NombreLista { get; set; }

        [Column("esPublica")]
        public bool EsPublica { get; set; }

        [Column("NumeroLikes")]
        public int NumeroLikes { get; set; }

        [Column("Imagen1")]
        public string? Imagen1 { get; set; }

        [Column("Imagen2")]
        public string? Imagen2 { get; set; }

        [Column("Imagen3")]
        public string? Imagen3 { get; set; }

        [Column("Imagen4")]
        public string? Imagen4 { get; set; }
    }
}
