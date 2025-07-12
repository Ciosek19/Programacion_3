using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Services;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
   public class HomeController : Controller
   {
      private readonly ProgramacionService _programacionService;
      private readonly PatrocinadorService _patrocinadorService;
      private readonly NoticiaService _noticiaService;

      public HomeController()
      {
         var contexto = new VozDelEsteBDEntities();
         _programacionService = new ProgramacionService(contexto);
         _patrocinadorService = new PatrocinadorService(contexto);
         _noticiaService = new NoticiaService(contexto);
      }

      public ActionResult Index()
      {
         var programacionDiaria = _programacionService.ObtenerProgramasDelDia();
         var patrocinadores = _patrocinadorService.ObtenerPatrocinadores();
         var noticias = _noticiaService.ObtenerResumenNoticias(5);
         var enVivo = _programacionService.ProgramaEnVivo();
         var ahora = DateTime.Now;
         var siguiente = programacionDiaria
             .Where(p => p.FechaHorario > ahora)
             .OrderBy(p => p.FechaHorario)
             .FirstOrDefault();

         var dashboard = new HomeIndexViewModel
         {
            programacionDiaria = programacionDiaria,
            patrocinadores = patrocinadores,
            noticiasResumen = noticias,
            ProgramaSiguiente = siguiente,
            ProgramaEnVivo = enVivo
         };

         return View(dashboard);
      }
   }
}