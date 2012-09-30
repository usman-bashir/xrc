using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages
{
	public class UriSegmentParameter
	{
		readonly string _pattern;

		const string PREFIX_PATTERN = @"^(?<prefix>[\w\._\-\+]*)"; // match any prefix with letter, digits and .+-_ 
		const string PARAMETER_PATTERN = @"\{(?<name>\w+)(?<catchAll>%)?\}";  // match any parameter name with letter, digits and optional ending with %
		const string SUFFIX_PATTERN = @"(?<suffix>[\w\._\-\+]*)$"; // match any suffix with letter, digits and .+-_
		static Regex PARAMETER_NAME_REGEX = new Regex(PREFIX_PATTERN + PARAMETER_PATTERN + SUFFIX_PATTERN, RegexOptions.Compiled);

		string _parameterName;
		string _parameterPattern;
		Regex _segmentRegEx;

		public UriSegmentParameter(string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
				throw new ArgumentNullException("pattern");

			_pattern = pattern;
			ParsePattern();

			_segmentRegEx = new Regex(_parameterPattern, RegexOptions.IgnoreCase);
		}

		public string Pattern
		{
			get { return _pattern; }
		}

		public string ParameterName
		{
			get { return _parameterName; }
		}

		public UriSegmentMatchResult Match(string url)
		{
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");

			var match = _segmentRegEx.Match(url);

			if (match.Success)
			{
				string value = match.Groups["value"].Value ?? string.Empty;
				value = value.ToLowerInvariant();
				string segment = match.Groups["segment"].Value ?? string.Empty;
				segment = segment.ToLowerInvariant();
				return new UriSegmentMatchResult(true, ParameterName, value, segment);
			}
			else
				return new UriSegmentMatchResult(false, null, null, null);
		}

		void ParsePattern()
		{
			_parameterName = null;

			string prefix;
			string suffix;
			string valuePattern;
			string endPattern;
			var match = PARAMETER_NAME_REGEX.Match(_pattern);
			if (!match.Success)
			{
				valuePattern = Regex.Escape(_pattern);
				prefix = string.Empty;
				suffix = string.Empty;
				endPattern = "(/|$)";
			}
			else
			{
				_parameterName = match.Groups["name"].Value;

				prefix = match.Groups["prefix"].Value;
				suffix = match.Groups["suffix"].Value;
				if (match.Groups["catchAll"].Success)
				{
					valuePattern = ".+"; // match all
					endPattern = "$";
				}
				else
				{
					valuePattern = "[^/]+?"; // match any character except / one or more time lazy
					endPattern = "(/|$)";
				}
			}

			_parameterPattern = string.Format(@"^/?(?<segment>{0}(?<value>{1}){2}){3}",
										RegexEscape(prefix), valuePattern, RegexEscape(suffix), endPattern);
		}

		static string RegexEscape(string value)
		{
			if (value == null)
				return null;
			else
				return Regex.Escape(value);
		}
	}

	public class UriSegmentMatchResult
	{
		readonly bool _success;
		readonly string _parameterName;
		readonly string _parameterValue;
		readonly string _segment;

		public UriSegmentMatchResult(bool success, string parameterName, string parameterValue, string segment)
		{
			_success = success;
			_parameterName = parameterName;
			_parameterValue = parameterValue;
			_segment = segment;
		}

		public bool Success
		{
			get { return _success; }
		}
		public string ParameterName
		{
			get { return _parameterName; }
		}
		public string ParameterValue
		{
			get { return _parameterValue; }
		}
		public string Segment
		{
			get { return _segment; }
		}
	}
}
