using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaintSender.Core.Entities
{
    [Serializable]
    public class Mail :  ISerializable
    {

        private static string pathPart = "./data/";


        public int ID
        {
            get;set;
        }

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


        public Mail() { }

        public Mail(SerializationInfo info, StreamingContext context)
        {
            ID = (int)info.GetValue("id", typeof(int));
            Subject = (String)info.GetValue("subject", typeof(String));
            Sender = (String)info.GetValue("sender", typeof(String));
            Date = (DateTime)info.GetValue("date", typeof(DateTime));
            Body = (String)info.GetValue("body", typeof(String));
            
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", this.ID);
            info.AddValue("subject", this.Subject);
            info.AddValue("sender", this.Sender);
            info.AddValue("date", this.Date);
            info.AddValue("body", this.Body);
        }

        public static void Serialize(string output, ObservableCollection<Mail> mails, String email)
        {
            String path = pathPart + email + ".txt";
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(path)) File.Delete(path);
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, mails);
            stream.Close();
        }

        public static ObservableCollection<Mail> Deserialize(String email)
        {
            String path = pathPart + email + ".txt";
            ObservableCollection<Mail> _emailsToShow = null;
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _emailsToShow = (ObservableCollection<Mail>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally { fs.Close(); }
            return _emailsToShow;
        }
    }
}
