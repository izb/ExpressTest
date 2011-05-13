//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace com.kupio.ExpressTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using com.kupio.ExpressTest.UnitTesting;
    using System.Diagnostics;

    public class Runner
    {
        public void run()
        {
            Assembly thisAsm = Assembly.GetCallingAssembly();
            List<Type> types = thisAsm.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
            foreach (Type t in types)
            {
                object[] attribs = t.GetCustomAttributes(true);
                foreach (object a in attribs)
                {
                    if (a is TestClass)
                    {
                        ProcessClass(t);
                        continue;
                    }
                }
            }
        }

        private void ProcessClass(Type t)
        {
            MethodInfo[] methods = t.GetMethods();
            foreach (MethodInfo m in methods)
            {
                object[] attribs = m.GetCustomAttributes(true);
                foreach (object a in attribs)
                {
                    if (a is TestInitialize)
                    {
                        Trace.WriteLine("Init: " + m);
                        continue;
                    }

                    if (a is TestCleanup)
                    {
                        Trace.WriteLine("Cleanup: " + m);
                        continue;
                    }

                    if (a is TestMethod)
                    {
                        Trace.WriteLine("Method: " + m);
                        continue;
                    }

                }
            }
        }
    }
}
