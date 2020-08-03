using System.Text;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Parsers
    {
      class XMLContentParser : XMLTextParser
      {
        public static string Parse(string text, ref int index)
        {
          var contentBuilder = new StringBuilder();

          while (PeekChar(text, ref index) != c_openingBracket)
          {
            contentBuilder.Append(ReadChar(text, ref index));
          }

          return contentBuilder.ToString().Trim();
        }
      }
    }
  }
}