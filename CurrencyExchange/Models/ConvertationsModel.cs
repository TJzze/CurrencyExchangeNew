using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyExchange.Models
{
    public class ConvertationsModel
    {
        public bool inQuestion { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime tillDate { get; set; }
    }
}