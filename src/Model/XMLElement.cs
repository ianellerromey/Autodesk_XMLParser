using System.Collections.Generic;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Model
    {
      class XMLElement
      {
        public string Tag
        {
          get;
          set;
        }
        
        public Dictionary<string, string> Attributes
        {
          get;
          private set;
        }

        public List<XMLElement> Children
        {
          get;
          private set;
        }

        public string Content
        {
          get;
          set;
        }

        public XMLElement()
        {
          Attributes = new Dictionary<string, string>();
          Children = new List<XMLElement>();
        }
      }
    }
  }
}