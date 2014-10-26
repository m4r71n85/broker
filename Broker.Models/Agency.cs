using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models
{
    public class Agency
    {
        private List<ApplicationUser> _participants;
        private List<AgencyCandidacy> _candidacies;

        public Agency()
        {
            _participants = new List<ApplicationUser>();
            _candidacies = new List<AgencyCandidacy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }

        [InverseProperty("Agency")]
        public virtual List<ApplicationUser> Participants { get { return _participants; } set { _participants = value; } }

        [InverseProperty("Agency")]
        public virtual List<AgencyCandidacy> Candidacies { get { return _candidacies; } set { _candidacies = value; } }
    }
}