using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DynamicExpression;
using xrc.Modules;
using System.Linq.Expressions;

namespace xrc.Script
{

    [TestClass()]
    public class ScriptService_Test
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void It_should_be_possible_to_eval_script_constant()
        {
            ScriptService target = new ScriptService();

            var parameters = new ScriptParameterList();

			IScriptExpression exp;

            exp = target.Parse("\"ciao\"", typeof(string), parameters);
            Assert.AreEqual("ciao", target.Eval(exp, parameters));

            exp = target.Parse("95", typeof(int), parameters);
            Assert.AreEqual(95, target.Eval(exp, parameters));

            // type mismatch
            TestHelper.Throws<ParseException>(() => target.Parse("\"ciao\"", typeof(int), parameters));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_script_system_method()
        {
            ScriptService target = new ScriptService();

            var parameters = new ScriptParameterList();
            parameters.Add(new ScriptParameter("name", typeof(string), "mondo"));

            IScriptExpression exp;

            exp = target.Parse("string.Format(\"ciao {0}\", name)", typeof(string), parameters);
            Assert.AreEqual("ciao mondo", target.Eval(exp, parameters));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_script_constant_datetime()
        {
            ScriptService target = new ScriptService();

            IScriptExpression exp;

            // TODO Attualmente uso uno script engine che non supporta questo tipo di sintassi.
            exp = target.Parse("new DateTime(2012, 02, 05)", typeof(DateTime), new ScriptParameterList());
            Assert.AreEqual(new DateTime(2012, 02, 05), target.Eval(exp, new ScriptParameterList()));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_script_invoking_a_module()
        {
            ScriptService target = new ScriptService();

            ScriptParameterList parameters = new ScriptParameterList();
            parameters.Add(new ScriptParameter("Test", typeof(TestModule), new TestModule()));

            IScriptExpression exp;

            exp = target.Parse("Test.Name", typeof(string), parameters);
            Assert.AreEqual("Davide", target.Eval(exp, parameters));

            exp = target.Parse("Test[\"k1\"]", typeof(string), parameters);
            Assert.AreEqual("k1value", target.Eval(exp, parameters));

            exp = target.Parse("Test.Name + \" \" + Test.DateOfBirth.Year.ToString()", typeof(string), parameters);
            Assert.AreEqual("Davide 1981", target.Eval(exp, parameters));

            exp = target.Parse("Test.SayHello()", typeof(string), parameters);
            Assert.AreEqual("Hello Davide", target.Eval(exp, parameters));

            // Type mismatch
            TestHelper.Throws<ParseException>(() => target.Parse("Test.SayHello()", typeof(DateTime), parameters));
        }

        public class TestModule
        {
            public string Name { get { return "Davide"; } }
            public int Age { get { return 30; } }
            public DateTime DateOfBirth { get { return new DateTime(1981, 1, 1); } }

            public string SayHello()
            {
                return string.Format("Hello {0}", Name);
            }

            public string this[string key]
            {
                get { return key + "value"; }
            }
        }
    }
}
