using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApplicationUser : IdentityUser
    {
        private List<Mail> _mails;
        private List<Offer> _offers;

        public ApplicationUser()
        {
            _mails = new List<Mail>();
            _offers = new List<Offer>();
        }
        public virtual Agency Agency { get; set; }
        public bool IsAgencyCreator { get; set; }
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneCell { get; set; }
        public virtual List<Mail> Mails { get { return _mails; } set { _mails = value; } }

        public virtual List<Offer> Offers { get { return _offers; } set { _offers = value; } }
    }
}
