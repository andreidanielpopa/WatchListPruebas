using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
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

            }else if(idcontenido == 1)
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
    }
}
