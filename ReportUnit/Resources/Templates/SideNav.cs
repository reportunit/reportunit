namespace ReportUnit.Templates
{
    public class SideNav
    {
        public static string Link
        {
            get
            {
                return @"<li class='waves-effect report-item'>
	                    <a href='./@(Model.FileName).html'>
		                    <i class='mdi-action-assignment'></i>
		                    <span class='sidenav-filename'>@Model.FileName</span>
	                    </a>
                    </li>".Replace("\r\n", "").Replace("\t", "").Replace("    ", "");
            }
        }

        public static string IndexLink
        {
            get
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
}
