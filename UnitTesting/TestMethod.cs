//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace com.kupio.ExpressTest.UnitTesting
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public class TestMethod : Attribute
    {
    }
}
