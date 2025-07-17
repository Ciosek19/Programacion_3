using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;
using WebApplication.ViewModels;
using WebApplication.ViewModels.DTO;

namespace WebApplication.Services
{
   public class ClimaService
   {
      private readonly string _apiKey = "f939b347c0f7440d1c1fe1c4d192e806";
      private readonly string _urlPronostico = "https://api.openweathermap.org/data/2.5/forecast";
      private readonly string _urlActual = "https://api.openweathermap.org/data/2.5/weather";
      private readonly int _weatherMinutosActualizacion = 2;
      private readonly int _forecastMinutosActualizacion = 2;

      // --- Peticion API Weather y guardar en BD ---
      private async Task ActualizarClimaActual()
      {
         using (var context = new VozDelEsteBDEntities())
         {
            var ultimo = context.Clima
               .Where(c => c.Fecha < DateTime.Now)
                .OrderByDescending(c => c.Fecha)
                .FirstOrDefault();

            if (ultimo != null && (DateTime.Now - ultimo.Fecha).TotalMinutes < _weatherMinutosActualizacion)
               return; // Verificar si pasaron los minutos de la ultima peticion

            var client = new RestClient(_urlActual);
            var request = new RestRequest();
            request.AddParameter("q", "Maldonado,UY");
            request.AddParameter("appid", _apiKey);
            request.AddParameter("units", "metric");
            request.AddParameter("lang", "es");

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful) return;

            var data = JsonConvert.DeserializeObject<ClimaWeatherRespuestaDTO>(response.Content);
            if (data == null) return;

            var ahora = DateTime.Now;

            var nuevo = new Clima
            {
               Fecha = ahora,
               Temperatura = (decimal)data.Main.Temperatura,
               Humedad = data.Main.Humedad,
               Viento = (int)data.Viento.Velocidad,
               Condicion = data.Clima.FirstOrDefault()?.Descripcion ?? "",
               Icono = data.Clima.FirstOrDefault()?.Icono ?? ""
            };

            context.Clima.Add(nuevo);
            context.SaveChanges();
         }
      }

      // --- Peticion API Forecast y guardar en BD sin filtrar horas ---
      private async Task ActualizarPronosticoAsync()
      {
         using (var context = new VozDelEsteBDEntities())
         {
            var ahora = DateTime.Now;

            // Contar cuántos pronósticos futuros hay desde ahora
            var pronosticosFuturos = context.Clima
                .Where(c => c.Fecha > ahora)
                .Count();

            if (pronosticosFuturos >= 40)
               return; // Ya hay suficientes, no se actualiza

            // Hacer la petición si hay menos de 40
            var client = new RestClient(_urlPronostico);
            var request = new RestRequest();
            request.AddParameter("q", "Maldonado,UY");
            request.AddParameter("appid", _apiKey);
            request.AddParameter("units", "metric");
            request.AddParameter("lang", "es");

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful) return;

            var forecast = JsonConvert.DeserializeObject<ClimaForecastRespuestaDTO>(response.Content);
            if (forecast?.Lista == null || !forecast.Lista.Any()) return;

            foreach (var item in forecast.Lista)
            {
               var fecha = DateTime.Parse(item.DtTxt).ToLocalTime();

               var existente = context.Clima.FirstOrDefault(c => c.Fecha == fecha);
               if (existente != null)
               {
                  existente.Temperatura = item.Main.Temperatura;
                  existente.Humedad = item.Main.Humedad;
                  existente.Viento = (int)item.Viento.Velocidad;
                  existente.Condicion = item.Clima.FirstOrDefault()?.Descripcion ?? "";
                  existente.Icono = item.Clima.FirstOrDefault()?.Icono ?? "";
               }
               else
               {
                  context.Clima.Add(new Clima
                  {
                     Fecha = fecha,
                     Temperatura = item.Main.Temperatura,
                     Humedad = item.Main.Humedad,
                     Viento = (int)item.Viento.Velocidad,
                     Condicion = item.Clima.FirstOrDefault()?.Descripcion ?? "",
                     Icono = item.Clima.FirstOrDefault()?.Icono ?? ""
                  });
               }
            }

            context.SaveChanges();
         }
      }


      public async Task<ClimaPronosticoViewModel> ObtenerPronosticoCompletoAsync()
      {
         await ActualizarClimaActual();
         await ActualizarPronosticoAsync();

         using (var context = new VozDelEsteBDEntities())
         {

            var climaActual = context.Clima
                  .Where(c => c.Fecha <= DateTime.Now)
                  .OrderByDescending(c => c.Fecha)
                  .FirstOrDefault();


            if (climaActual == null)
               return null;

            // Obtener todos los registros posteriores
            var posteriores = context.Clima
                .Where(c => c.Fecha > climaActual.Fecha)
                .OrderBy(c => c.Fecha)
                .ToList();

            int cantidadPosteriores = posteriores.Count;

            // Obtener la misma cantidad de registros anteriores
            var anteriores = context.Clima
                .Where(c => c.Fecha < climaActual.Fecha)
                .OrderByDescending(c => c.Fecha)
                .Take(40)
                .OrderBy(c => c.Fecha) // Para mantener orden cronológico
                .ToList();

            return new ClimaPronosticoViewModel
            {
               ClimaActual = climaActual,
               Anteriores = anteriores,
               Posteriores = posteriores
            };
         }
      }

   }
}