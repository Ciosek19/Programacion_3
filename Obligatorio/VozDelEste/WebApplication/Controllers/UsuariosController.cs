using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication.Models;
using WebApplication.ViewModels;
using WebApplication.ViewModels.DTO;

namespace WebApplication.Controllers
{
   public class UsuariosController : Controller
   {
      private VozDelEsteBDEntities db = new VozDelEsteBDEntities();

      // GET: Usuarios
      [TienePermiso("Gestion Usuarios")]
      public ActionResult Index()
      {
         var usuario = db.Usuario.Include(u => u.Persona);
         return View(usuario.ToList());
      }

      [TienePermiso("Gestion Usuarios")]
      // GET: Usuarios/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         var usuario = db.Usuario
             .Include(u => u.Rol) // <-- importante
             .Include(u => u.Persona)
             .FirstOrDefault(u => u.Id == id);

         if (usuario == null)
         {
            return HttpNotFound();
         }

         return View(usuario);
      }

      [TienePermiso("Gestion Usuarios")]
      // GET: Usuarios/Create
      public ActionResult Create()
      {
         ViewBag.PersonaID = new SelectList(db.Persona, "Id", "Nombre");
         return View();
      }

      // POST: Usuarios/Create
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [TienePermiso("Gestion Usuarios")]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "Id,PersonaID,NombreUsuario,Email,Clave,Silenciado")] Usuario usuario)
      {
         if (ModelState.IsValid)
         {
            db.Usuario.Add(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.PersonaID = new SelectList(db.Persona, "Id", "Nombre", usuario.PersonaID);
         return View(usuario);
      }

      // GET: Usuarios/Edit/5
      [TienePermiso("Gestion Usuarios")]
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         var usuario = db.Usuario.Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
         if (usuario == null)
         {
            return HttpNotFound();
         }

         var viewModel = new UsuarioEditViewModel
         {
            OUsuario = new Usuario
            {
               Id = usuario.Id,
               NombreUsuario = usuario.NombreUsuario,
               Email = usuario.Email
            },
            RolIdsSeleccionados = usuario.Rol.Select(r => r.Id).ToList(),
            RolesDisponibles = db.Rol.Select(r => new SelectListItem
            {
               Value = r.Id.ToString(),
               Text = r.Nombre
            }).ToList()
         };

         return View(viewModel);
      }

      // POST: Usuarios/Edit/5
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
      // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [TienePermiso("Gestion Usuarios")]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(UsuarioEditViewModel model)
      {
         var usuario = db.Usuario
                .Include(u => u.Rol)
                .Include(u => u.Persona)
                .FirstOrDefault(u => u.Id == model.OUsuario.Id);

         model.OUsuario = usuario;

         if (usuario == null)

         {
            // Si el usuario editado no existe, volvemos a cargar la lista de roles y mostramos la vista
            model.RolesDisponibles = db.Rol.Select(r => new SelectListItem
            {
               Value = r.Id.ToString(),
               Text = r.Nombre
            }).ToList();

            return View(model);
         }

         
         if (usuario == null)
         {
            return HttpNotFound();
         }

         usuario.NombreUsuario = model.OUsuario.NombreUsuario;
         usuario.Email = model.OUsuario.Email;

         usuario.Rol.Clear();

         if (model.RolIdsSeleccionados != null)
         {
            foreach (var rolId in model.RolIdsSeleccionados)
            {
               var rol = db.Rol.Find(rolId);
               if (rol != null)
               {
                  usuario.Rol.Add(rol);
               }
            }
         }

         db.SaveChanges();

         return RedirectToAction("Index");
      }

      // GET: Usuarios/Delete/5
      [TienePermiso("Gestion Usuarios")]
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Usuario usuario = db.Usuario.Find(id);
         if (usuario == null)
         {
            return HttpNotFound();
         }
         return View(usuario);
      }

      // POST: Usuarios/Delete/5
      [TienePermiso("Gestion Usuarios")]
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Usuario usuario = db.Usuario.Find(id);
         db.Usuario.Remove(usuario);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      [HttpGet]
      public ActionResult Login()
      {
         return View();
      }

      [HttpPost]
      public ActionResult Login(string nombreUsuario, string clave)
      {
         var usuario = db.Usuario
             .Include(u => u.Persona)
             .Include(u => u.Rol.Select(r => r.Permiso)) // asegúrate de incluir permisos
             .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Clave == clave);

         if (usuario != null)
         {
            // Armar DTO desconectado
            var usuarioSesion = new UsuarioSesion
            {
               Id = usuario.Id,
               NombreUsuario = usuario.NombreUsuario,
               NombreCompleto = usuario.Persona?.Nombre + " " + usuario.Persona?.Apellido,
               Permisos = usuario.Rol
                    .SelectMany(r => r.Permiso)
                    .Select(p => p.Nombre)
                    .Distinct()
                    .ToList()
            };

            // Crea la cookie de autenticación
            FormsAuthentication.SetAuthCookie(usuarioSesion.NombreUsuario, false);

            // Guarda el DTO en sesión
            Session["UsuarioActual"] = usuarioSesion;

            return RedirectToAction("Index", "Home");
         }

         ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
         return View();
      }


      [HttpGet]
      public ActionResult Registro()
      {
         var model = new RegistroViewModel
         {
            PersonaVM = new Persona(),
            UsuarioVM = new Usuario()
         };
         return View(model);
      }

      [HttpPost]
      public ActionResult Registro(RegistroViewModel registro)
      {
         if (ModelState.IsValid)
         {
            var persona = new Persona()
            {
               Nombre = registro.PersonaVM.Nombre,
               Apellido = registro.PersonaVM.Apellido,
               ImagenUrl = "",
               FechaNacimiento = registro.PersonaVM.FechaNacimiento,
            };
            db.Persona.Add(persona);
            db.SaveChanges();

            var rolPorDefecto = db.Rol.FirstOrDefault(r => r.Nombre == "Oyente");

            var usuario = new Usuario()
            {
               PersonaID = persona.Id,
               NombreUsuario = registro.UsuarioVM.NombreUsuario,
               Email = registro.UsuarioVM.Email,
               Clave = registro.UsuarioVM.Clave,
            };

            // Asignar el rol si se encontró
            if (rolPorDefecto != null)
            {
               usuario.Rol.Add(rolPorDefecto);
            }

            bool existeUsuario = db.Usuario.Any(u => u.NombreUsuario == registro.UsuarioVM.NombreUsuario);
            bool existeEmail = db.Usuario.Any(u => u.Email == registro.UsuarioVM.Email);

            if (!existeUsuario)
            {
               if (!existeEmail)
               {
                  db.Usuario.Add(usuario);
                  db.SaveChanges();
                  return RedirectToAction("Login");
               }
               else
               {
                  ModelState.AddModelError("UsuarioVM.Email", "El email ya está en uso.");
               }
            }
            else
            {
               ModelState.AddModelError("UsuarioVM.NombreUsuario", "El nombre de usuario ya está en uso.");
            }
         }

         return View(registro);
      }


      public ActionResult Logout()
      {
         FormsAuthentication.SignOut();
         Session.Clear();
         return RedirectToAction("Login");
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
