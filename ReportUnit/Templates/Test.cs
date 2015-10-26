using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUnit.Templates
{
    internal class Test
    {
        public static string GetSource()
        {
            return @"<tr>
                        <td class='test-name'>@Model.Name</td>
                        <td class='test-status @Model.Status.ToString().ToLower()'>
                            <span class='label @Model.Status.ToString().ToLower()'>@Model.Status.ToString()</span>
                            @if (!String.IsNullOrEmpty(Model.Description))
                            {
                                <p class='description'>Model.Description</p>
                            }
                            @if (!String.IsNullOrEmpty(Model.StatusMessage)) {
                                <pre>@Model.StatusMessage</pre>
                            }
                        </td>
                        <td class='test-features @Model.GetCategories()'></td>
                    </tr>".Replace("\r\n", "").Replace("\t", "").Replace("    ", "");
        }
    }
}
