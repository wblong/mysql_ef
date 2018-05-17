

namespace XML_Serial.XML
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    [Serializable]
    public class medicalSub
    {
        [XmlAttribute]
        public string ID;

        [XmlAttribute]
        public string Name;

        public MedicalType MedicalType; 
    }
    [Serializable]
    public class MedicalType
    {
        [XmlAttribute]
        public string ID;

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string MedicalDoc;

        [XmlAttribute]
        public string MedicalDate;

        [XmlElement]
        public List<Item> Item;
    }
    public class Item
    {
        [XmlAttribute]
        public string ID;

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Unit;

        [XmlAttribute]
        public string Parameters;


        public string Results;

        public string Value;

        public string Disease;

        public string MedicalBodyPart;

        public string MedicalImage;

        public string Conclusion;


    }  
}
