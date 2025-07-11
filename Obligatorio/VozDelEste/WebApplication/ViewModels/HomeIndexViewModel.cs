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
      public List<ProgramaDTO> siguientesProgramas { get; set; }
      public List<ProgramaDTO> programacionDiaria { get; set; }
      public List<Patrocinador> patrocinadores { get; set; }
      public List<Noticia> noticiasResumen { get; set; }
   }
}