//-----------------------------------------------------------------------
// ExpressTest. A stand-in for MSTest when using Visual Studio 2010 Express.
//-----------------------------------------------------------------------

namespace ExpressTest.UnitTesting
{
    public class Assert
    {
        public static void IsNotNull(object o, string message = "IsNotNull failure")
        {
            if (o == null)
            {
                throw new ExpressTestException(message);
            }
        }

        public static void IsTrue(bool b, string message)
        {
            if (b == false)
            {
                throw new ExpressTestException(message);
            }
        }

        public static void AreEqual(long l1, long l2, string message)
        {
            if (l1 != l2)
            {
                throw new ExpressTestException(message + (" [" + l1 + " != " + l2 + "]"));
            }
        }

        public static void Fail(string message)
        {
            throw new ExpressTestException(message);
        }

        public static void AreEqual(object o1, object o2, string message)
        {
            if (o1 == null && o2 == null)
            {
                return;
            }

            if (o1 == null || !o1.Equals(o2))
            {
                throw new ExpressTestException(message + (" [" + ((o1 == null) ? "null" : o1.ToString()) + " != " + ((o2 == null) ? "null" : o2.ToString()) + "]"));
            }
        }

        public static void AreNotEqual(object o1, object o2, string message)
        {
            if (o1 == null ^ o2 == null)
            {
                return;
            }

            if (o1 != null && o1.Equals(o2))
            {
                throw new ExpressTestException(message + "[" + ((o1 == null) ? "null" : o1.ToString()) + "]");
            }
        }
    }
}
