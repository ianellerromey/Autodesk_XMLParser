using System;
using System.Text;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Parsers
    {
      class XMLAttributeParser : XMLTextParser
      {
        public Tuple<string, string> Parse(string text, ref int index)
        {
          var key = ParseKey(text, ref index); 
          WalkWhitespace(text, ref index);
          var value = ParseValue(text, ref index);

          return new Tuple<string, string>(key, value);
        }
        
        private string ParseKey(string text, ref int index)
        {
          var keyBuilder = new StringBuilder();

          while(true)
          {
            var currChar = ReadChar(text, ref index);

            if (currChar == c_equal)
            {
              // The key has been parsed

              return keyBuilder.ToString();
            }
            else if (char.IsWhiteSpace(currChar))
            {
              // The key has been parsed, but we need to walk to the delimiter

              WalkWhitespace(text, ref index);
            }
            else if (c_validIdentifierCharacters.Contains(currChar))
            {
              // The key is still parsing

              keyBuilder.Append(currChar);
            }
            else
            {
              throw new ArgumentException($"Invalid identifier character encountered at index {index}");
            }
          }
        }

        private string ParseValue(string text, ref int index)
        {
          var delimiter = ReadChar(text, ref index);

          if (delimiter != c_singleQuote && delimiter != c_doubleQuote)
          {
            throw new ArgumentException($"Invalid attribute value delimiter character encountered at index {index}");
          }

          var valueBuilder = new StringBuilder();

          while(true)
          {
            var currChar = ReadChar(text, ref index);

            if (currChar == delimiter)
            {
              // The value has been parsed

              return valueBuilder.ToString();
            }
            else
            {
              // The value is still parsing

              valueBuilder.Append(currChar);
            }
          }
        }
      }
    }
  }
}