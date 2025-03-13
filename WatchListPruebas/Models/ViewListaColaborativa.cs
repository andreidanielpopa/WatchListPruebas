using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("vw_ListasColaborativas")]
    public class ViewListaColaborativa
    {
        [Key]
        [Column("idLista")]
        public int IdLista { get; set; }

        [Column("colaborador")]
        public int Colaborador { get; set; }

        [Column("propietario")]
        public int Propietario { get; set; }

        [Column("nombreLista")]
        public string NombreLista { get; set; }

        [Column("esPublica")]
        public bool EsPublica { get; set; }

        [Column("imagen1")]
        public string? Imagen1 { get; set; }

        [Column("imagen2")]
        public string? Imagen2 { get; set; }

        [Column("imagen3")]
        public string? Imagen3 { get; set; }

        [Column("imagen4")]
        public string? Imagen4 { get; set; }
    }
}
