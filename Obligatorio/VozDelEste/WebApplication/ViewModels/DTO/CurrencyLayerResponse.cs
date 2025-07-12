using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.ViewModels.DTO
{
   public class CurrencyLayerResponse
   {
      public bool success { get; set; }
      public long timestamp { get; set; }
      public string source { get; set; }
      public Dictionary<string, decimal> quotes { get; set; }
   }
}