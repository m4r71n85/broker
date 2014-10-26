using Broker.Data;
using Broker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Broker.Web.Controllers
{
    public class BaseController : Controller
    {
        protected static ApplicationDbContext Db = new ApplicationDbContext();

        public BaseController()
            : this(new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(Db)))
        {
        }

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            var userId = System.Web.HttpContext.Current.User;
            ApplicationUser = UserManager.FindById(userId.Identity.GetUserId());

            ViewBag.ApplicationUser = ApplicationUser;
        }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
	}
}