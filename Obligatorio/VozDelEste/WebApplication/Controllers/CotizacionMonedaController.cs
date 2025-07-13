using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebApplication.Models;
using WebApplication.Services;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
   public class CotizacionMonedaController : Controller
   {
      private VozDelEsteBDEntities db = new VozDelEsteBDEntities();
      private readonly CotizacionesService _cotizacionesService;

      public CotizacionMonedaController()
      {
         _cotizacionesService = new CotizacionesService();
      }

      public async Task<ActionResult> Home()
      {
         var historico = await _cotizacionesService.ObtenerHistoricoCotizacionesAsync();
         var viewModel = new CotizacionesViewModel
         {
            Historico = historico
         };

         return View(viewModel);
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
