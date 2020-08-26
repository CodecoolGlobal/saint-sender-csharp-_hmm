using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.Core.Entities
{
    [Serializable]
    public class Mail : IDeserializationCallback, ISerializable
    {

        private static string path = @"C:\Users\Máté\Desktop\Advance\3_TW\1.txt";

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

        public static void Serialize(string output, ObservableCollection<Mail> mails )
        {

            IFormatter formatter = new BinaryFormatter();
            if (!File.Exists(path))
            {
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, mails);
                stream.Close();
            }
            else
            {
                File.Delete(path);
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, mails);
                stream.Close();
            }
        }

        public static ObservableCollection<Mail> Deserialize()
        {
            ObservableCollection<Mail> _emailsToShow = null;

            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and
                // assign the reference to the local variable.
                _emailsToShow = (ObservableCollection<Mail>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return _emailsToShow;
            
           
        }

        void IDeserializationCallback.OnDeserialization(Object sender)
        {
            
        }
    }
}
