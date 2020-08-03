using Autodesk.XMLParser.Model;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Parsers
    {
      class XMLTreeParser : XMLTextParser
      {
        public static XMLElement Parse(string text, ref int index)
        {
          WalkWhitespace(text, ref index);

          return XMLElementParser.Parse(text, ref index);
        }
      }
    }
  }
}