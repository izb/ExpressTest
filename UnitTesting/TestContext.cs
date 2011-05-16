//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace ExpressTest.UnitTesting
{
    using System.IO;

    public class TestContext
    {
        public string TestDir
        {
            get
            {
                string cd = Directory.GetCurrentDirectory();
                while (!HasSolutionFile(cd))
                {
                    cd = Directory.GetParent(cd).ToString();
                }
                return cd + "\\ExpressTestOutput\\";
            }
        }

        private bool HasSolutionFile(string cd)
        {
            return (Directory.GetFiles(cd, "*.sln").Length > 0);
        }

        public string TestName { get; set; }
    }
}
