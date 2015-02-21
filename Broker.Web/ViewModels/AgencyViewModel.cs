using Broker.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Broker.Web.ViewModels
{
    public class AgencyViewTeasedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

    }
    public class AgencyViewModel
    {
        private List<ApplicationUser> _agencyParticipants;

        public AgencyViewModel()
        {
            _agencyParticipants = new List<ApplicationUser>();
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

        public virtual List<ApplicationUser> AgencyParticipants { get { return _agencyParticipants; } set { _agencyParticipants = value; } }

    }
}