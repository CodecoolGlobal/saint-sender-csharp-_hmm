using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using OpenPop.Mime;
using SaintSender.Core.Entities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SaintSender.DesktopUI.ViewModels
{
    class MainViewModel : PropertyNotifier
    {
        private static MainViewModel _instance;

        private List<Message> _emailsInMessage;
        private ObservableCollection<Mail> _emailsToShow;
        private Mail selectedEmail;
        private User loggedInUser;
        
        private MainViewModel()
        {
            _emailsInMessage = new List<Message>();
            _emailsToShow = new ObservableCollection<Mail> ();
        }

        public List<Message> EmailsInMessage
        {
            get { return _emailsInMessage; }
            set { _emailsInMessage = value; }
        }

        public ObservableCollection<Mail> EmailsToShow
        {
            get { return _emailsToShow; }
            set { _emailsToShow = value; }
        }

        public Mail SelectedMail
        {
            get { return selectedEmail; }
        }

        internal List<Message> GetEmails()
        {
            List<Message> resultList = new List<Message>();
            try
            {
                Pop3Client client = new Pop3Client();
                client.Connect("pop.gmail.com", 995, true);
                client.Authenticate(loggedInUser.UserName, loggedInUser.Password, AuthenticationMethod.UsernameAndPassword);
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
            int idCounter = _emailsInMessage.Count();
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
                    AddToEmailsToShowList(new Mail { ID = idCounter, Subject = email.Headers.Subject, Sender = email.Headers.From.DisplayName, Date = email.Headers.DateSent, Body = body });
                    idCounter--;
                    OnPropertyChanged("Email");
                }
            }
        }

        internal void AddToEmailsToShowList(Mail oneMail)
        {
            _emailsToShow.Add(oneMail);
        }

        internal void PutEmailIntoSelectedField(int id)
        {
            foreach (Mail mail in _emailsToShow)
            {
                if (mail.ID == id)
                {
                    selectedEmail = mail;
                }
            }
        }

        internal void handleLogIn(string text, string password)
        {
            User user = new User();
            user.UserName = text;
            user.Password = password;
            user.SaveUser();

            loggedInUser = user;

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
