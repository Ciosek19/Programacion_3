using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class ProgramacionController : Controller
    {
        private readonly ProgramacionService _programacionService;
        private VozDelEsteBDEntities db = new VozDelEsteBDEntities();
        public ProgramacionController()
        {
            _programacionService = new ProgramacionService(db);
        }

        // GET: Programacion
        public ActionResult Index()
        {
            return View(_programacionService.ObtenerProgramacion());
        }

        public ActionResult Home()
        {
            var programas = _programacionService.ObtenerProgramacionSemanal();
            return View(programas);
        }

        // GET: Programacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programacion programacion = db.Programacion.Find(id);
            if (programacion == null)
            {
                return HttpNotFound();
            }
            return View(programacion);
        }

        // GET: Programacion/Create
        public ActionResult Create()
        {
            ViewBag.ProgramaID = new SelectList(db.ProgramaRadio, "Id", "Nombre");
            return View();
        }

        // POST: Programacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProgramaID,FechaHorario")] Programacion programacion)
        {
            if (ModelState.IsValid)
            {
                db.Programacion.Add(programacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProgramaID = new SelectList(db.ProgramaRadio, "Id", "Nombre", programacion.ProgramaID);
            return View(programacion);
        }

        // GET: Programacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programacion programacion = db.Programacion.Find(id);
            if (programacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramaID = new SelectList(db.ProgramaRadio, "Id", "Nombre", programacion.ProgramaID);
            return View(programacion);
        }

        // POST: Programacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProgramaID,FechaHorario")] Programacion programacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramaID = new SelectList(db.ProgramaRadio, "Id", "Nombre", programacion.ProgramaID);
            return View(programacion);
        }

        // GET: Programacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programacion programacion = db.Programacion.Find(id);
            if (programacion == null)
            {
                return HttpNotFound();
            }
            return View(programacion);
        }

        // POST: Programacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programacion programacion = db.Programacion.Find(id);
            db.Programacion.Remove(programacion);
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
