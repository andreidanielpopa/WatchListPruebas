using Microsoft.EntityFrameworkCore;
using WatchListPruebas.Models;

namespace WatchListPruebas.Data
{
    public class WatchlistContext : DbContext
    {
        public WatchlistContext(DbContextOptions<WatchlistContext> options) :base(options) { }

        public DbSet<ViewContenidoCompleto> ViewContenidos { get; set; }

        public DbSet<ViewContenidoValorado> ViewContenidosValorados { get; set; }

        public DbSet<ViewListaReproduccion> ViewListasReproduccion { get; set; }

        public DbSet<ListaReproduccion> ListasReproduccion { get; set; }

        public DbSet<ViewDetalleListaReproduccion> ViewDetallesListaReproduccion { get; set; }

        public DbSet<Contenido> Contenidos { get; set; }

        public DbSet<ListaContenidos> ListasContenidos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<ViewListaAmigos> ListaAmigos { get; set; }

        public DbSet<ViewListadoUsuarios> ListaUsuarios { get; set; }

        public DbSet<AmigoTable> AmigosTable { get; set; }

        public DbSet<ViewListaColaborativa> ListasColaborativas { get; set; }

        public DbSet<ColaboradorLista> ColaboradoresListas { get; set; }

        public DbSet<Valoracion> Valoraciones { get; set; }

        public DbSet<ProgresoUsuarioContenido> ProgresosUsuarioContenidos { get; set; }

        public DbSet<Progreso> Progresos { get; set; }

        public DbSet<ListaLike> ListaLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListaContenidos>()
                        .HasKey(lc => new { lc.IdLista, lc.IdContenido });

            modelBuilder.Entity<AmigoTable>()
                        .HasKey(a => new { a.IdUsuario1, a.IdUsuario2 });

            modelBuilder.Entity<ColaboradorLista>()
                        .HasKey(c => new { c.IdLista , c.IdUsuario });

            modelBuilder.Entity<ListaLike>()
                        .HasKey(c => new { c.IdUsuario, c.IdLista });
        }


    }


}
