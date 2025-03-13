using System.Security.Claims;
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
        public async Task<IActionResult> Create(ListaReproduccion lista)
        {
            await this.repo.InsertListaReproduccion(lista.Nombre, lista.IdUsuario, lista.IdTipoLista, lista.EsPublica);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detalles(int id)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != null)
            {
                int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                bool isLiked = await repo.ListaEstaLikeadaAsync(idUsuario, id);
                ViewData["isLiked"] = isLiked;
            }

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

            return RedirectToAction("Detalles", new { id = idLista });
        }

        public async Task<IActionResult> AddColaboradorLista(int idLista)
        {
            int usuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            ViewListaAmigos amigos = await this.repo.FindAmigosColaboradoresAsync(idLista, usuario);

            return View(amigos);
        }

        [HttpPost]
        public async Task<IActionResult> AddColaboradorLista(int idLista, int idUsuario)
        {
            await this.repo.AddColaboradorListaAsync(idLista, idUsuario);

            return RedirectToAction("Detalles", "Listas", new { id = idLista });
        }
        public async Task<IActionResult> AddListaLike(int idLista)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await repo.AddLikeListaAsync(idUsuario, idLista);
            return RedirectToAction("Detalles", "Listas", new { id = idLista });
        }

        public async Task<IActionResult> RemoveListaLike(int idLista)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await repo.RemoveLikeListaAsync(idUsuario, idLista);
            return RedirectToAction("Detalles", "Listas", new { id = idLista });
        }
    }
}
