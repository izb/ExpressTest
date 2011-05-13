//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace com.kupio.ExpressTest.UnitTesting
{
    using System.IO;

    public class TestContext
    {
        public string TestDir
        {
            get
            {
                return Directory.GetCurrentDirectory() + "\\ExpressTest\\";
            }
        }

        public string TestName
        {
            get
            {
                return "JeffTheTest";
            }
        }
    }
}
