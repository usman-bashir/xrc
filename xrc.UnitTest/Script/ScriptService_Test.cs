using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DynamicExpression;

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
        public void It_should_be_possible_to_eval_script()
        {
			ScriptService target = new ScriptService();

			Dictionary<string, Type> arguments = new Dictionary<string,Type>();
			arguments.Add("Person", typeof(Person));
			IScriptExpression exp;

			exp = target.Parse("\"ciao\"", arguments, typeof(string));
			Assert.AreEqual("ciao", exp.Eval(new object[]{null}));
            TestHelper.Throws<ParseException>(() => target.Parse("\"ciao\"", arguments, typeof(int)));

            var person = new Person() { Name = "Davide", DateOfBirth = new DateTime(1981, 1, 1) };

            exp = target.Parse("Person.Name", arguments, typeof(string));
            Assert.AreEqual("Davide", exp.Eval(person));

            exp = target.Parse("Person[\"k1\"]", arguments, typeof(string));
            Assert.AreEqual("k1value", exp.Eval(person));

            exp = target.Parse("Person.Name + \" \" + Person.DateOfBirth.Year.ToString()", arguments, typeof(string));
            Assert.AreEqual("Davide 1981", exp.Eval(person));

            exp = target.Parse("Person.SayHello()", arguments, typeof(string));
            Assert.AreEqual("Hello Davide", exp.Eval(person));

            TestHelper.Throws<ParseException>(() => target.Parse("Person.SayHello()", arguments, typeof(DateTime)));
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime DateOfBirth { get; set; }

            public string SayHello()
            {
                return string.Format("Hello {0}", Name);
            }

            public string this[string key]
            {
                get { return key + "value"; }
            }
        }

		[TestMethod()]
		public void It_Should_be_possible_to_ExtractInlineScript()
		{
			ScriptService target = new ScriptService();

			string expression;
			bool valid;

			valid = target.TryExtractInlineScript("@prova", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("prova", expression);

			valid = target.TryExtractInlineScript("@test.html(\"ciao\") ", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("test.html(\"ciao\")", expression);

			valid = target.TryExtractInlineScript(" @test.html()", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("test.html()", expression);

			valid = target.TryExtractInlineScript("@new Player() { FirstName = \"John\", LastName = \"Wayne\" }", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("new Player() { FirstName = \"John\", LastName = \"Wayne\" }", expression);

			valid = target.TryExtractInlineScript("#test.html()", out expression);
			Assert.AreEqual(false, valid);
			valid = target.TryExtractInlineScript("{@test.html()}", out expression);
			Assert.AreEqual(false, valid);
		}

    }
}
