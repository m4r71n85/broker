using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Broker.Models;
using Broker.Data;
using Broker.Web.ViewModels;
using Broker.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Broker.Web.Constants;
using ImageResizer;
using System.IO;

namespace Broker.Web.Controllers
{
    public class AgencyController : BaseController
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Agencies.ToList());
        }

        public ActionResult List()
        {
            return View(db.Agencies.ToList());
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
            
            ViewBag.googleQr =  new GoogleQRGenerator.GoogleQr(
                Url.Action("Details", "Agency", new { id = agency.Id }, 
                this.Request.Url.Scheme), "200x200", false);
            return View(agency);
        }

        // GET: /Agency/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.roles = UserManager.GetRoles(userId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Image,Description,Email,HomePhone,MobilePhone,Address")] AgencyViewModel agencyVm,
            HttpPostedFileBase imageFile)
        {
            if (imageFile != null)
            {
                agencyVm.Image = SaveImage.Save(imageFile, ImageType.AgencyLogo);
                ModelState.Remove("Image");
            }
            
            if (ModelState.IsValid)
            {
                var agency = new Agency() { Image = agencyVm.Image, Address = agencyVm.Address, Description = agencyVm.Description, Email = agencyVm.Email, HomePhone = agencyVm.HomePhone, MobilePhone = agencyVm.MobilePhone, Name = agencyVm.Name };
                
                ApplicationUser.Agency = agency;
                ApplicationUser.IsAgencyCreator= true;

                UserManager.AddToRole(ApplicationUser.Id, UserRoles.CompanyCreator);
                UserManager.AddToRole(ApplicationUser.Id, UserRoles.CompanyParticipator);

                return RedirectToAction("Index");
            }

            return View(agencyVm);
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
            var imageFile = agency.Image;
            if (imageFile!=null)
            {
                var imageSizes = ImageSizes.GetSizes(ImageType.AgencyLogo, imageFile);
                foreach (var imageSize in imageSizes)
                {
                    string fullPath = Request.MapPath(imageSize.Path);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }
            db.Agencies.Remove(agency);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Join(int id)
        {
            var agency = db.Agencies.FirstOrDefault(x => x.Id == id);
            var currentUser = db.Users.FirstOrDefault(x => x.Id == ApplicationUser.Id);

            AgencyCandidacy candidacy = db.AgencyCandidacies.FirstOrDefault(x => x.Candidator.Id == currentUser.Id);
            if (candidacy == null)
            {
                candidacy = new AgencyCandidacy { Candidator = currentUser, Agency = agency};
            }
            db.AgencyCandidacies.AddOrUpdate(candidacy);
            
            db.SaveChanges();
            //var creatorUser = agency.Participants.FirstOrDefault(x => x.IsAgencyCreator);


            return null;
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
