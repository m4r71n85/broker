using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public int AmontOfBedrooms { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public string Address { get; set; }
        public float Area { get; set; } //square meters
        public virtual Client Client { get; set; }
    }

    public enum Currency
    {
        Eur, Bgn, Usd
    }
}
