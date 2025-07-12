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

      public NoticiaService(VozDelEsteBDEntities context)
      {
         _contexto = context;
      }

      // Obtener todas las noticias ordenadas por fecha descendente (más recientes primero)
      public List<Noticia> ObtenerTodasNoticiasOrdenadasPorFecha()
      {
         return _contexto.Noticia
             .OrderByDescending(n => n.FechaPublicacion)
             .ToList();
      }

      // Obtener noticias paginadas de forma eficiente en base de datos
      public List<Noticia> ObtenerNoticiasPaginadas(int pagina, int pageSize)
      {
         return _contexto.Noticia
             .OrderByDescending(n => n.FechaPublicacion)
             .Skip((pagina - 1) * pageSize)
             .Take(pageSize)
             .ToList();
      }

      // Contar el total de noticias
      public int ContarNoticias()
      {
         return _contexto.Noticia.Count();
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