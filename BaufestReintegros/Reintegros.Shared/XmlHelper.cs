using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;


namespace Reintegros.Shared
{
    public static class XmlHelper
    {
        public static T ConvertNode<T>(XmlNode node) where T : class
        {
            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.InnerXml);
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(T));
            T result = (ser.Deserialize(stm) as T);

            return result;
        }

        public static int GetResultID(XmlNode resultsNode)
        {
            int id = 0;

            XmlNamespaceManager ns = new XmlNamespaceManager(resultsNode.OwnerDocument.NameTable);
            ns.AddNamespace("sp", resultsNode.NamespaceURI);
            var errorCode = resultsNode.SelectSingleNode("//sp:ErrorCode", ns).InnerText;
            if (errorCode == "0x00000000")
            {                
                id = int.Parse(resultsNode.SelectSingleNode("//@ows_ID").Value);
            }
            return id;
        }

        public static List<string> ParseComboResult(XmlNode ndList,string fieldName)
        {
            var innernd = ndList.InnerXml;

            XmlDocument xdoc = new XmlDocument();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("ans", "http://schemas.microsoft.com/sharepoint/soap/");

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(ndList.SelectSingleNode(".//ans:Fields", nsmgr).OuterXml);

            MemoryStream stream = new MemoryStream(byteArray);
            XElement xe = XElement.Load(stream);

            XElement qry =
                    (from field in xe.Descendants()
                     where field.Attribute("Name") != null
                     where field.Attribute("Name").Value == fieldName
                     select field).Single();

            List<string> ret = new List<string>();

            foreach (XElement xle in qry.XPathSelectElements(".//ans:CHOICES", nsmgr).Elements())
            {
                ret.Add(xle.Value);
            }

            return ret;
        }

    }

}
