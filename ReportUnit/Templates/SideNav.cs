namespace ReportUnit.Templates
{
    public class SideNav
    {
        public static string GetSource()
        {
            return @"<li class='waves-effect report-item'>
	                    <a href='./@Model.GetHtmlFileName()'>
		                    <i class='mdi-action-assignment'></i>
		                    <span class='sidenav-filename'>@Model.GetHtmlFileName().Replace("".html"", """")</span>
	                    </a>
                    </li>".Replace("\r\n", "").Replace("\t", "").Replace("    ", "");
        }

        public static string GetIndexLink()
        {
            return @"<li class='waves-effect report-item'>
	                    <a href='./Index.html'>
		                    <i class='mdi-action-assignment'></i>
		                    <span class='sidenav-filename'>Index</span>
	                    </a>
                    </li>".Replace("\r\n", "").Replace("\t", "").Replace("    ", "");
        }
    }
}
