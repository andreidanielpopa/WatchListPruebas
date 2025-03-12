using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WatchListPruebas.Models
{
    
    [Table("LISTAS_CONTENIDOS")]
    public class ListaContenidos
    {
        [Column("idLista")]
        public int IdLista { get; set; }

        [Column("idContenido")]
        public int IdContenido { get; set; }
    }
}
