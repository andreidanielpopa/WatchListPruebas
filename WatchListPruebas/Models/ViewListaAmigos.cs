using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WatchListPruebas.Models
{
    [Table("vw_VistaAmigos")]
    public class ViewListaAmigos
    {
        [Key]
        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("AmigosJson")]
        public string? AmigosJson { get; set; }

        [NotMapped]
        public List<Amigo> Amigos =>
            string.IsNullOrEmpty(AmigosJson)
                ? new List<Amigo>()
                : JsonConvert.DeserializeObject<List<Amigo>>(AmigosJson);
    }

    public class Amigo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string FotoPerfil { get; set; }
        public string TipoSolicitud { get; set; }
        public int IdUsuario1 { get; set; }
    }
}
