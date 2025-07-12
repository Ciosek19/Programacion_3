using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.ViewModels.DTO
{
   public class ClimaForecastRespuestaDTO
   {
      [JsonProperty("list")]
      public List<ItemForecast> Lista { get; set; }

      [JsonProperty("city")]
      public CityInfo Ciudad { get; set; }
   }

   public class ItemForecast
   {
      [JsonProperty("dt")]
      public long Dt { get; set; }

      [JsonProperty("main")]
      public MainData Main { get; set; }

      [JsonProperty("weather")]
      public List<WeatherInfo> Clima { get; set; }

      [JsonProperty("wind")]
      public WindData Viento { get; set; }

      [JsonProperty("dt_txt")]
      public string DtTxt { get; set; }
   }

   public class MainData
   {
      [JsonProperty("temp")]
      public decimal Temperatura { get; set; }

      [JsonProperty("humidity")]
      public int Humedad { get; set; }
   }

   public class WeatherInfo
   {
      [JsonProperty("main")]
      public string Main { get; set; }

      [JsonProperty("description")]
      public string Descripcion{ get; set; }

      [JsonProperty("icon")]
      public string Icono { get; set; }
   }

   public class WindData
   {
      [JsonProperty("speed")]
      public decimal Velocidad { get; set; }
   }

   public class CityInfo
   {
      [JsonProperty("name")]
      public string Nombre { get; set; }
   }
}