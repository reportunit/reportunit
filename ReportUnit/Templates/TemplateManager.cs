using System.Text;

namespace ReportUnit.Templates
{
    public class TemplateManager
    {
        public static string GetSummaryTemplate()
        {
            return GetEncodedResource(@"..\Templates\Summary.cshtml");
        }

        public static string GetFileTemplate()
        {
            return GetEncodedResource(@"..\Templates\File.cshtml");
        }

        private static string GetEncodedResource(string path)
        {
            return Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path));
        }
    }
}
