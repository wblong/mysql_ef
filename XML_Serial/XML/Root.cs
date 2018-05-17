

namespace XML_Serial.XML
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class Root
    {
        [XmlElement("Person")]
        public Person person;
        //public Person Person;
        [XmlArray("MedicalItems"),XmlArrayItem("MedicalSub")]
        public List<medicalSub> medicalItems;
        //public List<MedicalSub> MedicalItems;
    }
}
