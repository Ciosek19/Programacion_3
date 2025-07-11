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
    public class PatrocinadoresController : Controller
    {
        private VozDelEsteBDEntities db = new VozDelEsteBDEntities();

        // GET: Patrocinadores
        public ActionResult Index()
        {
            return View(db.Patrocinador.ToList());
        }

        // GET: Patrocinadores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Patrocinador patrocinador = db.Patrocinador.Find(id);
            if (patrocinador == null)
                return HttpNotFound();

            return View(patrocinador);
        }

        // GET: Patrocinadores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patrocinadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patrocinador patrocinador, HttpPostedFileBase Imagen)
        {
            if (ModelState.IsValid)
            {
                if (Imagen != null && Imagen.ContentLength > 0)
                {
                    var nombreArchivo = Path.GetFileName(Imagen.FileName);
                    var rutaRelativa = Path.Combine("~/Assets/Imagenes", nombreArchivo);
                    var rutaFisica = Server.MapPath(rutaRelativa);

                    Imagen.SaveAs(rutaFisica);
                    patrocinador.UrlImagen = Url.Content(rutaRelativa);
                }

                db.Patrocinador.Add(patrocinador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patrocinador);
        }

        // GET: Patrocinadores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Patrocinador patrocinador = db.Patrocinador.Find(id);
            if (patrocinador == null)
                return HttpNotFound();

            return View(patrocinador);
        }

        // POST: Patrocinadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patrocinador patrocinador, HttpPostedFileBase Imagen)
        {
            if (ModelState.IsValid)
            {
                var patrocinadorDb = db.Patrocinador.Find(patrocinador.Id);
                if (patrocinadorDb == null)
                    return HttpNotFound();

                patrocinadorDb.Nombre = patrocinador.Nombre;
                patrocinadorDb.Descripcion = patrocinador.Descripcion;
                patrocinadorDb.TransmisionDiaria = patrocinador.TransmisionDiaria;

                if (Imagen != null && Imagen.ContentLength > 0)
                {
                    var nombreArchivo = Path.GetFileName(Imagen.FileName);
                    var rutaRelativa = Path.Combine("~/Assets/Imagenes", nombreArchivo);
                    var rutaFisica = Server.MapPath(rutaRelativa);

                    Imagen.SaveAs(rutaFisica);
                    patrocinadorDb.UrlImagen = Url.Content(rutaRelativa);
                }

                db.Entry(patrocinadorDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patrocinador);
        }

        // GET: Patrocinadores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Patrocinador patrocinador = db.Patrocinador.Find(id);
            if (patrocinador == null)
                return HttpNotFound();

            return View(patrocinador);
        }

        // POST: Patrocinadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patrocinador patrocinador = db.Patrocinador.Find(id);
            db.Patrocinador.Remove(patrocinador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
