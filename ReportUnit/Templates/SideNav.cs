namespace ReportUnit.Templates
{
    public class SideNav
    {
        public static string GetSource()
        {
            return @"<li class='waves-effect report-item'>
	                    <a href='./@Model.GetHtmlFileName()'>
		                    <i class='mdi-action-assignment'></i>
		                    @Model.GetHtmlFileName().Replace("".html"", """")
	                    </a>
                    </li>".Replace("\r\n", "").Replace("\t", "").Replace("    ", "");
        }

        public static string GetIndexLink()
        {
            return @"<li class='waves-effect report-item'>
	                    <a href='./Index.html'>
		                    <i class='mdi-action-assignment'></i>
		                    Index
	                    </a>
                    </li>".Replace("\r\n", "").Replace("\t", "").Replace("    ", "");
        }
    }
}
