using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            var consulta = await context.ViewListasReproduccion.OrderByDescending(x => x.NumeroLikes).Where(x => x.EsPublica).ToListAsync();

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

        public async Task InsertListaReproduccion(string nombre, int idUsuario, int idTipoLista, bool esPublica)
        {
            ListaReproduccion lista = new ListaReproduccion
            {
                IdLista = await GetMaxIdListasReproduccion(),
                IdUsuario = idUsuario,
                IdTipoLista = idTipoLista,
                Nombre = nombre,
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

            var listasColaborativas = await context.ListasColaborativas.Where(x => x.Colaborador == idUsuario).ToListAsync();

            UsuariosListasReproduccion user = new UsuariosListasReproduccion
            {
                User = userData,
                ListasUser = listas,
                ListasColaborativasUser = listasColaborativas
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

        public async Task<ViewListaAmigos> FindAmigosColaboradoresAsync(int idLista, int idUsuario)
        {
            var colaboradores = await context.ColaboradoresListas.Where(x => x.IdLista == idLista).ToListAsync();
            var amigos = await FindAmigosByUserId(idUsuario, 2);
            var amigosNoColaboradores = amigos.Amigos.Where(amigo => !colaboradores.Any(c => c.IdUsuario == amigo.Id)).ToList();

            ViewListaAmigos viewModel = new ViewListaAmigos();
            viewModel.AmigosJson = JsonConvert.SerializeObject(amigosNoColaboradores);

            return viewModel;

        }

        public async Task AddColaboradorListaAsync(int idLista, int idUsuario)
        {
            ColaboradorLista colaborador = new ColaboradorLista
            {
                IdLista = idLista,
                IdUsuario = idUsuario,
            };

            await this.context.ColaboradoresListas.AddAsync(colaborador);

            await this.context.SaveChangesAsync();
        }

        private async Task<int> GetMaxIdValoracionesAsync()
        {
            int maxId = 0;
            if (await this.context.Valoraciones.AnyAsync())
            {
                maxId = await this.context.Valoraciones.MaxAsync(v => v.IdValoracion);
            }
            return maxId + 1;
        }

        public async Task InsertValoracionAsync(int idUsuario, int idContenido, int nota, string opinion)
        {
            Valoracion valoracion = new Valoracion
            {
                IdValoracion = await GetMaxIdValoracionesAsync(),
                IdUsuario = idUsuario,
                IdContenido = idContenido,
                Nota = nota,
                Opinion = opinion,
                FechaCreacion = DateTime.Now
            };

            await this.context.Valoraciones.AddAsync(valoracion);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<ProgresoUsuarioContenido>> GetProgresoUsuarioContenidosAsync(int idUsuario, int idContenido)
        {
            return await this.context.ProgresosUsuarioContenidos.Where(x => x.IdUsuario == idUsuario && x.IdContenido == idContenido).ToListAsync();
        }

        public async Task InsertProgresoUsuarioContenidoAsync(int idUsuario, int idCapitulo)
        {
            string sql = "sp_InsertProgresoYContenido @idUsuario, @idCapitulo";
            SqlParameter paramUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter paramCapitulo = new SqlParameter("@idCapitulo", idCapitulo);

            await this.context.Database.ExecuteSqlRawAsync(sql, paramUsuario, paramCapitulo);
        }

        public async Task DeleteProgresoUsuarioContenidoAsync(int idProgreso)
        {
            var progreso = await this.context.Progresos.FindAsync(idProgreso);
            if (progreso != null)
            {
                this.context.Progresos.Remove(progreso);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<Contenido>> GetContenidosMeGustaAsync(int idUsuario)
        {
            var consulta = from lista in this.context.ListasReproduccion
                           join lc in this.context.ListasContenidos on lista.IdLista equals lc.IdLista
                           join contenido in this.context.Contenidos on lc.IdContenido equals contenido.IdContenido
                           where lista.IdUsuario == idUsuario && lista.IdTipoLista == 1
                           select contenido;

            return await consulta.ToListAsync();
        }

        public async Task AddMeGustaAsync(int idUsuario, int idContenido)
        {
            // Se asume que la lista "Me gusta" (idTipoLista = 1) existe para el usuario.
            ListaReproduccion listaMeGusta = await this.context.ListasReproduccion
                .FirstOrDefaultAsync(l => l.IdUsuario == idUsuario && l.IdTipoLista == 1);

            ListaContenidos nuevo = new ListaContenidos
            {
                IdLista = listaMeGusta.IdLista,
                IdContenido = idContenido
            };

            await this.context.ListasContenidos.AddAsync(nuevo);
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveMeGustaAsync(int idUsuario, int idContenido)
        {
            // Se asume que la lista "Me gusta" (idTipoLista = 1) existe para el usuario.
            var listaMeGusta = await this.context.ListasReproduccion
                .FirstOrDefaultAsync(l => l.IdUsuario == idUsuario && l.IdTipoLista == 1);

            // Se asume que la relación existe.
            var registro = await this.context.ListasContenidos
                .FirstOrDefaultAsync(lc => lc.IdLista == listaMeGusta.IdLista && lc.IdContenido == idContenido);

            this.context.ListasContenidos.Remove(registro);
            await this.context.SaveChangesAsync();
        }

        public async Task AddLikeListaAsync(int idUsuario, int idLista)
        {
            // Obtener la lista a partir del idLista
            var lista = await this.context.ListasReproduccion.FirstOrDefaultAsync(l => l.IdLista == idLista);

            // Solo se permite dar like si la lista es pública
            if (lista != null && lista.EsPublica)
            {
                ListaLike nuevo = new ListaLike
                {
                    IdUsuario = idUsuario,
                    IdLista = idLista,
                    FechaCreacion = DateTime.Now
                };

                await this.context.ListaLikes.AddAsync(nuevo);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task RemoveLikeListaAsync(int idUsuario, int idLista)
        {
            var registro = await this.context.ListaLikes
                .FirstOrDefaultAsync(l => l.IdUsuario == idUsuario && l.IdLista == idLista);

            if (registro != null)
            {
                this.context.ListaLikes.Remove(registro);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<bool> ListaEstaLikeadaAsync(int idUsuario, int idLista)
        {
            return await this.context.ListaLikes.AnyAsync(l => l.IdUsuario == idUsuario && l.IdLista == idLista);
        }

        public async Task<List<ViewListaReproduccion>> GetViewListasLikeadasAsync(int idUsuario)
        {
            var listas = await (from v in this.context.ViewListasReproduccion
                                join like in this.context.ListaLikes on v.IdLista equals like.IdLista
                                where like.IdUsuario == idUsuario
                                select v)
                                .ToListAsync();
            return listas;
        }

        public async Task CrearUsuarioAsync(string nombreCompleto, string nombreUsuario, string email, string contrasena, int idFotoPerfil)
        {
            string sql = "sp_CrearUsuario @nombreCompleto, @nombreUsuario, @email, @contrasena, @idFotoPerfil";

            SqlParameter paramNombreCompleto = new SqlParameter("@nombreCompleto", nombreCompleto);
            SqlParameter paramNombreUsuario = new SqlParameter("@nombreUsuario", nombreUsuario);
            SqlParameter paramEmail = new SqlParameter("@email", email);
            SqlParameter paramContrasena = new SqlParameter("@contrasena", contrasena);
            SqlParameter paramIdFotoPerfil = new SqlParameter("@idFotoPerfil", idFotoPerfil);

            await this.context.Database.ExecuteSqlRawAsync(sql,
                paramNombreCompleto,
                paramNombreUsuario,
                paramEmail,
                paramContrasena,
                paramIdFotoPerfil);
        }

        public async Task<Usuario> LoginAsync(string nombreUsuario, string password)
        {
            Usuario user = await this.context.Usuarios.Where(x => x.NombreUsuario == nombreUsuario && x.Contrasena == password).FirstOrDefaultAsync();

            return user;
        }
    }
}
