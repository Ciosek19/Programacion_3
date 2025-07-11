using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

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

      var usuario = session["UsuarioActual"] as Usuario;
      if (usuario == null)
         return false;

      // Verifica si el usuario tiene el permiso indicado
      bool tienePermiso = usuario.Rol != null && usuario.Rol
          .SelectMany(r => r.Permiso)
          .Any(p => string.Equals(p.Nombre, _permiso, StringComparison.OrdinalIgnoreCase));

      return tienePermiso;
   }

   protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
   {
      // Si no está autorizado, redirige al login o a una página de error
      filterContext.Result = new RedirectResult("/Usuarios/Login");
   }
}