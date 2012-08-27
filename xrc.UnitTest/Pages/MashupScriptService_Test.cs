using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DynamicExpression;
using xrc.Modules;
using System.Linq.Expressions;
using xrc.Script;

namespace xrc.Pages.Script
{

    [TestClass()]
    public class PageScriptService_Test
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
        public void It_should_be_possible_to_parse_and_eval_script_with_modules()
        {
            var scriptService = new Moq.Mock<IScriptService>();
            scriptService.Setup(p => p.Parse("TestModule.Name", typeof(string), Moq.It.IsAny<ScriptParameterList>())).Returns(new Mocks.ScriptExpressionMock("TestModule.Name", typeof(string)));
            TestModule testModule = new TestModule();

            ModuleDefinitionList modules = new ModuleDefinitionList();
            modules.Add(new ModuleDefinition("TestModule", TestModule.Definition));
			PageParameterList parameters = new PageParameterList();

			PageScriptService target = new PageScriptService(scriptService.Object);

            XValue exp;
            exp = target.Parse("@TestModule.Name", typeof(string), modules, parameters);

            scriptService.Verify(p => p.Parse("TestModule.Name", typeof(string), Moq.It.IsAny<ScriptParameterList>()));

            Dictionary<string, IModule> modulesInstance = new Dictionary<string, IModule>();
            modulesInstance.Add("TestModule", testModule);
            ContextParameterList parametersInstance = new ContextParameterList();

            target.Eval(exp, modulesInstance, parametersInstance);

            scriptService.Verify(p => p.Eval(Moq.It.IsAny<IScriptExpression>(), Moq.It.IsAny<ScriptParameterList>()));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_constant()
        {
			PageScriptService target = new PageScriptService(null);
            IContext context = new Mocks.ContextMock();
            ModuleDefinitionList modulesDefinition = new ModuleDefinitionList();
			PageParameterList pageParameters = new PageParameterList();
            Dictionary<string, IModule> modules = new Dictionary<string, IModule>();
            ContextParameterList parameters = new ContextParameterList();

            XValue exp;

			exp = target.Parse("ciao", typeof(string), modulesDefinition, pageParameters);
            Assert.AreEqual("ciao", target.Eval(exp, modules, parameters));

			exp = target.Parse("459", typeof(int), modulesDefinition, pageParameters);
            Assert.AreEqual(459, target.Eval(exp, modules, parameters));

			exp = target.Parse("0.59", typeof(double), modulesDefinition, pageParameters);
            Assert.AreEqual(0.59, target.Eval(exp, modules, parameters));

			exp = target.Parse("01/02/2045", typeof(DateTime), modulesDefinition, pageParameters);
            Assert.AreEqual(new DateTime(2045, 01, 02), target.Eval(exp, modules, parameters));
        }

		[TestMethod()]
		public void It_Should_be_possible_to_ExtractScript()
		{
			PageScriptService target = new PageScriptService(null);

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
