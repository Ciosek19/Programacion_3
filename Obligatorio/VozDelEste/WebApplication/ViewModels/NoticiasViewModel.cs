using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
   public class NoticiasViewModel
   {
      public List<Noticia> Noticias { get; set; }
      public int PaginaActual { get; set; }
      public int PaginasTotales { get; set; }
   }
}