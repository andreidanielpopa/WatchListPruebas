using Microsoft.AspNetCore.Mvc;
using WatchListPruebas.Models;
using WatchListPruebas.Repositories;

namespace WatchListPruebas.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryWatchlist repo;

        public UsuariosController(RepositoryWatchlist repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<ViewListadoUsuarios> listUsers = await this.repo.GetListadoUsuariosAsync();

            if (HttpContext.Session.GetString("usuario") != null)
            {
                int usuario = int.Parse(HttpContext.Session.GetString("usuario"));

                ViewListaAmigos amigos = await this.repo.FindAmigosByUserId(usuario,2);

                ViewData["amigos"] = amigos;

                ViewListaAmigos pendientes = await this.repo.FindAmigosByUserId(usuario, 1);

                ViewData["pendientes"] = pendientes;
            }
            else
            {
                ViewData["amigos"] = null;

                ViewData["pendientes"] = null;
            }
            
            return View(listUsers);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string usuario)
        {
            HttpContext.Session.SetString("usuario", usuario);
            return RedirectToAction("Perfil");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("usuario");
            return RedirectToAction("LogIn");
        }

        public async Task<IActionResult> Perfil()
        {
            int usuario = int.Parse(HttpContext.Session.GetString("usuario"));

            UsuariosListasReproduccion user = await this.repo.FindUsuarioById(usuario);
            return View(user);
        }

        public async Task<IActionResult> Amigos()
        {
            int usuario = int.Parse(HttpContext.Session.GetString("usuario"));

            ViewListaAmigos pendientes = await this.repo.FindAmigosByUserId(usuario, 1);
            ViewListaAmigos amigos = await this.repo.FindAmigosByUserId(usuario,2);

            ModelViewListaAmigos listaAmigos = new ModelViewListaAmigos
            {
                ListaAceptados = amigos,
                ListaPendientes = pendientes
            };

            return View(listaAmigos);
        }

        public async Task<IActionResult> AddAmigo(int idUsuario1, int idUsuario2)
        {
            await this.repo.CreateSolicitudAmistad(idUsuario1, idUsuario2);

            return RedirectToAction("Index", "Usuarios");
        }

        public async Task<IActionResult> AccionSolicitud(int idUsuario1, int idUsuario2, int accion)
        {
            await this.repo.AccionSolicitudAmistadAsync(idUsuario1, idUsuario2, accion);

            return RedirectToAction("Amigos");
        }
    }
}
