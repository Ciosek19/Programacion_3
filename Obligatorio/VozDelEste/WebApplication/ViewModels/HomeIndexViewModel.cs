using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;
using WebApplication.ViewModels.DTO;

namespace WebApplication.ViewModels
{
   public class HomeIndexViewModel
   {
      public List<ProgramaDTO> programacionDiaria { get; set; }
      public List<Patrocinador> patrocinadores { get; set; }
      public List<Noticia> noticiasResumen { get; set; }
      public ProgramaDTO ProgramaEnVivo { get; set; }
      public ProgramaDTO ProgramaSiguiente { get; set; }
   }
}