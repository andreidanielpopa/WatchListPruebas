using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WatchListPruebas.Models
{
    [Table("vw_ListasContenidos")]
    public class ViewDetalleListaReproduccion
    {
        [Key]
        [Column("idLista")]
        public int IdLista { get; set; }

        [Column("nombreLista")]
        public string NombreLista { get; set; }

        [Column("contenidosRelacionados")]
        public string? ContenidosJson { get; set; }

        // Propiedad deserializada
        [NotMapped]
        public List<ContenidoLista> Contenidos => DeserializeJson<ContenidoLista>(ContenidosJson);

        // Método genérico para deserializar JSON
        private List<T> DeserializeJson<T>(string json)
        {
            return string.IsNullOrEmpty(json) ? new List<T>() : JsonConvert.DeserializeObject<List<T>>(json);
        }
    }

    public class ContenidoLista
    {
        [Column("idContenido")]
        public int IdContenido { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

        [Column("imagenContenido")]
        public string ImagenContenido { get; set; }
    }
}


