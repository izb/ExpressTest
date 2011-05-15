//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace com.kupio.ExpressTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using com.kupio.ExpressTest.UnitTesting;
    using System.Xml;

    public class Runner
    {
        public void run()
        {
            /*
            try
            {
                Directory.Delete(new TestContext().TestDir, true);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                // Ignore
            }
            */


            Assembly thisAsm = Assembly.GetCallingAssembly();
            List<Type> types = thisAsm.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();

            string assemblyName = thisAsm.FullName.Substring(0, thisAsm.FullName.IndexOf(','));

            List<string> includes = ReadTestSet(thisAsm, assemblyName);

            List<string> fails = new List<string>();
            int testCount = 0;

            foreach (Type t in types)
            {
                object[] attribs = t.GetCustomAttributes(true);
                foreach (object a in attribs)
                {
                    if (a is TestClass)
                    {
                        ProcessClass(t, fails, includes, ref testCount);
                        continue;
                    }
                }
            }

            Trace.WriteLineIf(fails.Count > 0, "WARNING: There were " + fails.Count + " failed tests!!");

            if (fails.Count > 0)
            {
                using(TextWriter tw = new StreamWriter(new TestContext().TestDir + "/" + assemblyName+".testSet.xml", false))
                {
                    tw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    tw.WriteLine("<tests>");
                    Trace.WriteLine("Summary of failure:");
                    foreach (var fail in fails)
                    {
                        Trace.WriteLine("FAILED: " + fail);
                        tw.WriteLine("    <include>"+fail+"</include>");
                    }
                    tw.WriteLine("</tests>");
                }
            }
            Trace.WriteLine("Pass rate: " + (testCount - fails.Count) + "/" + testCount);

            Trace.WriteLineIf(fails.Count == 0, "YESSSS!!");

        }

        private List<string> ReadTestSet(Assembly asm, string asmName)
        {
            string xmlRes = asmName + ".res.express." + asmName + ".testSet.xml";

            List<string> includes = new List<string>();

            using (Stream s = asm.GetManifestResourceStream(xmlRes))
            {
                if (s == null)
                {
                    return null;
                }

                using (XmlReader reader = XmlReader.Create(s))
                {
                    while (reader.ReadToFollowing("include"))
                    {
                        includes.Add(reader.ReadElementContentAsString());
                    }
                }
            }

            string[] names = asm.GetManifestResourceNames();

            return includes.Count > 0? includes : null;
        }

        private void ProcessClass(Type t, List<string> fails, List<string> includes, ref int testCount)
        {
            MethodInfo[] methods = t.GetMethods();

            MethodInfo init = null;
            MethodInfo cleanup = null;
            List<MethodInfo> tests = new List<MethodInfo>();
            Dictionary<MethodInfo, Type> expectedExceptions = new Dictionary<MethodInfo, Type>();

            foreach (MethodInfo m in methods)
            {
                object[] attribs = m.GetCustomAttributes(true);
                foreach (object a in attribs)
                {
                    if (a is TestInitialize)
                    {
                        /* TODO: Can you have multiple inits? */
                        init = m;
                    }

                    if (a is TestCleanup)
                    {
                        /* TODO: Can you have multiple cleanups? */
                        cleanup = m;
                    }

                    if (a is TestMethod)
                    {
                        if (includes == null || includes.Contains(t.Name+"."+m.Name))
                        {
                            tests.Add(m);
                        }
                    }

                    if (a is ExpectedException)
                    {
                        expectedExceptions.Add(m, ((ExpectedException)a).Type);
                    }

                }
            }

            ConstructorInfo ci = t.GetConstructor(new Type[] { });
            object testObject = ci.Invoke(new Object[] { });

            foreach (MethodInfo method in tests)
            {
                try
                {
                    SetProperty(t, "TestContext", new TestContext() { TestName = method.Name }, testObject);
                }
                catch (Exception)
                {
                    Trace.WriteLine("No test context was set.");
                }

                if (init != null)
                {
                    Call(t, init, testObject);
                }

                bool passed = true;
                try
                {
                    testCount++;
                    Call(t, method, testObject);
                }
                catch (Exception e)
                {
                    if (!(expectedExceptions.ContainsKey(method) && e.InnerException.GetType() == expectedExceptions[method]))
                    {
                        fails.Add(t.Name + "." + method.Name);
                        passed = false;
                        Debug.WriteLine("FAIL: "+method.Name+"; "+e.InnerException.Message);
                    }
                }

                Trace.WriteLineIf(passed, "OK: "+method.Name);

                if (cleanup != null)
                {
                    Call(t, cleanup, testObject);
                }
            }
        }

        private static void SetProperty(Type t, string name, object val, object testObject)
        {
            t.InvokeMember(name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                    Type.DefaultBinder, testObject, new object[] { val });
        }

        private static void Call(Type t, MethodInfo method, object testObject)
        {
            t.InvokeMember(method.Name,
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, testObject, new Object[] { });
        }
    }
}
