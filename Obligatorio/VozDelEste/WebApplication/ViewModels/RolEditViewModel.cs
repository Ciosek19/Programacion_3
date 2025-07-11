using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.ViewModels
{
    public class RolEditViewModel
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }

        public List<PermisoCheckboxViewModel> Permisos { get; set; }
    }
}