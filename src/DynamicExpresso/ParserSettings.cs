using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DynamicExpresso
{
    public class ParserSettings
    {
        Dictionary<string, object> keywords;

        IDictionary<string, object> externals;

        CultureInfo _culture;

        public ParserSettings()
        {
            keywords = CreateKeywords();
            _culture = CultureInfo.InvariantCulture;
        }

        Dictionary<string, object> CreateKeywords()
        {
            Dictionary<string, object> d = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            d.Add("true", ParserConstants.trueLiteral);
            d.Add("false", ParserConstants.falseLiteral);
            d.Add("null", ParserConstants.nullLiteral);
            d.Add(ParserConstants.keywordIt, ParserConstants.keywordIt);
            d.Add(ParserConstants.keywordIif, ParserConstants.keywordIif);
            d.Add(ParserConstants.keywordNew, ParserConstants.keywordNew);
            foreach (Type type in ParserConstants.predefinedTypes) 
                d.Add(type.Name, type);
            return d;
        }

        public CultureInfo Culture
        {
            get { return _culture; }
        }

        public IDictionary<string, object> Externals
        {
            get { return externals; }
        }
        public IDictionary<string, object> Keywords
        {
            get { return keywords; }
        }
    }
}
