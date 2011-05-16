//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace ExpressTest.UnitTesting
{
    using System;

    public class ExpressTestException : Exception
    {
        public ExpressTestException(string message)
                : base(message)
        {
        }
    }
}
