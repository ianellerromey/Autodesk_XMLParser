using System.Text;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Parsers
    {
      class XMLContentParser : XMLTextParser
      {
        public string Parse(string text, ref int index)
        {
          var contentBuilder = new StringBuilder();

          while (PeekChar(text, ref index) != c_openingBracket)
          {
            var currChar = ReadChar(text, ref index);
          }

          return contentBuilder.ToString();
        }
      }
    }
  }
}