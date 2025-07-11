using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
   public class PatrocinadorService
   {
      private readonly VozDelEsteBDEntities _contexto;

      public PatrocinadorService(VozDelEsteBDEntities contexto)
      {
         _contexto = contexto;
      }

      public List<Patrocinador> ObtenerPatrocinadores()
      {
         return _contexto.Patrocinador.ToList();
      }
   }
}