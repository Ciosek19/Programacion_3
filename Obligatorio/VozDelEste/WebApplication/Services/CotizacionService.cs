using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication.ViewModels.DTO;

namespace WebApplication.Services
{
   public class CotizacionesService
   {
      private readonly string _apiKey = "583f05c5547873c881272fbe27832683";
      private readonly string _url = "http://api.currencylayer.com/live";

      public async Task<List<CotizacionMoneda>> ObtenerHistoricoCotizacionesAsync()
      {
         await ActualizarCotizacionesDesdeApiAsync();

         using (var context = new VozDelEsteBDEntities())
         {
            return context.CotizacionMoneda
               .OrderBy(c => c.Fecha)
               .ToList();
         }
      }

      public CotizacionMoneda ObtenerUltimaCotizacion()
      {
         using (var context = new VozDelEsteBDEntities())
         {
            return context.CotizacionMoneda
                .OrderByDescending(c => c.Fecha)
                .FirstOrDefault();
         }
      }

      public async Task<List<CotizacionMoneda>> ObtenerUltimasCotizacionesAsync()
      {
         await ActualizarCotizacionesDesdeApiAsync();

         using (var context = new VozDelEsteBDEntities())
         {
            return context.CotizacionMoneda
                .GroupBy(c => c.TipoMoneda)
                .Select(g => g.OrderByDescending(c => c.Fecha).FirstOrDefault())
                .ToList();
         }
      }

      private async Task ActualizarCotizacionesDesdeApiAsync()
      {
         var ultima = ObtenerUltimaCotizacion();
         if (ultima == null || (DateTime.Now - ultima.Fecha).TotalHours < 8) return;

         var client = new RestClient(_url);
         var request = new RestRequest();

         request.AddParameter("access_key", _apiKey);
         request.AddParameter("currencies", "USD,ARS,BRL,EUR");
         request.AddParameter("source", "UYU");
         request.AddParameter("format", "1");

         var response = await client.ExecuteAsync(request);
         if (!response.IsSuccessful) return;

         var datos = JsonConvert.DeserializeObject<CurrencyLayerResponse>(response.Content);
         if (datos?.quotes == null || !datos.success) return;

         using (var context = new VozDelEsteBDEntities())
         {
            foreach (var par in datos.quotes)
            {
               string key = par.Key; // Ej: "UYUUSD"
               string tipoMoneda = key.Substring(3); // USD
               decimal valorDesdeUYU = par.Value;

               if (tipoMoneda != "UYU")
               {
                  decimal valorConvertido = 1 / valorDesdeUYU;

                  context.CotizacionMoneda.Add(new CotizacionMoneda
                  {
                     Fecha = DateTime.Now,
                     TipoMoneda = tipoMoneda,
                     ValorCompra = valorConvertido
                  });
               }
            }

            context.SaveChanges();
         }
      }
   }
}
