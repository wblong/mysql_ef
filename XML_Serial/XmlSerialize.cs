﻿
namespace XML_Serial
{
    using System.IO;
    using System.Xml.Serialization;

    public class XmlSerialize
    {
        /// <summary>
        /// 反序列化XML为类实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlObj"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(string xmlObj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlObj))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// 序列化实例为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeXML<T>(T obj) {
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(obj.GetType()).Serialize((TextWriter)writer, obj);
                return writer.ToString();
            }
        }
    }
}
