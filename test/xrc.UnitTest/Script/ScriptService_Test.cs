using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using xrc.Modules;
using System.Linq.Expressions;

namespace xrc.Script
{
    [TestClass()]
    public class ScriptService_Test
    {
        [TestMethod()]
        public void It_should_be_possible_to_eval_script_constant()
        {
            ScriptService target = new ScriptService();

            var parameters = new ScriptParameterList();

			IScriptExpression exp;

            exp = target.Parse("\"ciao\"", parameters);
            Assert.AreEqual(typeof(string), exp.ReturnType);
            Assert.AreEqual("ciao", target.Eval(exp, parameters));

            exp = target.Parse("95", parameters);
            Assert.AreEqual(typeof(int), exp.ReturnType);
            Assert.AreEqual(95, target.Eval(exp, parameters));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_script_system_method()
        {
            ScriptService target = new ScriptService();

            var parameters = new ScriptParameterList();
            parameters.Add(new ScriptParameter("name", typeof(string), "mondo"));

            IScriptExpression exp;

            exp = target.Parse("string.Format(\"ciao {0}\", name)", parameters);
            Assert.AreEqual(typeof(string), exp.ReturnType);
            Assert.AreEqual("ciao mondo", target.Eval(exp, parameters));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_script_constant_datetime()
        {
            ScriptService target = new ScriptService();

            IScriptExpression exp;

            exp = target.Parse("new DateTime(2012, 02, 05)", new ScriptParameterList());
            Assert.AreEqual(typeof(DateTime), exp.ReturnType);
            Assert.AreEqual(new DateTime(2012, 02, 05), target.Eval(exp, new ScriptParameterList()));
        }

        [TestMethod()]
        public void It_should_be_possible_to_eval_script_invoking_a_module()
        {
            ScriptService target = new ScriptService();

            ScriptParameterList parameters = new ScriptParameterList();
            parameters.Add(new ScriptParameter("Test", typeof(TestModule), new TestModule()));

            IScriptExpression exp;

            exp = target.Parse("Test.Name", parameters);
            Assert.AreEqual(typeof(string), exp.ReturnType);
            Assert.AreEqual("Davide", target.Eval(exp, parameters));

            exp = target.Parse("Test[\"k1\"]", parameters);
            Assert.AreEqual(typeof(string), exp.ReturnType);
            Assert.AreEqual("k1value", target.Eval(exp, parameters));

            exp = target.Parse("Test.Name + \" \" + Test.DateOfBirth.Year.ToString()", parameters);
            Assert.AreEqual(typeof(string), exp.ReturnType);
            Assert.AreEqual("Davide 1981", target.Eval(exp, parameters));

            exp = target.Parse("Test.SayHello()", parameters);
            Assert.AreEqual(typeof(string), exp.ReturnType);
            Assert.AreEqual("Hello Davide", target.Eval(exp, parameters));
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
