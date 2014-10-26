using System.Collections.Generic;

namespace Broker.Models
{
    public class Agency
    {
        private List<ApplicationUser> _participants;
        private List<ApplicationUser> _candidators;

        public Agency()
        {
            _participants = new List<ApplicationUser>();
            _candidators = new List<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public virtual List<ApplicationUser> Participants { get { return _participants; } set { _participants = value; } }
        public virtual List<ApplicationUser> Candidators { get { return _candidators; } set { _candidators = value; } }
    }
}