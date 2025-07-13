using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
   public class CotizacionesViewModel
   {
      public CotizacionMoneda CotizacionActual { get; set; }
      public List<CotizacionMoneda> Historico { get; set; }
   }
}