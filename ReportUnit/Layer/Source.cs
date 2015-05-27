namespace ReportUnit.Layer
{
    using System.Collections.Generic;

    internal class Source
    {
        public Source ()
        {
            SourceFiles = new Dictionary<string, string>();
        }

        public Dictionary<string, string> SourceFiles { get; private set; }
    }
}
