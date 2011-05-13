//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace com.kupio.ExpressTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Runner
    {
        public void run()
        {
            Assembly thisAsm = Assembly.GetCallingAssembly();
            List<Type> types = thisAsm.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList(); 
            throw new System.NotImplementedException();
        }
    }
}
