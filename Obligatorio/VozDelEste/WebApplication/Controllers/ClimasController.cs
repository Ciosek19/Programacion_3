using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
   public class ClimasController : Controller
   {
      private VozDelEsteBDEntities db = new VozDelEsteBDEntities();
      private readonly ClimaService _climaService;

      public ClimasController()
      {
         _climaService = new ClimaService();
      }

      // GET: Climas
      public async Task<ActionResult> Home()
      {
         var pronostico = await _climaService.ObtenerPronosticoCompletoAsync();
         if (pronostico == null)
         {
            return View("Error"); // O podés mostrar un mensaje en la misma vista
         }

         return View(pronostico);
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
