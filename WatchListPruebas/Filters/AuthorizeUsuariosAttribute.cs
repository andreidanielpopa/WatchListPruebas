using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WatchListPruebas.Filters
{
    public class AuthorizeUsuariosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            // Se obtienen los valores de controlador y acción de la ruta
            string controller = context.RouteData.Values["controller"]?.ToString() ?? "";
            string action = context.RouteData.Values["action"]?.ToString() ?? "";

            // Se buscan posibles parámetros en la ruta según las nuevas convenciones
            object routeId = context.RouteData.Values["idcontenido"]
                           ?? context.RouteData.Values["idLista"]
                           ?? context.RouteData.Values["idCapitulo"]
                           ?? context.RouteData.Values["idProgreso"]
                           ?? context.RouteData.Values["idUsuario"]
                           ?? context.RouteData.Values["id"];

            // Cargamos TempData para guardar la información y poder redireccionar luego
            ITempDataProvider provider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();
            var tempData = provider.LoadTempData(context.HttpContext);
            tempData["controller"] = controller;
            tempData["action"] = action;
            if (routeId != null)
            {
                tempData["id"] = routeId.ToString();
            }
            else
            {
                tempData.Remove("id");
            }
            provider.SaveTempData(context.HttpContext, tempData);

            // Si el usuario no está autenticado, redirige a Usuarios/LogIn
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = GetRoute("Usuarios", "LogIn");
            }
        }

        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            var ruta = new RouteValueDictionary(new { controller = controller, action = action });
            return new RedirectToRouteResult(ruta);
        }
    }
}
