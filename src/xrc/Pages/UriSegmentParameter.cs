using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace xrc.Pages
{
	public class UriSegmentParameter
	{
		readonly string _pattern;

		const string PREFIX_PATTERN = @"^(?<prefix>[\w\._\-\+]*)"; // match any prefix with letter, digits and .+-_ 
		const string PARAMETER_PATTERN = @"\{(?<name>\w+)(?<catchAll>_CATCH-ALL)?\}";  // match any parameter name with letter, digits and optional ending with _CATCH-ALL
		const string SUFFIX_PATTERN = @"(?<suffix>[\w\._\-\+]*)$"; // match any suffix with letter, digits and .+-_
		static Regex PARAMETER_NAME_REGEX = new Regex(PREFIX_PATTERN + PARAMETER_PATTERN + SUFFIX_PATTERN, RegexOptions.Compiled | RegexOptions.IgnoreCase);

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

		public bool IsParameter
		{
			get { return _parameterName != null; }
		}

		public UriSegmentMatchResult Match(string url)
		{
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");

			var match = _segmentRegEx.Match(url);

			if (match.Success)
			{
				string value = match.Groups["value"].Value ?? string.Empty;
				value = HttpUtility.UrlDecode(value);
				string segment = match.Groups["segment"].Value ?? string.Empty;
				string nextPart = match.Groups["nextPart"].Value;
				if (nextPart != null)
				{
					if (nextPart.Length == 0)
						nextPart = null;
				}
				return new UriSegmentMatchResult(true, ParameterName, value, segment, nextPart);
			}
			else
				return new UriSegmentMatchResult(false, null, null, null, null);
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
				endPattern = "(/(?<nextPart>.*)|$)";
			}
			else
			{
				_parameterName = match.Groups["name"].Value;

				prefix = match.Groups["prefix"].Value;
				suffix = match.Groups["suffix"].Value;
				if (match.Groups["catchAll"].Success)
				{
					valuePattern = ".+?"; // match any character lazy
					endPattern = "(?<nextPart>/?$)";
				}
				else
				{
					valuePattern = "[^/]+?"; // match any character except / one or more time lazy
					endPattern = "(/(?<nextPart>.*)|$)";
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
		readonly string _currentUrlPart;
		readonly string _nextUrlPart;

		public UriSegmentMatchResult(bool success, string parameterName, 
							string parameterValue, string currentUrlPart, 
							string nextPart)
		{
			_success = success;
			_parameterName = parameterName;
			_parameterValue = parameterValue;
			_currentUrlPart = currentUrlPart;
			_nextUrlPart = nextPart;
		}

		public bool Success
		{
			get { return _success; }
		}
		public string ParameterName
		{
			get { return _parameterName; }
		}
		public bool IsParameter
		{
			get { return _parameterName != null; }
		}
		public string ParameterValue
		{
			get { return _parameterValue; }
		}
		public string CurrentUrlPart
		{
			get { return _currentUrlPart; }
		}
		public string NextUrlPart 
		{
			get { return _nextUrlPart; }
		}
		public bool HasNext
		{
			get { return _nextUrlPart != null; }
		}
	}
}
