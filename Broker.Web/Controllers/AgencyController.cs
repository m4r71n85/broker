using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Broker.Models;
using Broker.Data;

namespace Broker.Web.Controllers
{
    public class AgencyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Agency/
        public async Task<ActionResult> Index()
        {
            return View(await db.Agencies.ToListAsync());
        }

        // GET: /Agency/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = await db.Agencies.Where(x => x.Id == (id ?? -1)).FirstOrDefaultAsync();
            if (agency == null)
            {
                return HttpNotFound();
            }
            return View(agency);
        }

        // GET: /Agency/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Agency/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,Name,Image,Address")] Agency agency)
        {
            if (ModelState.IsValid)
            {
                db.Agencies.Add(agency);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(agency);
        }

        // GET: /Agency/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = await db.Agencies.Where(x => x.Id == (id ?? -1)).FirstOrDefaultAsync();
            if (agency == null)
            {
                return HttpNotFound();
            }
            return View(agency);
        }

        // POST: /Agency/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Name,Image,Address")] Agency agency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agency).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(agency);
        }

        // GET: /Agency/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = await db.Agencies.Where(x => x.Id == (id ?? -1)).FirstOrDefaultAsync();
            if (agency == null)
            {
                return HttpNotFound();
            }
            return View(agency);
        }

        // POST: /Agency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Agency agency = await db.Agencies.Where(x => x.Id == id).FirstOrDefaultAsync();
            db.Agencies.Remove(agency);
            await db.SaveChangesAsync();
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
