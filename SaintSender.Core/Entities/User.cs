using System;
using System.IO;

namespace SaintSender.Core.Entities
{
    public class User
    {
        private static string path = "./data/user.txt";

        public string UserName
        {
            get; set;
        }
        
        public string Password
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
        
        public static bool HaveAlreadyLoggedInUser()
        {
            if (String.IsNullOrEmpty(File.ReadAllText(path))) return false;
            return true;
        }

        public static String  GetSavedUsername()
        {
            return File.ReadAllText(path).Split(' ')[0];
        } 
        
        public static String  GetSavedpassword()
        {
            return File.ReadAllText(path).Split(' ')[1];
        }
    }
}
