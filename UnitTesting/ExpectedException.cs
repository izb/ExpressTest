//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace com.kupio.ExpressTest.UnitTesting
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public class ExpectedException : Attribute
    {
        public ExpectedException(Type t)
        {
            this.typ = t;
        }

        public Type typ { get; set; }
    }
}
