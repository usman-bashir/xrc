using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace xrc.Pages
{
	public class ParametricUriSegmentResult
	{
		readonly bool _success;
		readonly string _parameterName;
		readonly string _parameterValue;
		readonly string _currentUrlPart;
		readonly string _nextUrlPart;

		public ParametricUriSegmentResult(bool success, string parameterName, 
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
