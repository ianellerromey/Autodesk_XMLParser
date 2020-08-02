using System;
using System.Collections.Generic;

namespace Autodesk
{
  namespace XMLParser
  {
    namespace Parsers
    {
      abstract class XMLTextParser
      {
        protected const char c_openingBracket = '<';
        protected const char c_closingBracket = '>';
        protected const char c_equal = '=';
        protected const char c_singleQuote = '\'';
        protected const char c_doubleQuote = '"';
        protected const char c_slash = '/';
        protected readonly HashSet<char> c_validIdentifierCharacters = new HashSet<char>(
          new char [] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '_', '-', '.'
          }
        );

        protected char ReadChar(string text, ref int index)
        {
          if(index >= text.Length)
          {
            throw new InvalidOperationException("Attempted read of invalid XML");
          }

          return text[index++];
        }

        protected char PeekChar(string text, ref int index)
        {
            if(index >= text.Length)
            {
              throw new InvalidOperationException("Attempted read of invalid XML");
            }

            return text[index];
        }

        protected void WalkWhitespace(string text, ref int index)
        {
          while (char.IsWhiteSpace(PeekChar(text, ref index)))
          {
            ReadChar(text, ref index);
          }
        }
      }
    }
  }
}