using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;

namespace Broker.Models
{
    public class Mail
    {
        public Mail()
        {
            CreatedAt = DateTime.Now;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; private set; }
        public MailType MailType { get; set; }

        [InverseProperty("Mails")]
        public virtual ApplicationUser ToUser { get; set; }
        public virtual ApplicationUser FromUser { get; set; }

    }
}
