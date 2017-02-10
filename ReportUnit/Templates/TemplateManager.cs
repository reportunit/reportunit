using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace ReportUnit.Templates
{
    public class TemplateManager
    {
        public static string GetSummaryTemplate()
        {
            return GetEmbeddedResourceAsUtf8String("ReportUnit.Templates.Summary.cshtml");
        }

        public static string GetFileTemplate()
        {
            return GetEmbeddedResourceAsUtf8String("ReportUnit.Templates.File.cshtml");
        }

        private static string GetEmbeddedResourceAsUtf8String(string embeddedResourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(embeddedResourceName))
            using (var reader = new StreamReader(stream))
            {
                return Encoding.UTF8.GetString(Encoding.Default.GetBytes(reader.ReadToEnd()));
            }
        }
    }
}
