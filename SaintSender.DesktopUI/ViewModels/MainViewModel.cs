using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using OpenPop.Mime;

namespace SaintSender.DesktopUI.ViewModels
{
    class MainViewModel
    {
        private List<Message> _emailsToShow;

        List<Message> EmailsToShow
        {
            get { return _emailsToShow; }
            set { _emailsToShow = value; }
        }


        internal List<Message> GetEmails()
        {
            Pop3Client client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true);
            client.Authenticate("kisisit111@gmail.com", "adammate/1", AuthenticationMethod.UsernameAndPassword);
            int messageCount = client.GetMessageCount();
            List<Message> allEmails = new List<Message>();

            for (int i=messageCount; i > 0; i--)
            {
                allEmails.Add(client.GetMessage(i));
            }
            // msgList[0].MessagePart.MessageParts[0].GetBodyAsText();
           
            return allEmails;
        }
    }
}
