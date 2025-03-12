using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WatchListPruebas.Data;
using WatchListPruebas.Models;

namespace WatchListPruebas.Repositories
{
    public class RepositoryWatchlist
    {
        private WatchlistContext context;

        public RepositoryWatchlist(WatchlistContext context)
        {
            this.context = context;
        }

        public async Task<ViewContenidoCompleto> ObtenerDetallesContenidoAsync(int id)
        {
            var consulta = await context.ViewContenidos.Where(c => c.Id == id).FirstOrDefaultAsync();
            return consulta;
        }

        public async Task<List<ViewContenidoValorado>> GetTopContenidosAsync(int tipocontenido, int? cantidad = null)
        {
            string sql = "sp_GetContenidoValorado @TipoContenido, @Cantidad";
            SqlParameter paramTipoContenido = new SqlParameter("@TipoContenido", tipocontenido);
            SqlParameter paramCantidad = new SqlParameter("@Cantidad", cantidad.HasValue ? (object)cantidad.Value : DBNull.Value);

            var consulta = await this.context.ViewContenidosValorados.FromSqlRaw(sql, paramTipoContenido, paramCantidad).ToListAsync();
            
            return consulta;
        }

        public async Task<List<ViewListaReproduccion>> GetTopListas()
        {
            var consulta = await context.ViewListasReproduccion.OrderByDescending(x => x.NumeroLikes).Where(x=>x.EsPublica).ToListAsync();

            return consulta;
        }

        private async Task<int> GetMaxIdListasReproduccion()
        {
            if (this.context.ListasReproduccion.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.ListasReproduccion.MaxAsync(x => x.IdLista) + 1;
            }
        }

        public async Task<ViewDetalleListaReproduccion> ObtenerDetallesListaReproduccionAsync(int id)
        {
            var consulta = await context.ViewDetallesListaReproduccion.Where(c => c.IdLista == id).FirstOrDefaultAsync();
            return consulta;
        }

        public async Task InsertListaReproduccion(string nombre, int idUsuario, int idTipoLista, int? esAdmin, int esPublica)
        {
            ListaReproduccion lista = new ListaReproduccion
            {
                IdLista = await GetMaxIdListasReproduccion(),
                IdUsuario = idUsuario,
                IdTipoLista = idTipoLista,
                Nombre = nombre,
                EsAdmin = esAdmin,
                EsPublica = esPublica
            };

            await this.context.ListasReproduccion.AddAsync(lista);

            await this.context.SaveChangesAsync();
        }

        public async Task<List<Contenido>> BuscarContenidosAsync(string query)
        {
            return await context.Contenidos
                .Where(c => c.Titulo.StartsWith(query)).Take(6)
                .ToListAsync();
        }

        public async Task InsertContenidoListaAsync(int idLista, int idContenido)
        {
            ListaContenidos lista = new ListaContenidos
            {
                IdLista = idLista,
                IdContenido = idContenido
            };

            await this.context.ListasContenidos.AddAsync(lista);

            await this.context.SaveChangesAsync();
        }

        public async Task<UsuariosListasReproduccion> FindUsuarioById(int idUsuario)
        {
            var userData = await context.Usuarios.Where(c => c.IdUsuario == idUsuario).FirstOrDefaultAsync();

            var listas = await context.ViewListasReproduccion.Where(x => x.IdUsuario == idUsuario).ToListAsync();

            UsuariosListasReproduccion user = new UsuariosListasReproduccion
            {
                User = userData,
                ListasUser = listas
            };

            return user;
        }

        public async Task<ViewListaAmigos> FindAmigosByUserId(int idUsuario, int tipoSolicitud)
        {
            string sql = "sp_VistaAmigosPorUsuarioYTipoSolicitud @idUsuario, @tipoSolicitud";
            SqlParameter paramIdUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter paramTipoSolicitud = new SqlParameter("@tipoSolicitud", tipoSolicitud);

            var consulta = await this.context.ListaAmigos.FromSqlRaw(sql, paramIdUsuario, paramTipoSolicitud).AsNoTracking().ToListAsync();
            var resultados = consulta.FirstOrDefault();
            
            return resultados;
        }

        public async Task<List<ViewListadoUsuarios>> GetListadoUsuariosAsync()
        {
            var listUsuarios = await context.ListaUsuarios.ToListAsync();

            return listUsuarios;
        }

        public async Task CreateSolicitudAmistad(int idUsuario1, int idUsuario2) 
        {
            AmigoTable solicitud = new AmigoTable
            {
                IdUsuario1 = idUsuario1,
                IdUsuario2 = idUsuario2,
                IdTipoDeSolicitud = 1
            };

            await this.context.AmigosTable.AddAsync(solicitud);

            await this.context.SaveChangesAsync();
        }

        public async Task<AmigoTable> FindSolicitudAmistadAsync(int idUsuario1, int idUsuario2)
        {
            var consulta = await this.context.AmigosTable
                .Where(x => (x.IdUsuario1 == idUsuario1 && x.IdUsuario2 == idUsuario2)
                         || (x.IdUsuario1 == idUsuario2 && x.IdUsuario2 == idUsuario1))
                .FirstOrDefaultAsync();

            return consulta;
        }

        public async Task AccionSolicitudAmistadAsync(int idUsuario1, int idUsuario2, int accion)
        {
            AmigoTable solicitud = await FindSolicitudAmistadAsync(idUsuario1, idUsuario2);

            if (accion == 2)
            {
                solicitud.IdTipoDeSolicitud = 2;

                this.context.AmigosTable.Update(solicitud);
            }
            else if (accion == 3)
            {
                solicitud.IdTipoDeSolicitud = 3;

                this.context.AmigosTable.Update(solicitud);
            }

            await this.context.SaveChangesAsync();
        }

    }
}
