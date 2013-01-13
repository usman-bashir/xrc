using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicExpresso.UnitTest
{
    [TestClass]
    public class ExpressionEngine_Test
    {
        [TestMethod]
        public void Parse_Should_Understand_ReturnType()
        {
            var target = new ExpressionEngine();

            Assert.AreEqual(typeof(string), target.Parse("\"some string\"").ReturnType);
            Assert.AreEqual(typeof(int), target.Parse("234").ReturnType);
            Assert.AreEqual(typeof(object), target.Parse("null").ReturnType);
        }


        [TestMethod]
        public void Eval_Constants()
        {
            var target = new ExpressionEngine();

            Assert.AreEqual("ciao", target.Eval("\"ciao\""));
            Assert.AreEqual(45, target.Eval("45"));
            Assert.AreEqual(23423423423434, target.Eval("23423423423434"));
            //TODO Assert.AreEqual(45.232M, target.Eval("45.232M"));
            //TODO Assert.AreEqual(45.5, target.Eval("45.5"));
            //TODO Assert.AreEqual(45.8f, target.Eval("45.8f"));
            Assert.IsNull(target.Eval("null"));
            Assert.IsTrue((bool)target.Eval("true"));
            Assert.IsFalse((bool)target.Eval("false"));
        }

        [TestMethod]
        public void Eval_Numbers_Operators()
        {
            var target = new ExpressionEngine();

            Assert.AreEqual("ciao mondo", target.Eval("\"ciao \" + \"mondo\""));
            Assert.AreEqual(50, target.Eval("45 + 5"));
            Assert.AreEqual(40, target.Eval("45 - 5"));
            //TODO Assert.AreEqual(0.5, target.Eval("1.0 - 0.5"));
            Assert.AreEqual(8, target.Eval("2 * 4"));
            Assert.AreEqual(4, target.Eval("8 / 2"));
            //TODO Assert.AreEqual(9, target.Eval("3 ^ 2"));
        }

        [TestMethod]
        public void Eval_Comparison_Operators()
        {
            var target = new ExpressionEngine();

            Assert.IsFalse((bool)target.Eval("0 > 3"));
            Assert.IsTrue((bool)target.Eval("3 < 5"));
            Assert.IsTrue((bool)target.Eval("\"dav\" == \"dav\""));
            Assert.IsFalse((bool)target.Eval("\"dav\" == \"jack\""));
            Assert.IsFalse((bool)target.Eval("5 == 3"));
            Assert.IsTrue((bool)target.Eval("5 != 3"));
        }


        [TestMethod]
        public void Eval_Logical_Operators()
        {
            var target = new ExpressionEngine();

            Assert.IsTrue((bool)target.Eval("0 > 3 || true"));
            Assert.IsFalse((bool)target.Eval("0 > 3 && 4 < 6"));
        }

        [TestMethod]
        public void Eval_StaticMethod_of_PrimitiveTypes()
        {
            var target = new ExpressionEngine();

            Assert.AreEqual(Int32.MaxValue, target.Eval("Int32.MaxValue"));
            //TODO Assert.AreEqual(int.MaxValue, target.Eval("int.MaxValue"));
            Assert.AreEqual(Double.MaxValue, target.Eval("Double.MaxValue"));
            Assert.AreEqual(double.MaxValue, target.Eval("double.MaxValue"));
            Assert.AreEqual(DateTime.MaxValue, target.Eval("DateTime.MaxValue"));
            Assert.AreEqual(DateTime.Today, target.Eval("DateTime.Today"));
            Assert.AreEqual(string.Empty, target.Eval("string.Empty"));
        }

        [TestMethod]
        public void Eval_string_format()
        {
            var target = new ExpressionEngine();

            //TODO Assert.AreEqual(string.Format("ciao mondo, today is {0}", DateTime.Today),
            //                target.Eval("string.Format(\"ciao {0}, today is {1}\", \"mondo\", DateTime.Today))"));
            Assert.AreEqual(string.Format("ciao {0}, today is {1}", "mondo", DateTime.Today),
                            target.Eval("string.Format(\"ciao {0}, today is {1}\", \"mondo\", DateTime.Today.ToString())"));
        }

        [TestMethod]
        public void Eval_primitive_parameters()
        {
            var target = new ExpressionEngine();

            double x = 2;
            string y = "param y";
            var parameters = new[] {
                            new ExpressionParameter("x", x.GetType(), x),
                            new ExpressionParameter("y", y.GetType(), y)
                            };

            Assert.AreEqual(x, target.Eval("x", parameters));
            Assert.AreEqual(x + x + x, target.Eval("x+x+x", parameters));
            Assert.AreEqual(x * x, target.Eval("x * x", parameters));
            Assert.AreEqual(y, target.Eval("y", parameters));
            Assert.AreEqual(y.Length + x, target.Eval("y.Length + x", parameters));
        }

        [TestMethod]
        public void Eval_complex_parameters()
        {
            var target = new ExpressionEngine();

            var x = new MyTestService();
            var y = "davide";
            var z = 5;
            var w = DateTime.Today;
            var parameters = new[] {
                            new ExpressionParameter("x", x.GetType(), x),
                            new ExpressionParameter("y", y.GetType(), y),
                            new ExpressionParameter("z", z.GetType(), z),
                            new ExpressionParameter("w", w.GetType(), w)
                            };

            Assert.AreEqual(x, target.Eval("x", parameters));
            Assert.AreEqual(x.SomeNumber + 1, target.Eval("x.SomeNumber + 1", parameters));
            Assert.AreEqual(x.HelloWorld(), target.Eval("x.HelloWorld()", parameters));
            Assert.AreEqual(x.CallMethod(y, z, w), target.Eval("x.CallMethod( y, z,w)", parameters));
        }

        [TestMethod]
        public void Eval_complex_expression()
        {
            var target = new ExpressionEngine();

            var x = new MyTestService();
            var y = 5;
            var parameters = new[] {
                            new ExpressionParameter("x", x.GetType(), x),
                            new ExpressionParameter("y", y.GetType(), y),
                            };

            Assert.AreEqual(true, target.Eval("x.SomeNumber > y && x.HelloWorld().Length == 10", parameters));
            Assert.AreEqual(x.SomeNumber * 4 + 65 / x.SomeNumber, target.Eval("x.SomeNumber * 4 + 65 / x.SomeNumber", parameters));
        }

        // Missing tests
        // --------------
        // - iif
        // - new of custom types
        // - static method of custom types
        // - is operator
        // - typeof operator
    }

    public class MyTestService
    {
        public int SomeNumber
        {
            get { return 769; }
        }

        public string HelloWorld()
        {
            return "Ciao mondo";
        }

        public string CallMethod(string param1, int param2, DateTime param3)
        {
            return string.Format("{0} {1} {2}", param1, param2, param3);
        }
    }
}
