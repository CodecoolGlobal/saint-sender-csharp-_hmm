using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using OpenPop.Mime;
using SaintSender.Core.Entities;

namespace SaintSender.DesktopUI.ViewModels
{
    class MainViewModel : PropertyNotifier
    {
        private static MainViewModel _instance;
        private List<Message> _emailsInMessage;
        private List<Mail> _emailsToShow;

        
        private MainViewModel()
        {
            _emailsInMessage = new List<Message>();
            _emailsToShow = new List<Mail>();
        }

        public List<Message> EmailsInMessage
        {
            get { return _emailsInMessage; }
            set { _emailsInMessage = value; }
        }

        public List<Mail> EmailsToShow
        {
            get { return _emailsToShow; }
            set { _emailsToShow = value; }
        }

        public string UserEmail
        {
            get;set;
        }

        public string Password
        {
            get;set;
        }


        internal List<Message> GetEmails()
        {
            List<Message> resultList = new List<Message>();
            try
            {
                Pop3Client client = new Pop3Client();
                client.Connect("pop.gmail.com", 995, true);
                client.Authenticate(UserEmail, Password, AuthenticationMethod.UsernameAndPassword);
                int messageCount = client.GetMessageCount();
                List<Message> allEmails = new List<Message>();

                for (int i = messageCount; i > 0; i--)
                {
                    allEmails.Add(client.GetMessage(i));
                }

                resultList = allEmails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return resultList;
        }

        public void BuildUpEmailsToShow()
        {
            foreach (Message email in EmailsInMessage)
            {
                if(email != null) 
                {
                    string body;
                    MessagePart messagePart = email.FindFirstPlainTextVersion();
                    if (!email.MessagePart.IsMultiPart)
                    {
                        body = email.MessagePart.GetBodyAsText();
                    }
                    else
                    {
                        body = messagePart.GetBodyAsText();
                    }
                    AddToEmailsToShowList(new Mail { Subject = email.Headers.Subject, Sender = email.Headers.From.ToString(), Date = email.Headers.DateSent, Body = body });
                    OnPropertyChanged("Email");
                }
            }
        }

        internal void AddToEmailsToShowList(Mail oneMail)
        {
            _emailsToShow.Add(oneMail);
        }

        internal void handleLogIn(string text, string password)
        {
            UserEmail = text;
            Password = password;
            _emailsInMessage = GetEmails();
            BuildUpEmailsToShow();
        }

        public static MainViewModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MainViewModel();
            }

            return _instance;
        }
    }
}
