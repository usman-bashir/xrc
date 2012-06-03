using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DynamicExpression;
using xrc.Modules;

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
            ScriptService target = new ScriptService(new Mocks.ModuleFactoryMock(new TestModule()));

            Modules.ModuleDefinitionList modules = new Modules.ModuleDefinitionList();
            modules.Add(new ModuleDefinition("Test", TestModule.Definition));
			IScriptExpression exp;

			exp = target.Parse("\"ciao\"", modules, typeof(string));
			Assert.AreEqual("ciao", target.Eval(exp, null));
            TestHelper.Throws<ParseException>(() => target.Parse("\"ciao\"", modules, typeof(int)));

            exp = target.Parse("Test.Name", modules, typeof(string));
            Assert.AreEqual("Davide", target.Eval(exp, null));

            exp = target.Parse("Test[\"k1\"]", modules, typeof(string));
            Assert.AreEqual("k1value", target.Eval(exp, null));

            exp = target.Parse("Test.Name + \" \" + Test.DateOfBirth.Year.ToString()", modules, typeof(string));
            Assert.AreEqual("Davide 1981", target.Eval(exp, null));

            exp = target.Parse("Test.SayHello()", modules, typeof(string));
            Assert.AreEqual("Hello Davide", target.Eval(exp, null));

            TestHelper.Throws<ParseException>(() => target.Parse("Test.SayHello()", modules, typeof(DateTime)));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_script_context()
        {
            ScriptService target = new ScriptService(new Mocks.ModuleFactoryMock(null));

            Modules.ModuleDefinitionList modules = new Modules.ModuleDefinitionList();
            IScriptExpression exp;

            exp = target.Parse("Context.WorkingPath", modules, typeof(string));
            Assert.AreEqual("testValue", target.Eval(exp, new ContextMock()));
        }

		[TestMethod()]
		public void It_Should_be_possible_to_ExtractInlineScript()
		{
			ScriptService target = new ScriptService(new Mocks.ModuleFactoryMock(null));

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

        public class TestModule : IModule
        {
            public static ComponentDefinition Definition = new ComponentDefinition("TestModule", typeof(TestModule));

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

        public class ContextMock : IContext
        {
            public System.Web.HttpRequestBase Request
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public System.Web.HttpResponseBase Response
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public Configuration.ISiteConfiguration Configuration
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string WorkingPath
            {
                get
                {
                    return "testValue";
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public SiteManager.MashupFile File
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public Dictionary<string, string> Parameters
            {
                get { throw new NotImplementedException(); }
            }

            public SiteManager.MashupPage Page
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public RenderSlotEventHandler SlotCallback
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string GetAbsoluteUrl(string url)
            {
                throw new NotImplementedException();
            }

            public string GetAbsoluteFile(string file)
            {
                throw new NotImplementedException();
            }
        }
    }
}
