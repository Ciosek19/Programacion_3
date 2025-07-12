using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebApplication.Models;
using WebApplication.ViewModels.DTO;

namespace WebApplication.Services
{
   public class ProgramacionService
   {
      private readonly VozDelEsteBDEntities _contexto;

      public ProgramacionService(VozDelEsteBDEntities contexto)
      {
         _contexto = contexto;
      }
      public List<Programacion> ObtenerProgramacion()
      {
         return _contexto.Programacion.ToList();
      }
      public ProgramaDTO ProgramaEnVivo()
      {
         var ahora = DateTime.Now;

         // Suponiendo que _programasDelDia ya contiene la programación del día
         var enVivo = ObtenerProgramasDelDia()
             .FirstOrDefault(p => ahora >= p.FechaHorario && ahora < p.FechaHorario.Add(p.Duracion));

         return enVivo;
      }

      public List<ProgramaDTO> ObtenerProgramasDelDia()
      {
         var hoy = DateTime.Today;
         var mañana = hoy.AddDays(1);

         var programas = _contexto.Programacion
 .Where(p => p.FechaHorario >= hoy && p.FechaHorario < mañana)
 .OrderBy(p => p.FechaHorario)
 .Select(p => new ProgramaDTO
 {
    ProgramacionId = p.Id,
    Duracion = p.ProgramaRadio.Duracion,
    FechaHorario = p.FechaHorario,
    Nombre = p.ProgramaRadio.Nombre,
    Descripcion = p.ProgramaRadio.Descripcion,
    ImagenUrl = p.ProgramaRadio.ImagenUrl,
    NombresConductores = p.ProgramaRadio.Conductor
         .Select(c => c.Persona.Nombre + " " + c.Persona.Apellido)
         .ToList()
 })
 .ToList();

         return programas;
      }

      public List<ProgramaDTO> ObtenerProgramacionSemanal()
      {
         var ahora = DateTime.Now;
         var inicioSemana = ahora.Date.AddDays(-(int)ahora.DayOfWeek); // Domingo
         var finSemana = inicioSemana.AddDays(7);                      // Próximo domingo

         var programas = _contexto.Programacion
             .Where(p => p.FechaHorario >= inicioSemana && p.FechaHorario < finSemana)
             .OrderBy(p => p.FechaHorario)
             .Select(p => new ProgramaDTO
             {
                ProgramacionId = p.Id,
                FechaHorario = p.FechaHorario,
                Nombre = p.ProgramaRadio.Nombre,
                Descripcion = p.ProgramaRadio.Descripcion,
                ImagenUrl = p.ProgramaRadio.ImagenUrl,
                Duracion = p.ProgramaRadio.Duracion,
                NombresConductores = p.ProgramaRadio.Conductor
                 .Select(c => c.Persona.Nombre + " " + c.Persona.Apellido)
                 .ToList()

             }).ToList();

         return programas;
      }


   }
}