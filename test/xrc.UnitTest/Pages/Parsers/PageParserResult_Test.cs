﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using xrc.Modules;

namespace xrc.Pages.Parsers
{
	[TestClass]
	public class PageParserResult_Test
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void It_Should_be_possible_to_union_a_result_actions()
        {
			var target = new PageDefinition();
			target.Actions.Add(new PageAction("GET"));
			target.Actions.Add(new PageAction("POST"));

			var other = new PageDefinition();
			other.Actions.Add(new PageAction("GET"));
			other.Actions.Add(new PageAction("DELETE"));

			var result = target.Combine(other);

			Assert.AreEqual(3, result.Actions.Count);
			Assert.AreNotEqual(target.Actions["GET"], result.Actions["GET"]);
			Assert.AreEqual(other.Actions["GET"], result.Actions["GET"]);
		}

		[TestMethod]
		public void It_Should_be_possible_to_union_a_result_modules()
		{
			var target = new PageDefinition();
			target.Modules.Add(new ModuleDefinition("m1", new ComponentDefinition("m1", typeof(string))));
			target.Modules.Add(new ModuleDefinition("m2", new ComponentDefinition("m2", typeof(string))));

			var other = new PageDefinition();
			other.Modules.Add(new ModuleDefinition("m1", new ComponentDefinition("m1", typeof(string))));
			other.Modules.Add(new ModuleDefinition("m3", new ComponentDefinition("m3", typeof(string))));

			var result = target.Combine(other);

			Assert.AreEqual(3, result.Modules.Count);
			Assert.AreNotEqual(target.Modules["m1"], result.Modules["m1"]);
			Assert.AreEqual(other.Modules["m1"], result.Modules["m1"]);
		}

		[TestMethod]
		public void It_Should_be_possible_to_union_a_result_parameters()
		{
			var target = new PageDefinition();
			target.Parameters.Add(new PageParameter("p1", new XValue(typeof(string), "v1")));
			target.Parameters.Add(new PageParameter("p2", new XValue(typeof(string), "v2")));

			var other = new PageDefinition();
			other.Parameters.Add(new PageParameter("p1", new XValue(typeof(string), "v1.1")));
			other.Parameters.Add(new PageParameter("p3", new XValue(typeof(string), "v3")));

			var result = target.Combine(other);

			Assert.AreEqual(3, result.Parameters.Count);
			Assert.AreNotEqual(target.Parameters["p1"], result.Parameters["p1"]);
			Assert.AreEqual(other.Parameters["p1"], result.Parameters["p1"]);
		}
    }
}
