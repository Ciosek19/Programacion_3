using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
   public class ClimaPronosticoViewModel
   {
      public Clima ClimaActual { get; set; }
      public List<Clima> Anteriores { get; set; }
      public List<Clima> Posteriores { get; set; }
   }
}