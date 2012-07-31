using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DynamicExpression;
using xrc.Modules;
using System.Linq.Expressions;

namespace xrc.SiteManager
{

    [TestClass()]
    public class MashupScriptService_Test
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

        // TODO Da rivedere questi metodi di test (parse e eval)
        //[TestMethod()]
        //public void It_should_be_possible_to_eval_script_with_modules()
        //{
        //    var scriptService = new Mocks.ScriptServiceMock("Eval_mock");
        //    var moduleFactory = new Moq.Mock<Modules.IModuleFactory>();
        //    IContext context = new Mocks.ContextMock();
        //    TestModule testModule = new TestModule();
        //    moduleFactory.Setup(p => p.Get(TestModule.Definition, context)).Returns(testModule);
        //    MashupPage page = new MashupPage();
        //    page.Modules.Add(new ModuleDefinition("TestModule", TestModule.Definition));

        //    MashupScriptService target = new MashupScriptService(scriptService, moduleFactory.Object);
        //    XValue exp;

        //    exp = target.Parse("@TestModule.Name", typeof(string), page);

        //    Assert.AreEqual("Eval_mock", target.Eval(exp, page, context));

        //    moduleFactory.Verify(p => p.Get(TestModule.Definition, context));
        //    moduleFactory.Verify(p => p.Release(testModule));
        //}

        [TestMethod()]
        public void It_should_be_possible_to_eval_constant()
        {
            MashupScriptService target = new MashupScriptService(null, null);
            IContext context = new Mocks.ContextMock();
            ModuleDefinitionList modulesDefinition = new ModuleDefinitionList();
            MashupParameterList mashupParameters = new MashupParameterList();
            Dictionary<string, IModule> modules = new Dictionary<string, IModule>();
            ContextParameterList parameters = new ContextParameterList();

            XValue exp;

            exp = target.Parse("ciao", typeof(string), modulesDefinition, mashupParameters);
            Assert.AreEqual("ciao", target.Eval(exp, modules, parameters));

            exp = target.Parse("459", typeof(int), modulesDefinition, mashupParameters);
            Assert.AreEqual(459, target.Eval(exp, modules, parameters));

            exp = target.Parse("0.59", typeof(double), modulesDefinition, mashupParameters);
            Assert.AreEqual(0.59, target.Eval(exp, modules, parameters));

            exp = target.Parse("01/02/2045", typeof(DateTime), modulesDefinition, mashupParameters);
            Assert.AreEqual(new DateTime(2045, 01, 02), target.Eval(exp, modules, parameters));
        }

		[TestMethod()]
		public void It_Should_be_possible_to_ExtractScript()
		{
            MashupScriptService target = new MashupScriptService(null, null);

			string expression;
			bool valid;

			valid = target.TryExtractScript("@prova", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("prova", expression);

			valid = target.TryExtractScript("@test.html(\"ciao\") ", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("test.html(\"ciao\")", expression);

			valid = target.TryExtractScript(" @test.html()", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("test.html()", expression);

			valid = target.TryExtractScript("@new Player() { FirstName = \"John\", LastName = \"Wayne\" }", out expression);
			Assert.AreEqual(true, valid);
			Assert.AreEqual("new Player() { FirstName = \"John\", LastName = \"Wayne\" }", expression);

			valid = target.TryExtractScript("#test.html()", out expression);
			Assert.AreEqual(false, valid);
			valid = target.TryExtractScript("{@test.html()}", out expression);
			Assert.AreEqual(false, valid);
            valid = target.TryExtractScript("", out expression);
            Assert.AreEqual(false, valid);
            valid = target.TryExtractScript(null, out expression);
            Assert.AreEqual(false, valid);
        }

        public class TestModule : IModule
        {
            public static ComponentDefinition Definition = new ComponentDefinition("TestModule", typeof(TestModule));

            public string Name { get { return "Davide"; } }
        }
    }
}
