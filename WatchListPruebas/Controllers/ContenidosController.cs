using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using WatchListPruebas.Filters;
using WatchListPruebas.Models;
using WatchListPruebas.Repositories;

namespace WatchListPruebas.Controllers
{

    public class ContenidosController : Controller
    {
        private RepositoryWatchlist repo;

        public ContenidosController(RepositoryWatchlist repo)
        {
            this.repo = repo;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Detalles(int idcontenido)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != null)
            {
                int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                List<ProgresoUsuarioContenido> progreso = await this.repo.GetProgresoUsuarioContenidosAsync(idUsuario, idcontenido);

                ViewData["progeso"] = progreso;

                List<Contenido> meGustas = await this.repo.GetContenidosMeGustaAsync(idUsuario);

                ViewData["megustas"] = meGustas;
            }

            var contenido = await repo.ObtenerDetallesContenidoAsync(idcontenido);
            if (contenido == null) return NotFound();

            return View(contenido);
        }

        public async Task<IActionResult> ListadoContenidos(int idcontenido)
        {
            if (idcontenido == 2)
            {
                List<ViewContenidoValorado> topPeliculas = await repo.GetTopContenidosAsync(idcontenido);

                ViewData["Title"] = "Peliculas";

                return View(topPeliculas);

            }
            else if (idcontenido == 1)
            {
                List<ViewContenidoValorado> topSeries = await repo.GetTopContenidosAsync(idcontenido);

                ViewData["Title"] = "Series";

                return View(topSeries);
            }
            return View();
        }

        public IActionResult Buscador()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string query)
        {

            var resultados = await repo.BuscarContenidosAsync(query);

            var datos = resultados.Select(x => new { id = x.IdContenido, titulo = x.Titulo });
            return Json(datos);
        }

        public IActionResult InsertValoracion()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> InsertValoracion(int idContenido, Valoracion model)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await repo.InsertValoracionAsync(idUsuario, idContenido, model.Nota, model.Opinion);

            return RedirectToAction("Detalles", "Contenidos", new { idcontenido = idContenido });
        }

        public async Task<IActionResult> InsertContenidoLista()
        {
            int usuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            UsuariosListasReproduccion user = await this.repo.FindUsuarioById(usuario);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> InsertContenidoLista(int idContenido, int idLista)
        {
            await this.repo.InsertContenidoListaAsync(idLista, idContenido);

            return RedirectToAction("Detalles", "Contenidos", new { idcontenido = idContenido });
        }

        [HttpPost]
        public async Task<IActionResult> InsertProgreso(int idCapitulo, int idContenido)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await repo.InsertProgresoUsuarioContenidoAsync(idUsuario, idCapitulo);
            return RedirectToAction("Detalles", "Contenidos", new { idcontenido = idContenido });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProgreso(int idProgreso, int idContenido)
        {
            await repo.DeleteProgresoUsuarioContenidoAsync(idProgreso);
            return RedirectToAction("Detalles", "Contenidos", new { idcontenido = idContenido });
        }

        public async Task<IActionResult> AddMeGusta(int idContenido)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await repo.AddMeGustaAsync(idUsuario, idContenido);
            return RedirectToAction("Detalles", "Contenidos", new { idcontenido = idContenido });
        }

        public async Task<IActionResult> RemoveMeGusta(int idContenido)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await repo.RemoveMeGustaAsync(idUsuario, idContenido);
            return RedirectToAction("Detalles", "Contenidos", new { idcontenido = idContenido });
        }
    }
}
