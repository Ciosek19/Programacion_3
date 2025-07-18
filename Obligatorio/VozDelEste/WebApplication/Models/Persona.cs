//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel.DataAnnotations;
   using WebApplication.ViewModels.Validadores;

   public partial class Persona
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
      public Persona()
      {
         this.Conductor = new HashSet<Conductor>();
         this.Usuario = new HashSet<Usuario>();
      }

      [Key]
      public int Id { get; set; }

      [StringLength(maximumLength: 50, ErrorMessage = "Limite de 50 caracteres")]
      [Required]
      public string Nombre { get; set; }


      [StringLength(maximumLength: 50, ErrorMessage = "Limite de 50 caracteres")]
      [Required]
      public string Apellido { get; set; }

      [StringLength(maximumLength: 500, ErrorMessage = "Limite de 50 caracteres")]
      public string ImagenUrl { get; set; }

      [Required]
      [DataType(DataType.Date)]
      [CustomValidation(typeof(FechaValidador), nameof(FechaValidador.ValidarRango))]
      public DateTime FechaNacimiento { get; set; }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
      public virtual ICollection<Conductor> Conductor { get; set; }
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
      public virtual ICollection<Usuario> Usuario { get; set; }
   }
}
