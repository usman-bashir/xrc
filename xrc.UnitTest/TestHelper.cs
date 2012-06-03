using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace xrc
{
    public static class TestHelper
    {
        public static string GetWorkingPath()
        {
            var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            return Path.GetDirectoryName((new Uri(codeBase)).LocalPath);
        }

        public static string GetFile(string file)
        {
            return Path.Combine(GetWorkingPath(), file);
        }

        public static void Throws<T>(Action func) where T : Exception
        {
            var exceptionThrown = false;
            try
            {
                func.Invoke();
            }
            catch (T)
            {
                exceptionThrown = true;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(T), ex.GetType());
            }

            if (!exceptionThrown)
            {
                throw new AssertFailedException(
                    String.Format("An exception of type {0} was expected, but not thrown", typeof(T))
                    );
            }
        }
    }
}
