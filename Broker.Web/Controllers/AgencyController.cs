using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Broker.Models;
using Broker.Web.ViewModels;
using Broker.Web.Helpers;
using Microsoft.AspNet.Identity;
using Broker.Web.Constants;

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
            Agency agency = await Db.Agencies.Where(x => x.Id == (id ?? -1)).FirstOrDefaultAsync();
            if (agency == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.googleQr =  new GoogleQRGenerator.GoogleQr(
                Url.Action("Details", "Agency", new { id = agency.Id }, 
                Request.Url.Scheme), "200x200", false);
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

                var message = "Agency created successfully.";
                Alerts.Add(TempData, new AlertProps(message));
                return RedirectToAction("List");
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

                var message = "Changes saved successfully.";
                Alerts.Add(TempData, new AlertProps(message));
                return RedirectToAction("Index");
            }
            return View(agency);
        }

        // GET: /Agency/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                var message = "Delete can't be null.";
                Alerts.Add(TempData, new AlertProps(AlertType.Danger, message));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = await Db.Agencies.Where(x => x.Id == ((int) id)).FirstOrDefaultAsync();
            if (agency == null)
            {
                var message = "Can't find agency.";
                Alerts.Add(TempData, new AlertProps(AlertType.Danger, message));
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

            var message = "Agency deleted successfully.";
            Alerts.Add(TempData, new AlertProps(message));

            return RedirectToAction("Index");
        }

        public ActionResult Candidate(int id)
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

            var message = "Your candidacy to <strong>'" + agency.Name + "'</strong> has to be approved by administrator.";
            Alerts.Add(TempData, new AlertProps(message));

            return RedirectToAction("Details", new { agency.Id });
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

                Db.Mails.Add(new Mail
                {
                    FromUser = ApplicationUser,
                    ToUser = candidator,
                    MailType = MailType.Informational,
                    Title = "Your candidacy has been approved",
                    Body = "You now participate in '"+agency.Name+"' agency.",
                });

                Db.SaveChanges();
            }

            var message = "Candidacy accepted successfully.";
            Alerts.Add(TempData, new AlertProps(message));

            return RedirectToAction("Details", new { agency.Id });
        }

        public ActionResult DeclineCandidacy(string id)
        {
            var agency = ApplicationUser.Agency;
            var candidacy = Db.AgencyCandidacies.FirstOrDefault(x => x.Agency.Id == agency.Id && x.Candidator.Id == id);
            if (candidacy != null)
            {
                Db.AgencyCandidacies.Remove(candidacy);
                Db.SaveChanges();
            }
            
            var message = "Candidacy declined successfully.";
            Alerts.Add(TempData, new AlertProps(message));

            return RedirectToAction("Details", new { agency.Id });
        }

        public ActionResult Leave()
        {
            var agency = ApplicationUser.Agency;
            
            agency.Participants.Remove(ApplicationUser);
            var agencyCreator = agency.Participants.FirstOrDefault(x => x.IsAgencyCreator);
            Db.Mails.Add(new Mail
            {
                FromUser = ApplicationUser,
                ToUser = agencyCreator,
                MailType = MailType.ParticipationRequest,
                Title =
                    string.Format("User {0} {1} has left '{2}' company.", ApplicationUser.FirstName,
                        ApplicationUser.LastName, agency.Name),
                Body = "For more information go to 'My Agency > Candidates'",
            });

            var message = "Agency left successfully.";
            Alerts.Add(TempData, new AlertProps(message));

            Db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
