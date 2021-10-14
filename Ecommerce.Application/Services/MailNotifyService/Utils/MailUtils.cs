using DotLiquid;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Ecommerce.Application.Services.MailNotifyService.Utils
{
    public static class MailUtils
    {
        public static string GetTemplateMail(string eventCode, object properties)
        {
            var doc = ReadConfigFile();
            XElement selectedElement = doc.Descendants().Where(x => (string)x.Attribute("id") == eventCode).FirstOrDefault();
            var mailTemplate = selectedElement.ToString();
            var result = SetEmailProperties(mailTemplate, properties);
            return result;
        }

        public static string GetSubjectMail(string eventCode)
        {
            var doc = ReadConfigFile();
            XElement selectedElement = doc.Descendants().Where(x => (string)x.Attribute("subject") == eventCode).FirstOrDefault();
            return selectedElement.Value;
        }

        private static XDocument ReadConfigFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string xml = "";
            using (Stream fileStream = assembly.GetManifestResourceStream("Ecommerce.Application.Services.MailNotifyService.TemplateMail.TemplateMail.xml"))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    xml = reader.ReadToEnd();
                }
            }
            XDocument doc = XDocument.Parse(xml);
            return doc;
        }

        private static string SetEmailProperties(string mailTemplate, object properties)
        {
            var template = Template.Parse(mailTemplate);
            var jsonHash = Hash.FromAnonymousObject(properties);
            return template.Render(jsonHash);
        }
    }
}
