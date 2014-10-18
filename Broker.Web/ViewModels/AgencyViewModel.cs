using Broker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Broker.Web.ViewModels
{
    public class AgencyViewModel
    {
        private List<ApplicationUser> users;

        public AgencyViewModel()
        {
            users = new List<ApplicationUser>();
        }

        [Required]
        [Display(Name = "Agency name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Agency logo")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Home phone")]
        public string HomePhone { get; set; }

        [Required]
        [Display(Name = "Mobile phone")]
        public string MobilePhone { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        public virtual List<ApplicationUser> Users { get { return users; } set { users = value; } }

    }
}