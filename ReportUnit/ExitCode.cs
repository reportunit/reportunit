using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportUnit
{
    public enum ExitCode
    {
        Success = 0,
        Inconclusive = 1,
        Error = 2,
        Failure = 3,
        BadInput = 10
    }
}
