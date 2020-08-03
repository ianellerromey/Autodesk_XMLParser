using Autodesk.XMLParser.Model;
using Autodesk.XMLParser.Parsers;

namespace Autodesk
{
  namespace XMLParser
  {
    class XMLTreeBuilder
    {

      static public XMLElement BuildTree(ITextProvider textProvider)
      {
        var text = textProvider.GetText();
        var index = 0;

        return XMLTreeParser.Parse(text, ref index);
      }

    }
  }
}