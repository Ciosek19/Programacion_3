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

   public partial class ProgramaRadio
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
      public ProgramaRadio()
      {
         this.ComentarioPrograma = new HashSet<ComentarioPrograma>();
         this.Programacion = new HashSet<Programacion>();
         this.Conductor = new HashSet<Conductor>();
      }
      [Key]
      public int Id { get; set; }

      [StringLength(maximumLength: 50, ErrorMessage = "Limite de 50 caracteres")]
      [Required]
      public string Nombre { get; set; }

      [StringLength(maximumLength: 500, ErrorMessage = "Limite de 500 caracteres")]
      public string ImagenUrl { get; set; }

      [StringLength(maximumLength: int.MaxValue, ErrorMessage = "Limite de int.MaxValue caracteres")]
      public string Descripcion { get; set; }

      public System.TimeSpan Duracion { get; set; }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
      public virtual ICollection<ComentarioPrograma> ComentarioPrograma { get; set; }
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
      public virtual ICollection<Programacion> Programacion { get; set; }
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
      public virtual ICollection<Conductor> Conductor { get; set; }
   }
}
