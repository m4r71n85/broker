using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Broker.Models;
using Broker.Data;

namespace Broker.Web.Controllers
{
    public class OfferController : BaseController
    {

        // GET: /Offer/
        public ActionResult Index()
        {
            return View(Db.Offers.ToList());
        }

        // GET: /Offer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = Db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: /Offer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Offer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AmontOfBedrooms,Price,Currency,Address,Area,Client")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                Db.Offers.Add(offer);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offer);
        }

        // GET: /Offer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = Db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: /Offer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,AmontOfBedrooms,Price,Currency,Address,Area")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(offer).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offer);
        }

        // GET: /Offer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = Db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: /Offer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offer offer = Db.Offers.Find(id);
            Db.Offers.Remove(offer);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
