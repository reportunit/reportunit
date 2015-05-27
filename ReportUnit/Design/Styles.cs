namespace ReportUnit.Design
{
    using ReportUnit.Support;

    internal static class Styles
    {
        public static string Apply(this string html, Theme theme)
        {
            string css = "";

            switch (theme)
            {
                case Theme.Dark:
                    css = Dark;
                    break;
                case Theme.Standard:
                default:
                    return html;
            }

            return html.Replace(ReportHelper.MarkupFlag("themestyles"), css + ReportHelper.MarkupFlag("themestyles"));
        }

        private static string Dark
        {
            get
            {
                string css = @"<style>html { background-color: #333; }
                                body { color: #fff; }
                                #reportunit-container { background-color: #333; }
                                #topbar { border-bottom: 1px solid #595959; }
                                #dashboard { background-color: #444; border-bottom: 1px solid #595959; }
                                .summary-item { background: #222 none repeat scroll 0 0; border-left: 1px solid #c2c2c2; }
                                .summary-value { color: #fff; }
                                .summary-param { color: #eee; }
                                .dropdown-menu { background-color: #737373; }
                                .dropdown-menu .divider { background-color: #999; }
                                .btn-group > .btn:first-child:not(:last-child):not(.dropdown-toggle) { background-color: #737373; color: #fff; }
                                .fixture-container { border: 1px solid #929da1; }
                                .fixture-footer { background-color: #404040; }
                                .is-expanded { border-color: #eee !important; color: #fff; }
                                .simple-grey tr { border-bottom: 1px solid #595959; }
                                p.description { border: 1px solid #595959; color: #ddd; }
                                pre { background: #444 none repeat scroll 0 0; border: 1px solid #ccc; color: #eee; }
                                .dropdown-menu > li > a:focus, .dropdown-menu > li > a:hover { background-color: #555; color: #eee; }</style>";

                return css;
            }
        }
    }
}
