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
    public class CurrenciesController : Controller
    {
        private CurrencyExchangeEntities db = new CurrencyExchangeEntities();

        // GET: Currencies
        public ActionResult Index()
        {
            var enteredCurrencies = from currency
                                    in db.Currencies
                                    select currency;
            var orderedCurrencies = enteredCurrencies.OrderBy(curr => curr.OrderNum == null).ThenBy(curr => curr.OrderNum);
            var currenciesView = orderedCurrencies.ToList();
            
            return View(currenciesView);
        }

        // GET: Currencies/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Currency1,CurrencyName,CurrencyNameLatin,OrderNum")] Currency currency)
        {
            try
            {
                var orderNumbers =  from curr
                                    in db.Currencies
                                    select curr.OrderNum;
                orderNumbers = orderNumbers.OrderByDescending(curr => curr).ThenBy(curr => curr == null);
                var orders = orderNumbers.ToList();
                int? highestNumber = null;
                if (orders.Any()) {
                    highestNumber = (int?)orders.First();
                }
                if (highestNumber == null)
                {
                    highestNumber = 0;
                }
                int? orderNum = currency.OrderNum;

                if (ModelState.IsValid)
                {
                    if (orderNum != null && (orderNum - highestNumber > 1 || orders.Contains(orderNum) || orderNum < 1))
                    {
                        ViewBag.Message = string.Format("Please enter valid order!");
                    } 
                    else
                    {
                        db.Currencies.Add(currency);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch
            {
                ViewBag.Message = string.Format("Please enter valid information!");
                return View();
            }

            return View(currency);
        }

        // GET: Currencies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Currency1,CurrencyName,CurrencyNameLatin,OrderNum")] Currency currency)
        {
            try
            {
                var orderNumbers = from curr
                                   in db.Currencies
                                   select curr.OrderNum;
                orderNumbers = orderNumbers.OrderByDescending(curr => curr).ThenBy(curr => curr == null);
                var orders = orderNumbers.ToList();
                int? highestNumber = (int?)orders.First();
                if (highestNumber == null)
                {
                    highestNumber = 0;
                }
                int? orderNum = currency.OrderNum;

                var currentCurrency = from curr
                                              in db.Currencies
                                      where (currency.Currency1 == curr.Currency1)
                                      select curr.OrderNum;
                int? current = currentCurrency.ToList().First();


                if (ModelState.IsValid)
                {
                    if (orderNum != null && (orderNum - highestNumber > 1 || (orders.Contains(orderNum) && orderNum != current)  || orderNum < 1))
                    {
                        ViewBag.Message = string.Format("Please enter valid order!");
                    } 
                    else
                    {
                        if (current != null && orderNum != current)
                        {
                            var currencies = from curr
                                             in db.Currencies
                                             where (curr.OrderNum > current
                                             && curr.OrderNum != null)
                                             select curr;
                            var currencieslist = currencies.ToList();
                            foreach (var curr in currencieslist)
                            {
                                curr.OrderNum = curr.OrderNum - 1;
                            }
                            currency.OrderNum = currency.OrderNum - 1;
                        }

                        db.Entry(currency).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch
            {
                ViewBag.Message = string.Format("Please enter valid inputs");
            }
            return View(currency);
        }

        // GET: Currencies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Currency currency = db.Currencies.Find(id);
                int? orderNum = currency.OrderNum;

                var currencies = from curr
                                 in db.Currencies
                                 where (curr.OrderNum > orderNum 
                                 && curr.OrderNum != null)
                                 select curr;
                var currencieslist = currencies.ToList();
                foreach (var curr in currencieslist)
                {
                    curr.OrderNum = curr.OrderNum - 1;
                }
                db.Currencies.Remove(currency);
                db.SaveChanges();
            } 
            catch
            {
                ViewBag.Message = string.Format("Action has failed! Please try again!");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
