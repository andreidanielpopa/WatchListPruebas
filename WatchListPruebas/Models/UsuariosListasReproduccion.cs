namespace WatchListPruebas.Models
{
    public class UsuariosListasReproduccion
    {
        public Usuario User { get; set; }

        public List<ViewListaReproduccion> ListasUser { get; set; }

        public List<ViewListaColaborativa> ListasColaborativasUser { get; set; }
    }
}
