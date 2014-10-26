using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class AgencyCandidacy
    {
        public int Id { get; set; }
        
        [Index(IsUnique = true)]
        public ApplicationUser Candidator { get; set; }
        public Agency Agency { get; set; }
    }
}
