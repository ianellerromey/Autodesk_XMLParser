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
        private enum XMLElementState { Unclosed, Closed };

        public static XMLElement Parse(string text, ref int index)
        {
          XMLElement element = new XMLElement();

          var state = ParseOpeningTagAndAttributes(text, ref index, element);

          if (state != XMLElementState.Closed)
          {
            ParseContentAndChildrenAndClosingTag(text, ref index, element);
          }

          return element;
        }
        
        private static XMLElementState ParseOpeningTagAndAttributes(string text, ref int index, XMLElement element)
        {
          if (ReadChar(text, ref index) != c_openingBracket)
          {
            throw new ArgumentException($"Invalid character encountered at index {index}");
          }

          var parsingState = XMLElementState.Unclosed;
          var tagNameBuilder = new StringBuilder();

          while (true)
          {
            var currChar = ReadChar(text, ref index);

            if (currChar == c_slash)
            {
              // The tag has been parsed, walk to the closing bracket

              WalkToEndOfElement(text, ref index);

              parsingState = XMLElementState.Closed;
            }
           else if (currChar == c_closingBracket)
            {
              // The tag has been parsed completely

              element.Tag = tagNameBuilder.ToString();

              return parsingState;
            }
            else if (char.IsWhiteSpace(currChar))
            {
              // The tag has been parsed, but there might be attributes

              WalkWhitespace(text, ref index);
              
              ParseAttributes(text, ref index, element);
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

        private static XMLElementState ParseContentAndChildrenAndClosingTag(string text, ref int index, XMLElement element)
        {
          while (true)
          {
            WalkWhitespace(text, ref index);

            if (PeekChar(text, ref index) == c_openingBracket)
            {
              // An opening bracket could be a closing tag or a child element

              var indexPlus = index + 1;

              if (PeekChar(text, ref indexPlus) == c_slash)
              {
                ParseClosingTag(text, ref index, element);

                return XMLElementState.Closed;
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

        private static void ParseAttributes(string text, ref int index, XMLElement element)
        {
          while (PeekChar(text, ref index) != c_closingBracket && PeekChar(text, ref index) != c_slash)
          {
            var attribute = XMLAttributeParser.Parse(text, ref index);

            element.Attributes.Add(attribute.Item1, attribute.Item2);

            WalkWhitespace(text, ref index);
          }
        }

        private static void ParseContent(string text, ref int index, XMLElement element)
        {
          var content = XMLContentParser.Parse(text, ref index);

          if (element.Content == null)
          {
            element.Content = content;
          }
          else
          {
            element.Content += Environment.NewLine;
            element.Content += content;
          }
        }

        private static void ParseChild(string text, ref int index, XMLElement element)
        {
          var childElement = XMLElementParser.Parse(text, ref index);

          element.Children.Add(childElement);
        }

        private static void ParseClosingTag(string text, ref int index, XMLElement element)
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

          while (true)
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
              
              WalkToEndOfElement(text, ref index);
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

        private static void WalkToEndOfElement(string text, ref int index)
        {
          WalkWhitespace(text, ref index);

          if (PeekChar(text, ref index) != c_closingBracket)
          {
            throw new ArgumentException($"Invalid character encountered at index {index}");
          }
        }
      }
    }
  }
}