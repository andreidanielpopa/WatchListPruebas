using Microsoft.AspNetCore.Mvc;
using WatchListPruebas.Models;
using WatchListPruebas.Repositories;

namespace WatchListPruebas.Controllers
{
    public class ListasController : Controller
    {
        private RepositoryWatchlist repo;

        public ListasController(RepositoryWatchlist repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<ViewListaReproduccion> listasReproduccion = await this.repo.GetTopListas();
            return View(listasReproduccion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(ListaReproduccion lista)
        {
            await this.repo.InsertListaReproduccion(lista.Nombre, lista.IdUsuario, lista.IdTipoLista, lista.EsAdmin, lista.EsPublica);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var lista = await repo.ObtenerDetallesListaReproduccionAsync(id);
            return View(lista);
        }

        public IActionResult InsertContenidoLista()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertContenidoLista(int idLista, int idContenido)
        {
            await this.repo.InsertContenidoListaAsync(idLista, idContenido);

            return RedirectToAction("Detalles", new{ id = idLista});
        }
    }
}
