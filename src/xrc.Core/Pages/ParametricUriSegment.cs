﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace xrc.Pages
{
	public class ParametricUriSegment
	{
		const string PREFIX_PATTERN = @"^(?<prefix>[\w\._\-\+]*)"; // match any prefix with letter, digits and .+-_ 
		const string PARAMETER_PATTERN = @"\{(?<name>\w+)(?<catchAll>\.\.\.)?\}";  // match any parameter name with letter, digits and optional ending with "..."
		const string SUFFIX_PATTERN = @"(?<suffix>[\w\._\-\+]*)$"; // match any suffix with letter, digits and .+-_
		static Regex PARAMETER_NAME_REGEX = new Regex(PREFIX_PATTERN + PARAMETER_PATTERN + SUFFIX_PATTERN, RegexOptions.Compiled | RegexOptions.IgnoreCase);

		const string VALUE_CATCH_ALL = ".+"; // match any character one or more times
		const string VALUE_CATCH_SEGMENT = "[^/]+"; // match any character except / one or more times

		readonly Regex _segmentRegEx;
		readonly ParametricUriSegmentExpression _paramExpression;

		public ParametricUriSegment(string expression)
		{
			if (string.IsNullOrEmpty(expression))
				throw new ArgumentNullException("expression");

            if (expression.Contains('/'))
                throw new ArgumentException("Uri segment expression cannot contains '/' characters.");

			_paramExpression = ParseExpression(expression);

			_segmentRegEx = CreateRegex(_paramExpression);
		}

		public string Expression
		{
			get { return _paramExpression.Expression; }
		}

		public string ParameterName
		{
			get { return _paramExpression.ParameterName; }
		}

		public bool IsParametric
		{
			get { return _paramExpression.IsParametric; }
		}

		public int FixedCharacters
		{
			get { return _paramExpression.FixedCharacters; }
		}

		public ParametricUriSegmentResult Match(string url)
		{
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");

			var match = _segmentRegEx.Match(url);

			if (match.Success)
			{
				string value = match.Groups["value"].Value ?? string.Empty;
				value = HttpUtility.UrlDecode(value);
				value = UriExtensions.RemoveTrailingSlash(value);
				string segment = match.Groups["segment"].Value ?? string.Empty;
				segment = UriExtensions.RemoveTrailingSlash(segment);
				string nextPart = match.Groups["nextPart"].Value;
				if (nextPart != null && nextPart.Length == 0)
					nextPart = null;

				return new ParametricUriSegmentResult(true, ParameterName, value, segment, nextPart);
			}
			else
				return new ParametricUriSegmentResult(false, null, null, null, null);
		}

		public string BuildSegmentUrl(string paramValue)
		{
			if (_paramExpression.IsParametric)
				return string.Format("{0}{1}{2}", _paramExpression.Prefix, paramValue, _paramExpression.Suffix);
			else
				return _paramExpression.Expression;
		}

		internal class ParametricUriSegmentExpression
		{
			public string Expression;
			public bool IsParametric;
			public string ParameterName;
			public string Prefix;
			public string Suffix;
			public bool CatchAll;

			public int FixedCharacters
			{
				get 
				{
					if (IsParametric)
						return Length(Prefix) + Length(Suffix);
					else
						return Length(Expression);
				}
			}

			private int Length(string value)
			{
				return value == null ? 0 : value.Length;
			}
		}

		static ParametricUriSegmentExpression ParseExpression(string expression)
		{
			var paramExpression = new ParametricUriSegmentExpression() { Expression = expression };

			var match = PARAMETER_NAME_REGEX.Match(expression);
			if (!match.Success)
			{
				paramExpression.IsParametric = false;
				paramExpression.ParameterName = null;
				paramExpression.Prefix = null;
				paramExpression.Suffix = null;
				paramExpression.CatchAll = false;
			}
			else
			{
				paramExpression.IsParametric = true;
				paramExpression.ParameterName = match.Groups["name"].Value;
				paramExpression.Prefix = match.Groups["prefix"].Value;
				paramExpression.Suffix = match.Groups["suffix"].Value;
				paramExpression.CatchAll = match.Groups["catchAll"].Success;
			}

			return paramExpression;
		}

		static Regex CreateRegex(ParametricUriSegmentExpression paramExpression)
		{
			if (paramExpression.IsParametric)
				return CreateParametricRegex(paramExpression);
			else
				return CreateFixedRegex(paramExpression);
		}

		static Regex CreateParametricRegex(ParametricUriSegmentExpression paramExpression)
		{
			string valuePattern;
			if (paramExpression.CatchAll)
				valuePattern = VALUE_CATCH_ALL;
			else
				valuePattern = VALUE_CATCH_SEGMENT;

			return CreateBaseRegex(paramExpression.Prefix, valuePattern, paramExpression.Suffix, paramExpression.CatchAll);
		}

		static Regex CreateFixedRegex(ParametricUriSegmentExpression paramExpression)
		{
			string valuePattern = Regex.Escape(paramExpression.Expression);

			return CreateBaseRegex(paramExpression.Prefix, valuePattern, paramExpression.Suffix, paramExpression.CatchAll);
		}

		static Regex CreateBaseRegex(string prefix, string valuePattern, string suffix, bool catchAll)
		{
			string regExPattern = string.Format(@"^/?(?<segment>{0}(?<value>{1}){2})(/(?<nextPart>.*))?$",
											RegexEscape(prefix), valuePattern, RegexEscape(suffix));

			return new Regex(regExPattern, RegexOptions.IgnoreCase);
		}

		static string RegexEscape(string value)
		{
			if (value == null)
				return null;
			else
				return Regex.Escape(value);
		}
	}
}
