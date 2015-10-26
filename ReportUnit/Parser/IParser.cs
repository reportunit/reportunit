using ReportUnit.Model;

namespace ReportUnit.Parser
{
    public interface IParser
    {
        Report Parse(string filePath);
    }
}
