using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;
using WebApplication.ViewModels.DTO;

namespace WebApplication.Services
{
   public class CotizacionesService
   {
      private readonly string _apiKey = "583f05c5547873c881272fbe27832683";
      private readonly string _url = "http://api.currencylayer.com/live";

      public async Task GuardarCotizacionesAsync()
      {
         var client = new RestClient(_url);
         var request = new RestRequest();

         request.Method = Method.Get;

         request.AddParameter("access_key", _apiKey);
         request.AddParameter("currencies", "USD,ARS,BRL,EUR");
         request.AddParameter("source", "UYU");
         request.AddParameter("format", "1");

         var response = await client.ExecuteAsync(request);

         if (!response.IsSuccessful)
            return;//Es preferoble retornar unb mensaje de error

         var datos = JsonConvert.DeserializeObject<CurrencyLayerResponse>(response.Content);

         if (datos?.quotes == null || !datos.success)
            return;//Es preferoble retornar unb mensaje de error

         DateTime fechaHoy = DateTime.Today;

         using (var context = new VozDelEsteBDEntities())
         {

            foreach (var par in datos.quotes)
            {
               string key = par.Key; // Ejemplo: "UYUUSD", "UYUARS", etc.
               string tipoMoneda = key.Substring(3); // USD, ARS, etc.
               decimal valorDesdeUYU = par.Value;

               if (tipoMoneda != "UYU") // ignoramos la base
               {
                  // Convertimos: cuánto vale 1 unidad de esa moneda en pesos uruguayos
                  // Ejemplo: 1 USD = 1 / 0.024834 ≈ 40.25 UYU
                  decimal valorConvertido = 1 / valorDesdeUYU;

                  var existente = context.CotizacionMoneda
                      .FirstOrDefault(c => c.Fecha == fechaHoy && c.TipoMoneda == tipoMoneda);

                  if (existente == null)
                  {
                     context.CotizacionMoneda.Add(new CotizacionMoneda
                     {
                        Fecha = fechaHoy,
                        TipoMoneda = tipoMoneda,
                        ValorCompra = valorConvertido,
                     });
                  }
                  else
                  {
                     existente.ValorCompra = valorConvertido;
                  }
               }
            }


            context.SaveChanges();
         }
      }
   }
}