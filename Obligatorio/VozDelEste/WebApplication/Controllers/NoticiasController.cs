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
    public class NoticiasController : Controller
    {
        private VozDelEsteBDEntities db = new VozDelEsteBDEntities();

        // GET: Noticias
        public ActionResult Home()
        {
            var noticias = db.Noticia.OrderByDescending(n => n.FechaPublicacion).ToList();
            return View(noticias);
        }

        public ActionResult Index()
        {
            var noticias = db.Noticia.OrderByDescending(n => n.FechaPublicacion).ToList();
            return View(noticias);
        }


        // GET: Noticias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = db.Noticia.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // GET: Noticias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Noticias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Noticia noticia, HttpPostedFileBase Imagen)
        {
            if (ModelState.IsValid)
            {
                // Si se sube una imagen
                if (Imagen != null && Imagen.ContentLength > 0)
                {
                    var nombreArchivo = Path.GetFileName(Imagen.FileName);
                    var rutaRelativa = Path.Combine("~/Assets/Imagenes", nombreArchivo);
                    var rutaFisica = Server.MapPath(rutaRelativa);

                    Imagen.SaveAs(rutaFisica);

                    // Guardar solo la URL relativa
                    noticia.Imagen = Url.Content(rutaRelativa);
                    noticia.FechaPublicacion = DateTime.Now;
                }

                db.Noticia.Add(noticia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noticia);
        }


        // GET: Noticias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = db.Noticia.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // POST: Noticias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Noticia noticia, HttpPostedFileBase Imagen)
        {
            if (ModelState.IsValid)
            {
                // Buscar la noticia existente en la base de datos
                var noticiaExistente = db.Noticia.Find(noticia.Id);

                if (noticiaExistente == null)
                {
                    return HttpNotFound();
                }

                // Actualizar campos básicos
                noticiaExistente.Titulo = noticia.Titulo;
                noticiaExistente.Contenido = noticia.Contenido;
                noticiaExistente.FechaPublicacion = DateTime.Now;

                // Si se sube una nueva imagen, guardarla y actualizar la propiedad
                if (Imagen != null && Imagen.ContentLength > 0)
                {
                    var nombreArchivo = Path.GetFileName(Imagen.FileName);
                    var rutaRelativa = Path.Combine("~/Assets/Imagenes", nombreArchivo);
                    var rutaFisica = Server.MapPath(rutaRelativa);

                    Imagen.SaveAs(rutaFisica);
                    noticiaExistente.Imagen = Url.Content(rutaRelativa);
                }

                db.Entry(noticiaExistente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noticia);
        }


        // GET: Noticias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = db.Noticia.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // POST: Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Noticia noticia = db.Noticia.Find(id);
            db.Noticia.Remove(noticia);
            db.SaveChanges();
            return RedirectToAction("Index");
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
