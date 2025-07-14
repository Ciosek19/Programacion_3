using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
   public class ProgramaRadiosController : Controller
   {
      private VozDelEsteBDEntities db = new VozDelEsteBDEntities();

      // GET: ProgramaRadios
      [TienePermiso("Gestion Programas")]
      public ActionResult Index()
      {
         return View(db.ProgramaRadio.ToList());
      }

      // GET: ProgramaRadios
      public ActionResult Home()
      {
         return View(db.ProgramaRadio.ToList());
      }

      // GET: ProgramaRadios/Details/5
      public ActionResult Details(int? id)
      {
         var programa = db.ProgramaRadio
                  .Include("ComentarioPrograma")
                  .FirstOrDefault(p => p.Id == id);

         if (programa == null)
         {
            return HttpNotFound();
         }

         return View(programa);
      }

      // GET: ProgramaRadios/Create
      [TienePermiso("Gestion Programas")]
      public ActionResult Create(int[] ConductoresSeleccionados = null)
      {
         using (var db = new VozDelEsteBDEntities())
         {
            var conductores = db.Conductor.Include("Persona").ToList();
            ViewBag.Conductores = new MultiSelectList(
                conductores.Select(c => new { c.Id, Nombre = c.Persona.Nombre + " " + c.Persona.Apellido }),
                "Id",
                "Nombre",
                ConductoresSeleccionados
            );
         }

         return View();
      }


      [HttpPost]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Programas")]
      public ActionResult Create(ProgramaRadio programa, int[] ConductoresSeleccionados, HttpPostedFileBase Imagen)
      {
         // Validar que al menos un conductor haya sido seleccionado
         if (ConductoresSeleccionados == null || ConductoresSeleccionados.Length == 0)
         {
            ModelState.AddModelError("ConductoresSeleccionados", "Debe seleccionar al menos un conductor.");
         }

         if (ModelState.IsValid)
         {
            // Guardar imagen si se subió
            if (Imagen != null && Imagen.ContentLength > 0)
            {
               var nombreArchivo = Path.GetFileName(Imagen.FileName);
               var ruta = Path.Combine(Server.MapPath("~/assets/imagenes"), nombreArchivo);
               Imagen.SaveAs(ruta);
               programa.ImagenUrl = "~/assets/imagenes/" + nombreArchivo;
            }

            // Asociar conductores seleccionados
            programa.Conductor = new List<Conductor>();
            foreach (var id in ConductoresSeleccionados)
            {
               var conductor = db.Conductor.Find(id);
               if (conductor != null)
               {
                  programa.Conductor.Add(conductor);
               }
            }

            db.ProgramaRadio.Add(programa);
            db.SaveChanges();

            return RedirectToAction("Index");
         }

         // Si hay error, volver a cargar la lista de conductores
         var conductores = db.Conductor.Include("Persona").ToList();
         ViewBag.Conductores = new MultiSelectList(
             conductores.Select(c => new { c.Id, Nombre = c.Persona.Nombre + " " + c.Persona.Apellido }),
             "Id",
             "Nombre",
             ConductoresSeleccionados
         );

         return View(programa);
      }


      // GET: ProgramaRadios/Edit/5
      [TienePermiso("Gestion Programas")]
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ProgramaRadio programaRadio = db.ProgramaRadio.Find(id);
         if (programaRadio == null)
         {
            return HttpNotFound();
         }
         return View(programaRadio);
      }

      [HttpPost]
      //[ValidateAntiForgeryToken]
      [TienePermiso("Gestion Programas")]
      public ActionResult Edit(ProgramaRadio programa, HttpPostedFileBase Imagen)
      {
         if (ModelState.IsValid)
         {
            var programaDb = db.ProgramaRadio.Find(programa.Id);
            if (programaDb == null)
            {
               return HttpNotFound();
            }

            // Actualizar campos básicos
            programaDb.Nombre = programa.Nombre;
            programaDb.Descripcion = programa.Descripcion;
            programaDb.Duracion = programa.Duracion;

            // Imagen (si se cargó una nueva)
            if (Imagen != null && Imagen.ContentLength > 0)
            {
               var nombreArchivo = Path.GetFileName(Imagen.FileName);
               var rutaRelativa = Path.Combine("~/Assets/Imagenes", nombreArchivo);
               var rutaFisica = Server.MapPath(rutaRelativa);

               Imagen.SaveAs(rutaFisica);
               programaDb.ImagenUrl = Url.Content(rutaRelativa);
            }

            db.Entry(programaDb).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
         }

         return View(programa);
      }

      // GET: ProgramaRadios/Delete/5
      [TienePermiso("Gestion Programas")]
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ProgramaRadio programaRadio = db.ProgramaRadio.Find(id);
         if (programaRadio == null)
         {
            return HttpNotFound();
         }
         return View(programaRadio);
      }

      // POST: ProgramaRadios/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Programas")]
      public ActionResult DeleteConfirmed(int id)
      {
         ProgramaRadio programaRadio = db.ProgramaRadio.Find(id);
         db.ProgramaRadio.Remove(programaRadio);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult AgregarComentario(int ProgramaRadioId, string Contenido)
      {
         if (!User.Identity.IsAuthenticated)
         {
            // Redirige siempre a Login si no está autenticado
            return RedirectToAction("Login", "Usuarios");
         }

         if (string.IsNullOrWhiteSpace(Contenido))
         {
            ModelState.AddModelError("Contenido", "El comentario no puede estar vacío.");
         }

         var programa = db.ProgramaRadio
             .Include("ComentarioPrograma.Usuario")
             .Include("Conductor.Persona")
             .FirstOrDefault(p => p.Id == ProgramaRadioId);

         if (programa == null)
         {
            return HttpNotFound();
         }

         if (!ModelState.IsValid)
         {
            // Retorna la vista Details con el modelo y los errores
            return View("Details", programa);
         }

         var nombreUsuario = User.Identity.Name;
         var usuario = db.Usuario.FirstOrDefault(e => e.NombreUsuario == nombreUsuario);
         if (usuario == null)
         {
            // Si por alguna razón no encuentra el usuario, redirige a login
            return RedirectToAction("Login", "Usuarios");
         }

         var comentario = new ComentarioPrograma
         {
            ProgramaID = ProgramaRadioId,
            Comentario = Contenido,
            Usuario = usuario
         };

         db.ComentarioPrograma.Add(comentario);
         db.SaveChanges();

         return RedirectToAction("Details", new { id = ProgramaRadioId });
      }


      [TienePermiso("Gestion Programas")]
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
