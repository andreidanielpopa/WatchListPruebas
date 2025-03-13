using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchListPruebas.Models
{
    [Table("LISTA_LIKE")]
    public class ListaLike
    {
        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("idLista")]
        public int IdLista { get; set; }

        [Column("fechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
