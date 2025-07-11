using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.ViewModels.DTO
{
    public class ProgramaDTO
    {
        public int ProgramacionId { get; set; }
        public DateTime FechaHorario { get; set; }
        public TimeSpan Duracion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
        public bool EstaEnVivo
        {
            get
            {
                var ahora = DateTime.Now;
                return ahora >= FechaHorario && ahora <= FechaHorario.Add(Duracion);
            }
        }

        public List<string> NombresConductores { get; set; } = new List<string>();
    }

}