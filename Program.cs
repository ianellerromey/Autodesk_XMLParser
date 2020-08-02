﻿using System;
using System.Collections.Generic;
using Autodesk.XMLParser.Model;

namespace Autodesk
{
  namespace XMLParser
  {
    class Program
    {
      static void Main(string[] args)
      {
        var trees = new List<XMLElement>();
        var treeBuilder = new XMLTreeBuilder();

        foreach (var fileName in args)
        {
          try
          {
            var provider = new TextFromFileProvider(fileName);
            var tree = treeBuilder.BuildTree(provider);
            trees.Add(tree);
          }
          catch(Exception e)
          {
            Console.WriteLine($"Exception occurred in provided file {fileName}:");
            Console.WriteLine(e.Message);
          }
        }
      }
    }
  }
}
