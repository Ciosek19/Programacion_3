using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.ViewModels.Validadores
{
   public class FechaValidador
   {
      public static ValidationResult ValidarRango(DateTime fecha, ValidationContext context)
      {
         var fechaMin = new DateTime(1900, 1, 1);
         var fechaMax = DateTime.Now;

         if (fecha < fechaMin || fecha > fechaMax)
         {
            return new ValidationResult("La fecha debe estar entre 1900 y 2025.");
         }

         return ValidationResult.Success;
      }
   }
}