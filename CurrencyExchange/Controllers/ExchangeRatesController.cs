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
    public class ExchangeRatesController : Controller
    {
        private CurrencyExchangeEntities db = new CurrencyExchangeEntities();

        // GET: ExchangeRates
        public ActionResult Index()
        {
            var exchangeRates = db.ExchangeRates.Include(e => e.Currency1);
            return View(exchangeRates.ToList());
        }

        // GET: ExchangeRates/Create
        public ActionResult Create()
        {
            ViewBag.Currency = new SelectList(db.Currencies, "Currency1", "Currency1");
            return View();
        }

        // POST: ExchangeRates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RateID,Currency,BuyingRate,SellingRate")] ExchangeRate exchangeRate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ExchangeRates.Add(exchangeRate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = string.Format("Enter valid currency!");
                }
            }
            catch 
            {
                ViewBag.Message = string.Format("Please enter valid information!");
                return View();
            }
            return View(exchangeRate);
        }

        // GET: ExchangeRates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExchangeRate exchangeRate = db.ExchangeRates.Find(id);
            if (exchangeRate == null)
            {
                return HttpNotFound();
            }
            ViewBag.Currency = new SelectList(db.Currencies, "Currency1", "Currency1", exchangeRate.Currency);
            return View(exchangeRate);
        }

        // POST: ExchangeRates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RateID,Currency,BuyingRate,SellingRate")] ExchangeRate exchangeRate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(exchangeRate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = string.Format("Enter valid input!");
                }
            }
            catch
            {
                ViewBag.Message = string.Format("Error, please try again!");
            }
            return View(exchangeRate);
        }

        // GET: ExchangeRates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExchangeRate exchangeRate = db.ExchangeRates.Find(id);
            if (exchangeRate == null)
            {
                return HttpNotFound();
            }
            return View(exchangeRate);
        }

        // POST: ExchangeRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ExchangeRate exchangeRate = db.ExchangeRates.Find(id);
                db.ExchangeRates.Remove(exchangeRate);
                db.SaveChanges();
            }
            catch
            {
                ViewBag.Message = string.Format("Error, please try again!");
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
