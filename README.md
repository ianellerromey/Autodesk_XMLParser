# Autodesk_XMLParser

This console application accepts as input any number of files containing either valid or invalid XML. This is a barebones document parser, which will:

* Build a tree of elements
* Parse attributes
* Parse mixed content fields

And will not:

* Accept documents prepended with a DTD
* Throw errors when encountering invalid/unescaped characters in content fields