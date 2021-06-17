using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyExchange.Models
{
    public class viewModel
    {
        public IEnumerable<CurrencyExchange.DataAccess.Convertation> convertationsData { get; set; }
        public CurrencyExchange.Models.ConvertationsModel filterData { get; set; }
    }
}