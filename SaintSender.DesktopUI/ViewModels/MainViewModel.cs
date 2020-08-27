using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenPop.Mime;
using SaintSender.Core.Entities;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.IO;

namespace SaintSender.DesktopUI.ViewModels
{
    class MainViewModel : PropertyNotifier
    {
        private static MainViewModel _instance;

        private List<Message> _emailsInMessage;
        private ObservableCollection<Mail> _emailsToShow;
        private Mail selectedEmail;
        private User loggedInUser;
        private string loginButtonContent;
        
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
            set { _emailsToShow = value; OnPropertyChanged("EmailsToShow"); }
        }

        public Mail SelectedMail
        {
            get { return selectedEmail; }
        }

        public string LoginButtonContent
        {
            set { loginButtonContent = value; OnPropertyChanged("LoginButtonContent"); }
            get { return loginButtonContent; }
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

        internal void CheckForNewEmails()
        {
            List<Message> refreshedList = GetEmails();
            bool isListsAreNotEqual = !refreshedList.All(_emailsInMessage.Contains);
            if (isListsAreNotEqual)
            {
                _emailsInMessage = refreshedList;
                BuildUpEmailsToShow();
            }
        }

        public void BuildUpEmailsToShow()
        {
            _emailsToShow = new ObservableCollection<Mail>();

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
                    OnPropertyChanged("EmailsToShow");
                }
            }
        }

        internal void SendEmail(string emailTo, string mailSubject, string mailBody)
        {
          
            MailMessage mail = new MailMessage(loggedInUser.UserName, emailTo);
            mail.Subject = mailSubject;
            mail.Body = mailBody;

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential(loggedInUser.UserName, loggedInUser.Password);
            smtpServer.EnableSsl = true;
            try
            {
                smtpServer.Send(mail);
            }
            catch (SmtpFailedRecipientsException e)
            {
                Console.WriteLine(e.Message);
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

            LoginButtonContent = "Logout";

            _emailsInMessage = new List<Message>();
            _emailsToShow = new ObservableCollection<Mail>();
            if (IsConnectedToInternet())
            {
                _emailsInMessage = GetEmails();
                BuildUpEmailsToShow();
                SaveMails(text);
            }
            else  ReadMails(text);
            
        }

        public void SaveMails(String email)
        {
            Mail.Serialize("", _emailsToShow, email); 
        }

        public void ReadMails(String email)
        {
            _emailsToShow =  Mail.Deserialize(email);
        }

        internal void handleLogout()
        {
            loggedInUser = null;
            LoginButtonContent = "Login";
            EmailsInMessage = null;
            EmailsToShow = null;
            File.WriteAllText("./data/user.txt", "");
        }

        internal bool isSomeoneLoggedIn()
        {
            if (loggedInUser != null)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public bool IsConnectedToInternet()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                bool i = reply.Status == IPStatus.Success;
                return i;
            }
            catch (Exception) { }
            return false;
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
