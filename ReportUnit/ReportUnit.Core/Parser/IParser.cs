using ReportUnit.Core.Model;

namespace ReportUnit.Core.Parser
{
    public interface IParser
    {
        Report Parse(string filePath);
    }
}
