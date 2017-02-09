using NUnit.Framework;

namespace ReportUnitTest
{
    [TestFixture]
    public class JUnitTests
    {
        [Test]
        public void Test()
        {
            TestContext.Progress.WriteLine("Test Pass!");
        }
    }
}
