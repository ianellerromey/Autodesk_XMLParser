using System;
using System.Text;
using Autodesk.XMLParser.Model;
using Autodesk.XMLParser.Parsers;

namespace Autodesk
{
  namespace XMLParser
  {
    class XMLTreeBuilder
    {

      public XMLElement BuildTree(ITextProvider textProvider)
      {
        var text = textProvider.GetText();
        var index = 0;
        var parser = new XMLTreeParser();

        return parser.Parse(text, ref index);
      }

    }
  }
}