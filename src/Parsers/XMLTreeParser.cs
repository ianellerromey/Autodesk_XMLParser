using Autodesk.XMLParser.Model;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Parsers
    {
      class XMLTreeParser : XMLTextParser
      {
        public XMLElement Parse(string text, ref int index)
        {
          WalkWhitespace(text, ref index);

          var parser = new XMLElementParser();

          return parser.Parse(text, ref index);
        }
      }
    }
  }
}