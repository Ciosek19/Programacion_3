﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
   public class PermisosController : Controller
   {
      private VozDelEsteBDEntities db = new VozDelEsteBDEntities();


      // GET: Permisos
      [TienePermiso("Gestion Roles")]
      public ActionResult Index()
      {
         return View(db.Permiso.ToList());
      }

      // GET: Permisos/Details/5
      [TienePermiso("Gestion Roles")]
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Permiso permiso = db.Permiso.Find(id);
         if (permiso == null)
         {
            return HttpNotFound();
         }
         return View(permiso);
      }

      // GET: Permisos/Create
      [TienePermiso("Gestion Roles")]
      public ActionResult Create()
      {
         return View();
      }

      // POST: Permisos/Create
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Roles")]
      public ActionResult Create([Bind(Include = "Id,Nombre")] Permiso permiso)
      {
         if (ModelState.IsValid)
         {
            db.Permiso.Add(permiso);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(permiso);
      }

      // GET: Permisos/Edit/5
      [TienePermiso("Gestion Roles")]
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Permiso permiso = db.Permiso.Find(id);
         if (permiso == null)
         {
            return HttpNotFound();
         }
         return View(permiso);
      }

      // POST: Permisos/Edit/5
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Roles")]
      public ActionResult Edit([Bind(Include = "Id,Nombre")] Permiso permiso)
      {
         if (ModelState.IsValid)
         {
            db.Entry(permiso).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(permiso);
      }

      // GET: Permisos/Delete/5
      [TienePermiso("Gestion Roles")]
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Permiso permiso = db.Permiso.Find(id);
         if (permiso == null)
         {
            return HttpNotFound();
         }
         return View(permiso);
      }

      // POST: Permisos/Delete/5
      [TienePermiso("Gestion Roles")]
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Permiso permiso = db.Permiso.Find(id);
         db.Permiso.Remove(permiso);
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
