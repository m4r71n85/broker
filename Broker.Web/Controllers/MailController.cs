using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Broker.Web.Controllers
{
    public class MailController : BaseController
    {
        //
        // GET: /Mail/
        public ActionResult Inbox()
        {
            var mails = ApplicationUser.Mails;
            return View(mails);
        }

        public ActionResult Read(int? id)
        {
            var mail = ApplicationUser.Mails.FirstOrDefault(x => x.Id == (id ?? -1));
            mail.IsRead = true;
            Db.SaveChanges();

            return View(mail);
        }
    }
}