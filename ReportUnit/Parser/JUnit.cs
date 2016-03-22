using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ReportUnit.Model;

namespace ReportUnit.Parser
{
    internal class JUnit : IParser
    {
        public Report Parse(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File {0} does not exist.", filePath));
            }

            XDocument doc = XDocument.Load(filePath);



            return new Report();
        }
    }
}
