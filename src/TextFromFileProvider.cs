using System;
using System.IO;

namespace Autodesk
{
  namespace XMLParser
  {
    class TextFromFileProvider : ITextProvider
    {
      const string c_validExtension = ".xml";
      string m_fileName;

      public TextFromFileProvider(string fileName)
      {
        m_fileName = fileName;
      }

      public string GetText()
      {
        if(!File.Exists(m_fileName))
        {
          throw new FileNotFoundException("Provided file does not exist", m_fileName);
        }
        else if (Path.GetExtension(m_fileName) != c_validExtension)
        {
          throw new ArgumentOutOfRangeException(m_fileName, "Provided file must have an .xml extension");
        }

        return File.ReadAllText(m_fileName);
      }
    }
  }
}