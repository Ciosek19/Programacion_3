using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
   public class RolesController : Controller
   {
      private VozDelEsteBDEntities db = new VozDelEsteBDEntities();

      // GET: Roles
      [TienePermiso("Gestion Roles")]
      public ActionResult Index()
      {
         return View(db.Rol.ToList());
      }

      // GET: Roles/Details/5
      [TienePermiso("Gestion Roles")]
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         var rol = db.Rol.Include("Permiso").FirstOrDefault(r => r.Id == id);
         if (rol == null) return HttpNotFound();

         var viewModel = new RolEditViewModel
         {
            IdRol = rol.Id,
            Nombre = rol.Nombre,
            Permisos = rol.Permiso.Select(p => new PermisoCheckboxViewModel
            {
               IdPermiso = p.Id,
               Nombre = p.Nombre,
               Seleccionado = true // siempre true en Details, porque solo se muestran los asignados
            }).ToList()
         };

         return View(viewModel);
      }

      // GET: Roles/Create
      [TienePermiso("Gestion Roles")]
      public ActionResult Create()
      {
         var todosLosPermisos = db.Permiso.ToList();

         var viewModel = new RolEditViewModel
         {
            Permisos = todosLosPermisos.Select(p => new PermisoCheckboxViewModel
            {
               IdPermiso = p.Id,
               Nombre = p.Nombre,
               Seleccionado = false
            }).ToList()
         };

         return View(viewModel);
      }

      // POST: Roles/Create
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Roles")]
      public ActionResult Create(RolEditViewModel model)
      {
         if (!ModelState.IsValid)
         {
            // Vuelve a cargar permisos por si hay errores
            model.Permisos = db.Permiso.ToList().Select(p => new PermisoCheckboxViewModel
            {
               IdPermiso = p.Id,
               Nombre = p.Nombre,
               Seleccionado = model.Permisos.Any(mp => mp.IdPermiso == p.Id && mp.Seleccionado)
            }).ToList();

            return View(model);
         }

         var nuevoRol = new Rol
         {
            Nombre = model.Nombre,
            Permiso = new List<Permiso>()
         };

         foreach (var permisoVm in model.Permisos)
         {
            if (permisoVm.Seleccionado)
            {
               var permiso = db.Permiso.Find(permisoVm.IdPermiso);
               if (permiso != null)
               {
                  nuevoRol.Permiso.Add(permiso);
               }
            }
         }

         db.Rol.Add(nuevoRol);
         db.SaveChanges();

         return RedirectToAction("Index");
      }


      // GET: Roles/Edit/5
      [TienePermiso("Gestion Roles")]
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         var rol = db.Rol.Include("Permiso").FirstOrDefault(r => r.Id == id);
         if (rol == null) return HttpNotFound();

         var todosLosPermisos = db.Permiso.ToList();

         var viewModel = new RolEditViewModel
         {
            IdRol = rol.Id,
            Nombre = rol.Nombre,
            Permisos = todosLosPermisos.Select(p => new PermisoCheckboxViewModel
            {
               IdPermiso = p.Id,
               Nombre = p.Nombre,
               Seleccionado = rol.Permiso.Any(rp => rp.Id == p.Id)
            }).ToList()
         };

         return View(viewModel);
      }

      // POST: Roles/Edit/5
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Roles")]
      public ActionResult Edit(RolEditViewModel model)
      {
         if (!ModelState.IsValid)
         {
            return View(model);
         }

         var rol = db.Rol.Include("Permisos").FirstOrDefault(r => r.Id == model.IdRol);
         if (rol == null) return HttpNotFound();

         rol.Nombre = model.Nombre;

         // Actualizar permisos
         rol.Permiso.Clear();

         foreach (var permisoVm in model.Permisos)
         {
            if (permisoVm.Seleccionado)
            {
               var permiso = db.Permiso.Find(permisoVm.IdPermiso);
               if (permiso != null)
               {
                  rol.Permiso.Add(permiso);
               }
            }
         }

         db.SaveChanges();

         return RedirectToAction("Index");
      }

      // GET: Roles/Delete/5
      [TienePermiso("Gestion Roles")]
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Rol rol = db.Rol.Find(id);
         if (rol == null)
         {
            return HttpNotFound();
         }
         return View(rol);
      }

      // POST: Roles/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      [TienePermiso("Gestion Roles")]
      public ActionResult DeleteConfirmed(int id)
      {
         Rol rol = db.Rol.Find(id);
         db.Rol.Remove(rol);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      [TienePermiso("Gestion Roles")]
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
