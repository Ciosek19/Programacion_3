using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
   public class NoticiaService
   {
      private readonly VozDelEsteBDEntities _contexto;

      public NoticiaService(VozDelEsteBDEntities contexto)
      {
         _contexto = contexto;
      }

      public List<Noticia> ObtenerResumenNoticias(int cantidad)
      {
         var ahora = DateTime.Now;
         var noticias = _contexto.Noticia.
            Take(cantidad).
            OrderByDescending(n => n.FechaPublicacion)
            .ToList();

         return noticias;
      }

      public List<Noticia> ObtenerNoticiasSemanal(int cantidad)
      {
         var ahora = DateTime.Now;
         var noticias = _contexto.Noticia
            .Where(n => n.FechaPublicacion >= DateTime.Now.AddDays(-7))
            .OrderByDescending(n => n.FechaPublicacion)
            .ToList();

         return noticias;
      }
   }
}