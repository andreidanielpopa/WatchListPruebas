using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchListPruebas.Models;
using WatchListPruebas.Repositories;

namespace WatchListPruebas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RepositoryWatchlist repo;

        public HomeController(ILogger<HomeController> logger, RepositoryWatchlist repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            List<ViewContenidoValorado> topPeliculas = await repo.GetTopContenidosAsync(2,6);
            List<ViewContenidoValorado> topSeries = await repo.GetTopContenidosAsync(1,6);

            ListaContenidoValorado lista = new ListaContenidoValorado 
            { 
                Peliculas = topPeliculas,
                Series = topSeries
            };

            return View(lista);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
