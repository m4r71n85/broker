using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class Agency
    {
        private ICollection<ApplicationUser> user;

        public Agency()
        {
            this.user = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ApplicationUser> Participants
        {
            get { return user; }
            set { user = value; }
        }
    }
}
