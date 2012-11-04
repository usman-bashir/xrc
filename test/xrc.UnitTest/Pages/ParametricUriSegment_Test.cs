using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xrc.Pages
{
	[TestClass]
	public class ParametricUriSegment_Test
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullPattern()
		{
			new ParametricUriSegment(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EmptyPattern()
		{
			new ParametricUriSegment(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullUrl()
		{
			var target = new ParametricUriSegment("test");
			target.Match(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EmptyUrl()
		{
			var target = new ParametricUriSegment("test");
			target.Match(string.Empty);
		}

		[TestMethod]
		public void Fixed_Segment()
		{
			string pattern = "segment1";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.IsNull(target.ParameterName);
			Assert.IsFalse(target.IsParametric);
			Assert.AreEqual(8, target.FixedCharacters);
			Assert.AreEqual("segment1", target.BuildSegmentUrl("p1"));

			ParametricUriSegmentResult result;

			result = target.Match("/segment1");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue);
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
			Assert.IsFalse(result.IsParameter);

			result = target.Match("segment1");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue);
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("segment1/test.html");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue);
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.AreEqual("test.html", result.NextUrlPart);

			result = target.Match("/segment1/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue); 
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);
			Assert.IsTrue(result.HasNext);

			result = target.Match("segment1/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue);
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);
			Assert.IsTrue(result.HasNext);

			result = target.Match("SEGMENT1/test/Index");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("SEGMENT1", result.ParameterValue);
			Assert.AreEqual("SEGMENT1", result.CurrentUrlPart);
			Assert.AreEqual("test/Index", result.NextUrlPart);

			result = target.Match("other/segment1/test/index");
			Assert.AreEqual(false, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.IsNull(result.ParameterValue);
			Assert.IsNull(result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
			Assert.IsFalse(result.HasNext);
		}

		[TestMethod]
		public void Parameter_Segment()
		{
			string pattern = "{param1}";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);
			Assert.IsTrue(target.IsParametric);
			Assert.AreEqual(0, target.FixedCharacters);
			Assert.AreEqual("p1", target.BuildSegmentUrl("p1"));

			var result = target.Match("/italia/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);
			Assert.IsTrue(result.IsParameter);

			result = target.Match("italia/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("ITALIA/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("ITALIA", result.ParameterValue);
			Assert.AreEqual("ITALIA", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("Francia/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("Francia", result.ParameterValue);
			Assert.AreEqual("Francia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("Francia.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("Francia.html", result.ParameterValue);
			Assert.AreEqual("Francia.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Segment_With_SuffixPart()
		{
			string pattern = "{param1}.test";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);
			Assert.AreEqual(5, target.FixedCharacters);
			Assert.AreEqual("p1.test", target.BuildSegmentUrl("p1"));

			var result = target.Match("/italia.TEST/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia.TEST", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("ITALIA.test/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("ITALIA", result.ParameterValue);
			Assert.AreEqual("ITALIA.test", result.CurrentUrlPart);
			Assert.AreEqual("test/index.html", result.NextUrlPart);

			result = target.Match("Francia.html.test");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("Francia.html", result.ParameterValue);
			Assert.AreEqual("Francia.html.test", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("italia/test/index");
			Assert.AreEqual(false, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.IsNull(result.ParameterValue);
			Assert.IsNull(result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("italia.testt/test/index");
			Assert.AreEqual(false, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.IsNull(result.ParameterValue);
			Assert.IsNull(result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match(".test/test/index");
			Assert.AreEqual(false, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.IsNull(result.ParameterValue);
			Assert.IsNull(result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Segment_With_PrefixPart()
		{
			string pattern = "test{param1}";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);
			Assert.AreEqual(4, target.FixedCharacters);
			Assert.AreEqual("testp1", target.BuildSegmentUrl("p1"));

			var result = target.Match("/test.italia/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual(".italia", result.ParameterValue);
			Assert.AreEqual("test.italia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("test/test/index");
			Assert.AreEqual(false, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.IsNull(result.ParameterValue);
			Assert.IsNull(result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Segment_With_CatchAll()
		{
			string pattern = "{param1_CATCH-ALL}";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);
			Assert.AreEqual(0, target.FixedCharacters);
			Assert.AreEqual("p1", target.BuildSegmentUrl("p1"));

			var result = target.Match("/italia/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia/test/index.html", result.ParameterValue);
			Assert.AreEqual("italia/test/index.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("italia/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia/test/index.html", result.ParameterValue);
			Assert.AreEqual("italia/test/index.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("italia.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia.html", result.ParameterValue);
			Assert.AreEqual("italia.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("/test/");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("test", result.ParameterValue);
			Assert.AreEqual("test", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match(".html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual(".html", result.ParameterValue);
			Assert.AreEqual(".html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("/news/sport/basket/2012");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("news/sport/basket/2012", result.ParameterValue);
			Assert.AreEqual("news/sport/basket/2012", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("/news/sport/basket/2012/");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("news/sport/basket/2012", result.ParameterValue);
			Assert.AreEqual("news/sport/basket/2012", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
			
		}

		[TestMethod]
		public void Parameter_Segment_With_CatchAll_LowerCase()
		{
			string pattern = "{param1_catch-all}";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);
			Assert.AreEqual("p1", target.BuildSegmentUrl("p1"));

			var result = target.Match("/italia/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia/test/index.html", result.ParameterValue);
			Assert.AreEqual("italia/test/index.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("italia/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia/test/index.html", result.ParameterValue);
			Assert.AreEqual("italia/test/index.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Segment_With_CatchAll_and_Suffix()
		{
			string pattern = "{param1_CATCH-ALL}.html";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);
			Assert.AreEqual(5, target.FixedCharacters);
			Assert.AreEqual("p1.html", target.BuildSegmentUrl("p1"));

			var result = target.Match("/italia/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia/test/index", result.ParameterValue);
			Assert.AreEqual("italia/test/index.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("italia/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia/test/index", result.ParameterValue);
			Assert.AreEqual("italia/test/index.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("italia.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match(".html");
			Assert.AreEqual(false, result.Success);

			result = target.Match("italia.htm");
			Assert.AreEqual(false, result.Success);

			result = target.Match("italia.html/test");
			Assert.AreEqual(true, result.Success);

			result = target.Match("part1.html/test/part2.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("part1.html/test/part2", result.ParameterValue);
			Assert.AreEqual("part1.html/test/part2.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

			result = target.Match("test/index.html/next/part/");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("test/index", result.ParameterValue);
			Assert.AreEqual("test/index.html", result.CurrentUrlPart);
			Assert.AreEqual("next/part/", result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Special_Characters_in_prefix_suffix()
		{
			string pattern = "_+-.{param1}_+-.";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);
			Assert.AreEqual(8, target.FixedCharacters);
			Assert.AreEqual("_+-.p1_+-.", target.BuildSegmentUrl("p1"));

			var result = target.Match("_+-.italia.html_+-./test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia.html", result.ParameterValue);
			Assert.AreEqual("_+-.italia.html_+-.", result.CurrentUrlPart);
		}

		[TestMethod]
		public void Parameter_Special_Characters_in_parameter_segment()
		{
			string pattern = "{param1}";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual("param1", target.ParameterName);

			var result = target.Match("italia+francia/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia francia", result.ParameterValue);
			Assert.AreEqual("italia+francia", result.CurrentUrlPart);

			// All characters must be supported
			result = target.Match("%23%24%25%26%3C%42%7B%7D/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("#$%&<B{}", result.ParameterValue);
			Assert.AreEqual("%23%24%25%26%3C%42%7B%7D", result.CurrentUrlPart);
		}

		[TestMethod]
		public void Parameter_Special_Characters_in_fixed_segment()
		{
			string pattern = "_+-.TEST_+-.";
			var target = new ParametricUriSegment(pattern);
			Assert.AreEqual(pattern, target.Expression);
			Assert.AreEqual(pattern.Length, target.FixedCharacters);
			Assert.AreEqual("_+-.TEST_+-.", target.BuildSegmentUrl("p1"));

			var result = target.Match("_+-.TEST_+-./test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("_ -.TEST_ -.", result.ParameterValue);
			Assert.AreEqual("_+-.TEST_+-.", result.CurrentUrlPart);
		}
	}
}
