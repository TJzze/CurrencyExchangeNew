using CurrencyExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CurrencyExchange.Controllers
{
    public class CalculatorController : Controller
    {
        private CurrencyExchangeEntities db = new CurrencyExchangeEntities();
        // GET: Calculator
        public ActionResult Index()
        {
            CurrencyExchange.Models.CalculatorModel model = new CurrencyExchange.Models.CalculatorModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CurrencyExchange.Models.CalculatorModel model)
        {
            if (!model.ReceivedAmount.HasValue && model.ReturnedAmount.HasValue)
            {
                double result = (1 / Calculate(model.ReceivedCur, model.ReturnedCur)) * model.ReturnedAmount.Value;
                model.ReceivedAmount = result;
            } 
            else if (model.ReceivedAmount.HasValue && !model.ReturnedAmount.HasValue)
            {
                double result = Calculate(model.ReceivedCur, model.ReturnedCur) * model.ReceivedAmount.Value;
                model.ReturnedAmount = result;
            }
            else
            {
                ViewBag.Message = string.Format("Please enter only one value!");
                CurrencyExchange.Models.CalculatorModel data = new CurrencyExchange.Models.CalculatorModel();
                return View(data);
            }
            CurrencyExchange.DataAccess.Convertation currConvertation = new CurrencyExchange.DataAccess.Convertation()
            {
                BuyingCurrency = model.ReceivedCur,
                SellingCurency = model.ReturnedCur,
                AmountReceived = model.ReceivedAmount.Value,
                AmountReturned = model.ReturnedAmount.Value,
                ConvertationDate = DateTime.Now,
                ConverationComment = model.Comment
            };
            try
            {
                db.Convertations.Add(currConvertation);
                db.SaveChanges();
            }
            catch
            {
                ViewBag.Message = string.Format("Please enter correct currencies!");
            }
            return View(model);
        }

        private double Calculate(string ReceivedCur, string ReturnedCur)
        {
            var firstRate = (from rate in db.ExchangeRates
                            where rate.Currency == ReceivedCur
                            select rate.BuyingRate).Single();
            var secondRate = (from rate in db.ExchangeRates
                              where rate.Currency == ReturnedCur
                              select rate.SellingRate).Single();
            return firstRate / secondRate;
        }
    }
}