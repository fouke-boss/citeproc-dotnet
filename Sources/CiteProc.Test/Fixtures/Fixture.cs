using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CiteProc.Compilation;
using CiteProc.v10;

namespace CiteProc.Test.Fixtures
{
    public class Fixture
    {
        private Fixture()
        {
        }
        public static Fixture Load(Stream stream)
        {
            // read lines
            var lines = Fixture.ReadLines(stream).ToArray();

            // find first lines
            var groups = lines
                .Select((l, i) => new
                {
                    Index = i,
                    IsStart = (l.Replace(" ", "").StartsWith(">>==") && l.Replace(" ", "").EndsWith("==>>")),
                    IsEnd = (l.Replace(" ", "").StartsWith("<<==") && l.Replace(" ", "").EndsWith("==<<")),
                    Part = (l.Length > 8 ? l.Substring(4, l.Length - 8).Trim(' ', '=') : string.Empty)
                })
                .Where(x => x.IsStart || x.IsEnd)
                .GroupBy(x => x.Part)
                .ToArray();

            // init
            var result = new Fixture();

            // parse
            foreach (var group in groups)
            {
                // find lines
                var start = group.Single(x => x.IsStart);
                var end = group.Single(x => x.IsEnd);
                var block = string.Join(Environment.NewLine, lines
                    .Skip(start.Index + 1)
                    .Take(end.Index - start.Index - 1));

                // switch
                switch (group.Key.ToUpper())
                {
                    case "CSL":
                        result.Style = File.Parse<StyleFile>(block);
                        break;
                    case "INPUT":
                        result.Items = Input.LoadJson(block);
                        break;
                    case "MODE":
                        switch (block.ToUpper())
                        {
                            case "BIBLIOGRAPHY":
                                result.Mode = Mode.Bibliography;
                                break;
                            case "CITATION":
                                result.Mode = Mode.Citation;
                                break;
                            default:
                                throw new FeatureNotSupportedException(string.Format("test-mode-{0}", block.ToUpper()));
                        }
                        break;
                    case "RESULT":
                        result.ExpectedResult = block;
                        break;
                    default:
                        throw new FeatureNotSupportedException(string.Format("test-{0}", group.Key.ToUpper()));
                }
            }

            // done
            return result;
        }
        private static IEnumerable<string> ReadLines(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public StyleFile Style
        {
            get;
            set;
        }
        public IDataProvider[] Items
        {
            get;
            set;
        }
        public Mode Mode
        {
            get;
            set;
        }
        public string ExpectedResult
        {
            get;
            set;
        }

        public string Execute()
        {
            // init
            var result = string.Empty;

            // compile
            var processor = this.Style.Compile();

            // mode?
            switch (this.Mode)
            {
                case Fixtures.Mode.Citation:
                    // generate
                    var citation = processor.GenerateCitation(this.Items);

                    // done
                    result = (citation == null || citation.IsEmpty ? "[CSL STYLE ERROR: reference with no printed form.]" : citation.ToHtml().Replace("&amp;", "&#38;"));
                    break;
                case Fixtures.Mode.Bibliography:
                    // init
                    var builder = new StringBuilder(65536);
                    var settings = new XmlWriterSettings()
                    {
                        ConformanceLevel = ConformanceLevel.Document,
                        Indent = true,
                        OmitXmlDeclaration = true
                    };

                    // render
                    using (var xw = XmlWriter.Create(builder, settings))
                    {
                        // <div class="csl-bib-body">
                        xw.WriteStartElement("div");
                        xw.WriteAttributeString("class", "csl-bib-body");

                        // items
                        foreach (var run in processor.GenerateBibliography(this.Items))
                        {
                            // <div class="csl-entry">
                            xw.WriteStartElement("div");
                            xw.WriteAttributeString("class", "csl-entry");

                            // render
                            xw.WriteRaw(run.ToHtml().Replace("&amp;", "&#38;"));

                            // done
                            xw.WriteEndElement();
                        }

                        // done
                        xw.WriteEndElement();
                    }

                    // done
                    result = builder.ToString();
                    break;
            }

            // done
            return result;
        }
    }
}