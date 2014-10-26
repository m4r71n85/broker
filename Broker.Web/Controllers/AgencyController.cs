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
        public ActionResult Index()
        {
            return View(Db.Agencies.ToList());
        }

        public ActionResult List()
        {
            return View(Db.Agencies.ToList());
        }

        // GET: /Agency/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = await Db.Agencies.Where(x => x.Id == (id ?? -1)).FirstOrDefaultAsync();
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
                var agency = new Agency() { Image = agencyVm.Image, Address = agencyVm.Address, Description = agencyVm.Description, Email = agencyVm.Email, HomePhone = agencyVm.HomePhone, MobilePhone = agencyVm.MobilePhone, Name = agencyVm.Name, Participants = new List<ApplicationUser> { ApplicationUser } };
                
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
            Agency agency = await Db.Agencies.Where(x => x.Id == (id ?? -1)).FirstOrDefaultAsync();
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
                Db.Entry(agency).State = EntityState.Modified;
                await Db.SaveChangesAsync();
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
            Agency agency = await Db.Agencies.Where(x => x.Id == (id ?? -1)).FirstOrDefaultAsync();
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
            Agency agency = await Db.Agencies.Where(x => x.Id == id).FirstOrDefaultAsync();
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
            Db.Agencies.Remove(agency);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Join(int id)
        {
            var agency = Db.Agencies.FirstOrDefault(x => x.Id == id);
            var currentUser = Db.Users.FirstOrDefault(x => x.Id == ApplicationUser.Id);

            AgencyCandidacy candidacy = Db.AgencyCandidacies.FirstOrDefault(x => x.Candidator.Id == currentUser.Id);
            if (candidacy == null)
            {
                candidacy = new AgencyCandidacy { Candidator = currentUser, Agency = agency};
            }
            Db.AgencyCandidacies.AddOrUpdate(candidacy);

            var agencyCreator = agency.Participants.FirstOrDefault(x => x.IsAgencyCreator);
            Db.Mails.Add(new Mail
            {
                FromUser = currentUser,
                ToUser = agencyCreator,
                MailType = MailType.ParticipationRequest,
                Title =
                    string.Format("User {0} {1} wants to participate in {2} company.", currentUser.FirstName,
                        currentUser.LastName, agency.Name),
                Body = "For more information go to 'My Agency > Candidates'",
            });
            Db.SaveChanges();

            return null;
        }

        public ActionResult AcceptCandidacy(string id)
        {
            var agency = ApplicationUser.Agency;
            var candidator = Db.Users.FirstOrDefault(x=>x.Id == id);
            var candidacy = Db.AgencyCandidacies.FirstOrDefault(x => x.Agency.Id == agency.Id && x.Candidator.Id == id);
            if (candidacy!=null)
            {
                agency.Participants.Add(candidator);
                Db.AgencyCandidacies.Remove(candidacy);
                Db.SaveChanges();
            }
            return RedirectToAction("Details");
        }

        public ActionResult DeclineCandidacy(string id)
        {
            var agency = ApplicationUser.Agency;
            var candidator = Db.Users.FirstOrDefault(x => x.Id == id);
            var candidacy = Db.AgencyCandidacies.FirstOrDefault(x => x.Agency.Id == agency.Id && x.Candidator.Id == id);
            if (candidacy != null)
            {
                Db.AgencyCandidacies.Remove(candidacy);
                Db.SaveChanges();
            }
            return RedirectToAction("Details");
        }


    }
}
