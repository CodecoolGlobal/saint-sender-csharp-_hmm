using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.Core.Entities
{
    public class User
    {
        private const string path = @"E:\TestForSearching\MailAndPassword.txt";

        public string UserName
        {
            get; set;
        }
        
        public string EmailAdress
        {
            get; set;
        }
        
        public string Password
        {
            get; set;
        }
        public string OriginPassword
        {
            get; set;
        }


        public void SaveUser()
        {
            String text = UserName + " " + Password; 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, text);
        }
        
        public bool HaveAlreadyLoggedInUser()
        {
            String dat = File.ReadAllText(path);
            if (dat == "") return false;
            return true;
        }

        public String  GetSavedUsername()
        {
            String dat = File.ReadAllText(path);
            String[] strlist = dat.Split(' ');
            return strlist[0];
        } 
        public String  GetSavedpassword()
        {
            String dat = File.ReadAllText(path);
            String[] strlist = dat.Split(' ');
            return strlist[1];
        }
    }
}
