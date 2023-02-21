using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;

namespace MovilidadCF.Configuration
{
    public class AppSettings
    {
        private Dictionary<string, string> properties;

        public AppSettings()
        {
            properties = new Dictionary<string, string>();
            LoadXML(GetApplicationDirectory() + "\\app.config");
        }

        public AppSettings(string sFileName)
        {
            properties = new Dictionary<string, string>();
            LoadXML(sFileName);
        }

        private void LoadXML(string sFileName)
        {
            XmlTextReader reader = new XmlTextReader(sFileName);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.Read();

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(reader);
            XmlNodeList xnodes = xdoc.SelectNodes("config/application/param");

            foreach (XmlNode node in xnodes)
            {
                properties.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
            }
        }

        public string this[string key]
        {
            get
            {
                return properties[key];
            }
        }

        private string GetApplicationDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
        }
    }
}
