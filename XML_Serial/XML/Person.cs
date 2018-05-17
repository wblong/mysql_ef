namespace XML_Serial.XML
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

   
    public class Person
    {
        [XmlAttribute]
        public string IDCard;

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string MedicalID;

        [XmlAttribute]
        public string Sex;

        [XmlAttribute]
        public string Age;

        [XmlAttribute]
        public string MedicalRecordDate;

        [XmlAttribute]
        public string MedicalReportDate;

        [XmlAttribute]
        public string MedicalCount;

        [XmlAttribute]
        public string HospitalID;

        [XmlAttribute]
        public string HospitalName;

        [XmlArrayItem("Result")]
        public List<string> Results;

        [XmlArrayItem("Conclusion")]
        public List<string> Conclusions;

        [XmlArrayItem("Suggestion")]
        public List<string> Suggestions;

        public String Health;
        public Class1 Class1;
    }
    public class Class1
    {
       public SubClass SubClass;
    }
    public class SubClass
    {
        public string para1;
        public string para2;
    }
}
