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

namespace WebApplication.Controllers
{
    public class UsuariosController : Controller
    {
        private VozDelEsteBDEntities db = new VozDelEsteBDEntities();

        // GET: Usuarios
        public ActionResult Index()
        {
            var usuario = db.Usuario.Include(u => u.Persona);
            return View(usuario.ToList());
        }

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


        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.PersonaID = new SelectList(db.Persona, "Id", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, volvemos a cargar la lista de roles y mostramos la vista
                model.RolesDisponibles = db.Rol.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToList();

                return View(model);
            }

            var usuario = db.Usuario.Include(u => u.Rol).FirstOrDefault(u => u.Id == model.OUsuario.Id);

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
            .Include(u => u.Rol)
            .FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

            if (usuario != null)
            {
                // Crea la cookie de autenticación
                FormsAuthentication.SetAuthCookie(usuario.NombreUsuario, false);

                // Guarda el usuario en la sesión
                Session["UsuarioActual"] = usuario;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
            return View();
        }

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
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

                var usuario = new Usuario()
                {
                    PersonaID = persona.Id,
                    NombreUsuario = registro.UsuarioVM.NombreUsuario,
                    Email = registro.UsuarioVM.Email,
                    Clave = registro.UsuarioVM.Clave,
                };
                if (db.Usuario.Find(registro.UsuarioVM.NombreUsuario) == null)
                {
                    db.Usuario.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Login");
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
