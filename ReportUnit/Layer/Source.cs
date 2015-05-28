namespace ReportUnit.Layer
{
    using System.Collections.Generic;

    internal class Source
    {
        public Source ()
        {
            SourceFiles = new Dictionary<string, string>();
        }

        /// <summary>
        /// Used when creating a folder-report, will contain 
        /// file-paths and HTML source of each file
        /// </summary>
        public Dictionary<string, string> SourceFiles { get; private set; }
    }
}
