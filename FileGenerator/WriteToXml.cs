using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CrossPlatformConsole.FileGenerator
{
    public class WriteToXml
    {
        public static WriteToXml instance = new WriteToXml();

        public XmlTextWriter _writer;

        public WriteToXml()
        {
            if(instance == null)
            {
                instance = this;
                _writer = new XmlTextWriter(@"output\FileName.xml", null);
                _writer.WriteStartDocument();
                _writer.WriteStartElement("root");
            }
        }
        
        public void Write(string appName, string pathName, string appVersion, string appCompany)
        {
            _writer.WriteStartElement("software");
            _writer.WriteAttributeString("name", appName ?? "");
            _writer.WriteAttributeString("path", pathName ?? "");
            _writer.WriteAttributeString("version", appVersion ?? "");
            _writer.WriteAttributeString("company", appCompany ?? "");
            _writer.WriteEndElement();
        }

        public void Close()
        {
            _writer.WriteEndElement();
            _writer.Close();
        }


    }
}
