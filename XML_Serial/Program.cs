using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Serial
{
    using System.IO;
    using XML_Serial.XML;
    class Program
    {
        static void Main(string[] args)
        {
            string txt = File.ReadAllText("../../test.xml");
            Root k= XmlSerialize.DeserializeXML<Root>(txt);
            string res = XmlSerialize.SerializeXML<Root>(k);
            Console.WriteLine(res);
            //Root r = new Root();

            //r.Person = new Person();
            //r.Person.IDCard = "22";
            //r.Person.Results = new List<string>();
            //r.Person.Results.Add("1");
            //r.Person.Results.Add("1");
            //r.Person.Results.Add("1");
            //r.Person.Suggestions = new List<string>();
            //r.Person.Suggestions.Add("2");
            //r.Person.Suggestions.Add("2");
            //r.Person.Suggestions.Add("2");

            //r.MedicalItems = new List<MedicalSub>();
            //MedicalSub ms = new MedicalSub();
            //ms.ID = "ss";
            //ms.Name = "de";
            //ms.MedicalType = new MedicalType();
            //ms.MedicalType.ID = "wa";
            //ms.MedicalType.Name = "s";
            //ms.MedicalType.MedicalDoc = "qa";
            //ms.MedicalType.MedicalDate = "2010-5-5";
            //ms.MedicalType.Item = new List<Item>();
            //Item it = new Item();
            //it.ID = "f";
            //it.Name = "s";
            //it.Results = "s";
            //ms.MedicalType.Item.Add(it);
            //ms.MedicalType.Item.Add(it);
            //r.MedicalItems.Add(ms);
            //r.MedicalItems.Add(ms);

            //Console.WriteLine("序列化成功……");
            //Console.WriteLine(XmlSerialize.SerializeXML<Root>(r));  
        }
    }

}
