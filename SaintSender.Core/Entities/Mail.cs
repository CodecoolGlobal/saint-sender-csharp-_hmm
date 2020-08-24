using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.Core.Entities
{
    public class Mail
    {
        public string Subject
        {
            get;set;
        }

        public string Sender
        {
            get;set;
        }

        public DateTime Date
        {
            get;set;
        }

        public string Body
        {
            get;set;
        }
    }
}
