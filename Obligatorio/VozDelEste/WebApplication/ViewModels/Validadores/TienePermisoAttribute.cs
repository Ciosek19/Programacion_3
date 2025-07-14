using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.ViewModels.DTO;

public class TienePermisoAttribute : AuthorizeAttribute
{
   private readonly string _permiso;

   public TienePermisoAttribute(string permiso)
   {
      _permiso = permiso;
   }

   protected override bool AuthorizeCore(HttpContextBase httpContext)
   {
      var session = httpContext.Session;
      if (session == null)
         return false;

      var usuario = session["UsuarioActual"] as UsuarioSesion;
      if (usuario == null)
         return false;

      if (string.IsNullOrWhiteSpace(_permiso))
         return false;

      return usuario.Permisos.Any(p =>
          string.Equals(p, _permiso, StringComparison.OrdinalIgnoreCase));
   }

   protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
   {
      var session = filterContext.HttpContext.Session;
      if(session != null) filterContext.Result = new RedirectResult("Home/Index");
      else
      {
         // Si no está autorizado, redirige al login o a una página de error
         filterContext.Result = new RedirectResult("/Usuarios/Login");
      }
   }
}