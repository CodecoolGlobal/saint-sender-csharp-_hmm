using System;
using System.Collections.Generic;
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

        public void Serialize(string output)
        {

            IFormatter formatter = new BinaryFormatter();
            if (!File.Exists(path))
            {
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, this);
                stream.Close();
            }
            else
            {
                File.Delete(path);
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, this);
                stream.Close();
            }
        }

        public static Mail Deserialize()
        {
            Mail mail = new Mail();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            mail = (Mail)formatter.Deserialize(stream);
            stream.Close();
            return mail;
        }

        void IDeserializationCallback.OnDeserialization(Object sender)
        {
            
        }
    }
}
