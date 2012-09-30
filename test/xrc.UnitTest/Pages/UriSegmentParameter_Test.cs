using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xrc.Pages
{
	[TestClass]
	public class UriSegmentParameter_Test
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullPattern()
		{
			new UriSegmentParameter(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EmptyPattern()
		{
			new UriSegmentParameter(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullUrl()
		{
			var target = new UriSegmentParameter("test");
			target.Match(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EmptyUrl()
		{
			var target = new UriSegmentParameter("test");
			target.Match(string.Empty);
		}

		[TestMethod]
		public void Fixed_Segment()
		{
			string pattern = "segment1";
			var target = new UriSegmentParameter(pattern);
			Assert.AreEqual(pattern, target.Pattern);
			Assert.IsNull(target.ParameterName);

			UriSegmentMatchResult result;

			result = target.Match("/segment1");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue);
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);

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

			result = target.Match("segment1/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue);
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("SEGMENT1/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.AreEqual("segment1", result.ParameterValue);
			Assert.AreEqual("segment1", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("other/segment1/test/index");
			Assert.AreEqual(false, result.Success);
			Assert.IsNull(result.ParameterName);
			Assert.IsNull(result.ParameterValue);
			Assert.IsNull(result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Segment()
		{
			string pattern = "{param1}";
			var target = new UriSegmentParameter(pattern);
			Assert.AreEqual(pattern, target.Pattern);
			Assert.AreEqual("param1", target.ParameterName);

			var result = target.Match("/italia/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("italia/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("ITALIA/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("Francia/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("francia", result.ParameterValue);
			Assert.AreEqual("francia", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("Francia.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("francia.html", result.ParameterValue);
			Assert.AreEqual("francia.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Segment_With_SuffixPart()
		{
			string pattern = "{param1}.test";
			var target = new UriSegmentParameter(pattern);
			Assert.AreEqual(pattern, target.Pattern);
			Assert.AreEqual("param1", target.ParameterName);

			var result = target.Match("/italia.TEST/test/index");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia.test", result.CurrentUrlPart);
			Assert.AreEqual("test/index", result.NextUrlPart);

			result = target.Match("ITALIA.test/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia", result.ParameterValue);
			Assert.AreEqual("italia.test", result.CurrentUrlPart);
			Assert.AreEqual("test/index.html", result.NextUrlPart);

			result = target.Match("Francia.html.test");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("francia.html", result.ParameterValue);
			Assert.AreEqual("francia.html.test", result.CurrentUrlPart);
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
			var target = new UriSegmentParameter(pattern);
			Assert.AreEqual(pattern, target.Pattern);
			Assert.AreEqual("param1", target.ParameterName);

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
			string pattern = "{param1%}";
			var target = new UriSegmentParameter(pattern);
			Assert.AreEqual(pattern, target.Pattern);
			Assert.AreEqual("param1", target.ParameterName);

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

			result = target.Match(".html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual(".html", result.ParameterValue);
			Assert.AreEqual(".html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Segment_With_CatchAll_and_Suffix()
		{
			string pattern = "{param1%}.html";
			var target = new UriSegmentParameter(pattern);
			Assert.AreEqual(pattern, target.Pattern);
			Assert.AreEqual("param1", target.ParameterName);

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
			Assert.AreEqual(false, result.Success);

			result = target.Match("italia.html/test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia.html/test/index", result.ParameterValue);
			Assert.AreEqual("italia.html/test/index.html", result.CurrentUrlPart);
			Assert.IsNull(result.NextUrlPart);
		}

		[TestMethod]
		public void Parameter_Special_Characters()
		{
			string pattern = "_+-.{param1}_+-.";
			var target = new UriSegmentParameter(pattern);
			Assert.AreEqual(pattern, target.Pattern);
			Assert.AreEqual("param1", target.ParameterName);

			var result = target.Match("_+-.italia.html+francia_svezia-germania_+-./test/index.html");
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual("param1", result.ParameterName);
			Assert.AreEqual("italia.html+francia_svezia-germania", result.ParameterValue);
			Assert.AreEqual("_+-.italia.html+francia_svezia-germania_+-.", result.CurrentUrlPart);
		}
	}
}
