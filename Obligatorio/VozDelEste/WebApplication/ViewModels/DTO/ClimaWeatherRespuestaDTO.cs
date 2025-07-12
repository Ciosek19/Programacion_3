using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.ViewModels.DTO
{
   public class ClimaWeatherRespuestaDTO
   {
      [JsonProperty("weather")]
      public List<Weather> Clima { get; set; }

      [JsonProperty("main")]
      public Main Main { get; set; }

      [JsonProperty("wind")]
      public Wind Viento { get; set; }

      [JsonProperty("dt")]
      public long Dt { get; set; }  // Unix timestamp

      // Puedes agregar más campos si los necesitás
   }

   public class Weather
   {
      [JsonProperty("description")]
      public string Descripcion { get; set; }

      [JsonProperty("icon")]
      public string Icono { get; set; }
   }

   public class Main
   {
      [JsonProperty("temp")]
      public float Temperatura { get; set; }

      [JsonProperty("humidity")]
      public int Humedad { get; set; }
   }

   public class Wind
   {
      [JsonProperty("speed")]
      public float Velocidad { get; set; }
   }
}