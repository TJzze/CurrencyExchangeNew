using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CurrencyExchange.Models
{
    public class CalculatorModel
    {
        public string ReceivedCur { get; set; }
        public string ReturnedCur { get; set; }
        public double? ReceivedAmount { get; set; }
        public double? ReturnedAmount { get; set; }
        public string Comment { get; set; }
    }
}