using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.ViewModels.DTO
{
   public class UsuarioSesion
   {
      public int Id { get; set; }
      public string NombreUsuario { get; set; }
      public string NombreCompleto { get; set; }
      public List<string> Permisos { get; set; } = new List<string>();
   }
}