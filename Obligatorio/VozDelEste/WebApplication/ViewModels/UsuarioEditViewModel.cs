using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class UsuarioEditViewModel
    {
        public Usuario OUsuario { get; set; }
        public List<int> RolIdsSeleccionados { get; set; }
        public List<SelectListItem> RolesDisponibles { get; set; }
    }
}