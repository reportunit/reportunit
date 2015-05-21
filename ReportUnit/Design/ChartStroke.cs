namespace ReportUnit.Design
{
    using System.Text.RegularExpressions;

    internal static class ChartStroke
    {
        public static string ApplyStrokeColor(this string html, Theme theme)
        {
            string color = "#f7f7f7";
            
            switch (theme)
            {
                case Theme.Dark:
                    color = "#444444"; break;
                case Theme.Standard:
                default:
                    break;
            }

            Regex regex = new Regex(@"\/\*FLOTSTROKE\*\/.*\/\*FLOTSTROKE\*\/");

            return regex.Replace(html, @"/*FLOTSTROKE*/color: '" + color + "',/*FLOTSTROKE*/");
        }
    }
}
