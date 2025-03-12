using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("vw_Detalles_Completos_Contenido")]
    public class ViewContenidoCompleto
    {
        [Key]
        [Column("idContenido")]
        public int Id { get; set; }

        [Column("nombreContenido")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("fechaLanzamiento")]
        public DateTime? FechaLanzamiento { get; set; }

        [Column("nombreArchivoContenido")]
        public string Imagen { get; set; }

        [Column("tipoContenido")]
        public string TipoContenido { get; set; }

        [Column("categorias")]
        public string? CategoriasJson { get; set; }

        [Column("directores")]
        public string? DirectoresJson { get; set; }

        [Column("actores")]
        public string? ActoresJson { get; set; }

        [Column("plataformas")]
        public string? PlataformasJson { get; set; }

        [Column("temporadas")]
        public string? TemporadasJson { get; set; }

        // Propiedades deserializadas (No mapeadas en la base de datos)
        [NotMapped]
        public List<Categoria> Categorias => DeserializeJson<Categoria>(CategoriasJson);

        [NotMapped]
        public List<Director> Directores => DeserializeJson<Director>(DirectoresJson);

        [NotMapped]
        public List<Actor> Actores => DeserializeJson<Actor>(ActoresJson);

        [NotMapped]
        public List<Plataforma> Plataformas => DeserializeJson<Plataforma>(PlataformasJson);

        [NotMapped]
        public List<Temporada> Temporadas => DeserializeJson<Temporada>(TemporadasJson);

        // Método genérico para deserializar JSON
        private List<T> DeserializeJson<T>(string json)
        {
            return string.IsNullOrEmpty(json) ? new List<T>() : Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
        }
    }

    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
    }
    public class Director
    {
        public string Nombre { get; set; }

        public string ImagenDirector { get; set; }
    }
    public class Actor
    {
        public string Nombre { get; set; }

        public string NombrePersonaje { get; set; }

        public string ImagenActor { get; set; }
    }

    public class Plataforma
    {
        public string Nombre { get; set; }

        public string ImagenPlataforma { get; set; }
    }

    public class Temporada
    {

        public int NumeroTemporada { get; set; }

        public string TituloTemporada { get; set; }

        public List<Capitulo> Capitulos { get; set; } = new();
    }

    
    public class Capitulo
    {
        public string TituloCapitulo { get; set; }
        public int Duracion { get; set; }
    }
}

