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
      [TienePermiso("Gestion Programas")]
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
      public ActionResult Create()
      {
         return View();
      }

      // POST: ProgramaRadios/Create
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Programas")]
      public ActionResult Create([Bind(Include = "Id,Nombre,ImagenUrl,Descripcion,Duracion")] ProgramaRadio programaRadio)
      {

         if (ModelState.IsValid)
         {
            db.ProgramaRadio.Add(programaRadio);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(programaRadio);
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


      // POST: ProgramaRadios/Edit/5
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Programas")]
      public ActionResult Create(ProgramaRadio programa, HttpPostedFileBase Imagen)
      {
         if (ModelState.IsValid)
         {
            if (Imagen != null && Imagen.ContentLength > 0)
            {
               var nombreArchivo = Path.GetFileName(Imagen.FileName);
               var rutaRelativa = Path.Combine("~/Assets/Imagenes", nombreArchivo);
               var rutaFisica = Server.MapPath(rutaRelativa);

               // Guardar imagen
               Imagen.SaveAs(rutaFisica);

               // Ruta a guardar en la base
               programa.ImagenUrl = Url.Content(rutaRelativa);
            }

            db.ProgramaRadio.Add(programa);
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
      [TienePermiso("Gestion Programas")]
      public ActionResult AgregarComentario(int ProgramaRadioId, string Contenido)
      {
         if (!User.Identity.IsAuthenticated)
         {
            return RedirectToAction("Login", "Usuarios");
         }

         var nombreUsuario = User.Identity.Name;
         var usuario = db.Usuario.Where(e => e.NombreUsuario == nombreUsuario).FirstOrDefault();
         if (usuario == null)
         {
            Redirect("Home/Index");
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
