//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace ExpressTest.UnitTesting
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public class TestCleanup : Attribute
    {
    }
}
