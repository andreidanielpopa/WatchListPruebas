using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WatchListPruebas.Models;
using WatchListPruebas.Repositories;
using WatchListPruebas.Filters;

namespace WatchListPruebas.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryWatchlist repo;

        public UsuariosController(RepositoryWatchlist repo)
        {
            this.repo = repo;
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Index()
        {
            List<ViewListadoUsuarios> listUsers = await this.repo.GetListadoUsuariosAsync();

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != null)
            {
                int usuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                ViewListaAmigos amigos = await this.repo.FindAmigosByUserId(usuario, 2);

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
        public async Task<IActionResult> LogIn(string nombreUsuario, string password)
        {
            Usuario user = await this.repo.LoginAsync(nombreUsuario, password);

            if (user != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name,
                        ClaimTypes.Role
                    );


                Claim claimId = new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString());
                Claim claimName = new Claim(ClaimTypes.Name, user.NombreUsuario);

                identity.AddClaim(claimName);
                identity.AddClaim(claimId);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                if (TempData["id"] != null)
                {
                    string id = TempData["id"].ToString();

                    return RedirectToAction(action, controller, new { id = id });
                }
                else
                {

                    return RedirectToAction(action, controller);
                }

            }
            else
            {
                ViewData["mensaje"] = "Usuario/Pass incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Privacy", "Home");
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            var usuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<ViewListaReproduccion> listasLikeadas = await repo.GetViewListasLikeadasAsync(usuario);
            ViewData["listaslikeadas"] = listasLikeadas;

            UsuariosListasReproduccion user = await this.repo.FindUsuarioById(usuario);
            return View(user);
        }

        public async Task<IActionResult> DetallePerfil(int idUsuario)
        {
            UsuariosListasReproduccion user = await this.repo.FindUsuarioById(idUsuario);
            return View(user);
        }

        public async Task<IActionResult> Amigos()
        {
            int usuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            ViewListaAmigos pendientes = await this.repo.FindAmigosByUserId(usuario, 1);
            ViewListaAmigos amigos = await this.repo.FindAmigosByUserId(usuario, 2);

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

        public IActionResult Registro()
        {
            return View();
        }

        // Acción POST para procesar el registro
        [HttpPost]
        public async Task<IActionResult> Registro(string nombreCompleto, string nombreUsuario, string email, string contrasena, int idFotoPerfil)
        {

            await repo.CrearUsuarioAsync(nombreCompleto, nombreUsuario, email, contrasena, idFotoPerfil);

            return RedirectToAction("Login", "Usuarios");

        }
    }
}
