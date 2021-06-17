using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CurrencyExchange.DataAccess;

namespace CurrencyExchange.Controllers
{
    public class ConvertationsController : Controller
    {
        private CurrencyExchangeEntities db = new CurrencyExchangeEntities();

        // GET: Convertations
        [HttpGet]
        public ActionResult Index(CurrencyExchange.Models.viewModel data)
        {
            var convertations = db.Convertations.Include(c => c.Currency).Include(c => c.Currency1);
            var convertationsList = convertations.ToList();
            var searchedConvertations = new List<Convertation>();
            if (data.filterData != null)
            {
                if (data.filterData.inQuestion)
                {
                    searchedConvertations = (from convertation
                                             in convertationsList
                                             where (convertation.ConvertationDate >= data.filterData.fromDate &&
                                             convertation.ConvertationDate <= data.filterData.tillDate) &&
                                             convertation.AmountReceived > 3000
                                             select convertation).ToList();
                }
                else
                {
                    searchedConvertations = (from convertation
                                             in convertationsList
                                             where (convertation.ConvertationDate >= data.filterData.fromDate &&
                                             convertation.ConvertationDate <= data.filterData.tillDate)
                                             select convertation).ToList();
                }
                data.convertationsData = searchedConvertations;
                data.filterData = new Models.ConvertationsModel();
                return View(data);
            }
            else
            {
                CurrencyExchange.Models.viewModel model = new CurrencyExchange.Models.viewModel();
                model.convertationsData = convertationsList;
                model.filterData = new Models.ConvertationsModel();
                return View(model);
            }
        }
    }
}
