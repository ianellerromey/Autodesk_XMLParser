using System;
using System.Text;
using Autodesk.XMLParser.Model;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Parsers
    {
      class XMLElementParser : XMLTextParser
      {
        public XMLElement Parse(string text, ref int index)
        {
          XMLElement element = new XMLElement();

          ParseOpeningTagAndAttributes(text, ref index, element);
          ParseContentAndChildrenAndClosingTag(text, ref index, element);

          return element;
        }
        
        private void ParseOpeningTagAndAttributes(string text, ref int index, XMLElement element)
        {
          if (ReadChar(text, ref index) != c_openingBracket)
          {
            throw new ArgumentException($"Invalid character encountered at index {index}");
          }

          var tagNameBuilder = new StringBuilder();

          while(true)
          {
            var currChar = ReadChar(text, ref index);

            if (currChar == c_slash)
            {
              // The tag has been parsed, walk to the closing bracket

              WalkToEndOfElement(text, ref index);
            }
           else if (currChar == c_closingBracket)
            {
              // The tag has been parsed completely

              element.Tag = tagNameBuilder.ToString();

              return;
            }
            else if (char.IsWhiteSpace(currChar))
            {
              // The tag has been parsed, but there might be attributes
              
              ParseAttribute(text, ref index, element);
            }
            else if (c_validIdentifierCharacters.Contains(currChar))
            {
              // The tag is still parsing

              tagNameBuilder.Append(currChar);
            }
            else
            {
              throw new ArgumentException($"Invalid character encountered at index {index}");
            }
          }
        }

        private void ParseAttribute(string text, ref int index, XMLElement element)
        {
          WalkWhitespace(text, ref index);

          while(PeekChar(text, ref index) != c_closingBracket)
          {
            var parser = new XMLAttributeParser();

            var attribute = parser.Parse(text, ref index);

            element.Attributes.Add(attribute.Item1, attribute.Item2);

            WalkWhitespace(text, ref index);
          }
        }

        private void ParseContentAndChildrenAndClosingTag(string text, ref int index, XMLElement element)
        {
          while(true)
          {
            WalkWhitespace(text, ref index);

            if(PeekChar(text, ref index) == c_openingBracket)
            {
              // An opening bracket could be a closing tag or a child element

              var indexPlus = index + 1;

              if(PeekChar(text, ref indexPlus) == c_slash)
              {
                ParseClosingTag(text, ref index, element);

                return;
              }
              else
              {
                ParseChild(text, ref index, element);
              }
            }
            else
            {
              // This is just pure content

              ParseContent(text, ref index, element);
            }
          }
        }

        private void ParseContent(string text, ref int index, XMLElement element)
        {
          var parser = new XMLContentParser();

          var content = parser.Parse(text, ref index);

          if(element.Content == null)
          {
            element.Content = content;
          }
          else
          {
            element.Content += Environment.NewLine;
            element.Content += content;
          }
        }

        private void ParseChild(string text, ref int index, XMLElement element)
        {
          var parser = new XMLElementParser();

          var childElement = parser.Parse(text, ref index);

          element.Children.Add(childElement);
        }

        private void ParseClosingTag(string text, ref int index, XMLElement element)
        {
          if (ReadChar(text, ref index) != c_openingBracket)
          {
            throw new ArgumentException($"Invalid character encountered at index {index}");
          }
          
          if (ReadChar(text, ref index) != c_slash)
          {
            throw new ArgumentException($"Invalid character encountered at index {index}");
          }

          var tagNameBuilder = new StringBuilder();

          while(true)
          {
            var currChar = ReadChar(text, ref index);

            if (currChar == c_closingBracket)
            {
              // The tag has been parsed completely

              var tag = tagNameBuilder.ToString();

              if (tag != element.Tag)
              {
                throw new ArgumentException($"Invalid format, tag mismatch: {element.Tag} does not match {tag}");
              }

              return;
            }
            else if (char.IsWhiteSpace(currChar))
            {
              // The tag has been parsed, walk to the closing bracket
              
              WalkWhitespace(text, ref index);
            }
            else if (c_validIdentifierCharacters.Contains(currChar))
            {
              // The tag is still parsing

              tagNameBuilder.Append(currChar);
            }
            else
            {
              throw new ArgumentException($"Invalid character encountered at index {index}");
            }
          }
        }

        private void WalkToEndOfElement(string text, ref int index)
        {
          WalkWhitespace(text, ref index);

          if(PeekChar(text, ref index) != c_closingBracket)
          {
            throw new ArgumentException($"Invalid character encountered at index {index}");
          }
        }
      }
    }
  }
}