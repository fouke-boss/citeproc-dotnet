# CiteProc-.NET

CiteProc-.NET is a [Citation Style Language](http://citationstyles.org/) (CSL)
v1.0.1 processor written in C#.

## Getting started
To start using CiteProc-.NET, create a new project in Visual Studio and add a reference to the CiteProc.dll assembly. Then start generating bibliographies and citations with just a few lines of code:

    public void Example()
    {
        // Load a style from disk (or use one of the overloads for reading
        // from a stream, a text reader or an xml reader).
        var style = CiteProc.File.Load("harvard-cite-them-right.csl");

        // Compile the style to get a processor instance.
        var processor = Processor.Compile(style);

        // Data of the referenced items (books, articles, etc.) is accessed through the
        // IDataProvider interface. CiteProc.NET comes with a default implementation of
        // this interface that supports the CSL JSON format. Currently this default
        // implementation is part of the CiteProc.Test assembly, but in the near future
        // it will become part of the CiteProc assembly itself.
        var data = CiteProc.Test.Fixtures.Input.LoadJson("items.json");

        // Now, you are ready to render citations and bibliographies using
        // the selected style:
        var entries = processor.GenerateBibliography(data);

        // The result is an instance of a CiteProc.Formatting.Run class. This instance
        // can then be converted to the desired format. CiteProc supports plain text
        // and HTML out-of-the-box; the CiteProc.WpfDemo project contains an example
        // of how to show the result in a WPF TextBlock. Other formats can be added
        // easily.
        
        // Austen, J. (1995) Pride and Prejudice. New York, NY: Dover Publications.
        var plainText = entries.First().ToPlainText();

        // Austen, J. (1995) <i>Pride and Prejudice</i>. New York, NY: Dover Publications.
        var html = entries.First().ToHtml();
    }

## Dependencies
The CiteProc.dll assembly targets .NET framework 4.5, although I'm guessing it could be made to target earlier versions without much recoding. CiteProc.dll does not depend on any third-party components, and only a small number of System assemblies:
* System.dll
* System.Core.dll
* System.Xml.dll

The only reference to a third-party component is a reference from the CiteProc.Test.dll to the Newtonsoft.Json.dll for parsing CSL JSON. Once I've replaced this with my own JSON parser, this functionality will be moved to the CiteProc.dll.

As it does not depend on any specific .NET language or UI technology, CiteProc-.NET can be used together with ASP.NET, WinForms, Windows Presentation Foundation, etc., in either C# or Visual Basic.NET. 

## CSL Compatibility
CiteProc-.NET aims to implement [version 1.0.1 of the CSL specification](http://docs.citationstyles.org/en/stable/specification.html), and currently supports the larger part of this specification. There is same more work ahead however, as the following features (marked in code by the FeatureNotSupportedException) are not yet supported (as of june 28, 2016):
* Bibliography-specific options
* Subsequent author substitution
* Disambiguation
* Cite grouping
* Cite collapsing
* Citation-specific options
* Display attribute
* Position and disambiguate conditions
* the substitute element inside a names element
* Generation of citation numbers
* Removing multiple spaces and punctuation
* (not part of the CSL spec): parsing of raw dates and roman numbers in CSL JSON
* (not part of the CSL spec): inline html in CSL JSON

## CiteProc.Test
CiteProc-.NET comes with a CiteProc.Test.exe assembly, which contains:
* A couple of *code tests*, for testing specific parts of the C# code.
* A set of *basic tests* (not yet complete) for systematically testing the processing of each individual element and attribute of the CSL specification.
* The *[CiteProc Test Suite](https://bitbucket.org/bdarcus/citeproc-test)* as provided by Frank Bennett, the author of citeproc-js.

Running the CiteProc.Test.exe shows the results of these tests in the console. It also generates a log file (in the same folder as the CiteProc.Test.exe), which contains additional info on the failing tests, including a summary, showing the unsupported features (and the number of test cases failing because of them).

Currently, 377 of the available 820 test cases pass, while 65 of them fail (some more than others). The remaining 378 test cases fail because of features that are not yet supported, 211 of which because of unsupported test sections like CITATIONS, CITATION-ITEMS and BIBENTRIES.

## CiteProc.WpfDemo
CiteProc-.NET also comes with a small WPF demo that mimics the [CSL style code editor](http://editor.citationstyles.org/codeEditor/). Currently, it's main goal is to show how to use CiteProc-.NET with Windows Presentation Foundation (WPF), but it might one day become a fully functional CSL editor.

Running this demo (by double clicking Binaries\CiteProc.WpfDemo.exe) gives you a first impression of the current capabilities and shortcomings of CiteProc-.NET.

## Roadmap
Now that this first version is made public, the following work remains to be done: 
* Implementing the remaining *unsupported features*, at least the ones that are part of the CSL specification.
* Completing the *basic test* set.
* Creating a *default implementation of the IDataProvider* interface in the CiteProc.dll assembly, which  supports JSON CSL, BibTeX and other formats.
* *Documentation, documentation, documentation*.
* Enhancing and expanding the *CiteProc.WpfDemo* project.