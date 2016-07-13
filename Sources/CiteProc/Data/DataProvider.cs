using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    /// <summary>
    /// Represents a default implementation of the IDataProvider interface.
    /// </summary>
    public class DataProvider : IDataProvider
    {
        private Dictionary<string, IVariable> _Variables = new Dictionary<string, IVariable>();

        public DataProvider()
            : this(Culture.Invariant)
        {
        }
        public DataProvider(Culture culture)
        {
            // init
            this.Culture = culture;
        }

        public static IEnumerable<DataProvider> Parse(string value, DataFormat format)
        {
            using (var tr = new StringReader(value))
            {
                return Load(tr, format);
            }
        }
        public static IEnumerable<DataProvider> Load(string path, DataFormat format)
        {
            using (var tr = new StreamReader(path))
            {
                return Load(tr, format);
            }
        }
        public static IEnumerable<DataProvider> Load(Stream stream, DataFormat format)
        {
            using (var tr = new StreamReader(stream))
            {
                return Load(tr, format);
            }
        }
        public static IEnumerable<DataProvider> Load(TextReader reader, DataFormat format)
        {
            // parse
            return format.Parse(reader).ToArray();
        }

        public Culture Culture
        {
            get;
            set;
        }

        public IVariable this[string name]
        {
            get
            {
                return this._Variables[name.ToLower()];
            }
            set
            {
                this._Variables[name.ToLower()] = value;
            }
        }
        public Dictionary<string, IVariable> GetVariables()
        {
            return this._Variables;
        }
        public static Type GetVariableType(string name)
        {
            switch (name.ToLower())
            {
                case "abstract":
                case "annote":
                case "archive":
                case "archive_location":
                case "archive-place":
                case "authority":
                case "call-number":
                case "citation-label":
                case "collection-title":
                case "container-title":
                case "container-title-short":
                case "dimensions":
                case "doi":
                case "event":
                case "event-place":
                case "genre":
#warning Id is not an official CSL variable
                case "id":
                case "isbn":
                case "ISSN":
                case "jurisdiction":
                case "keyword":
                case "locator":
                case "medium":
                case "note":
                case "original-publisher":
                case "original-publisher-place":
                case "original-title":
                case "pmcid":
                case "pmid":
                case "publisher":
                case "publisher-place":
                case "references":
                case "reviewed-title":
                case "scale":
                case "section":
                case "source":
                case "status":
                case "title":
                case "title-short":
                case "type":
                case "url":
                case "version":
                    return typeof(ITextVariable);
                case "chapter-number":
                case "collection-number":
                case "edition":
                case "issue":
                case "page":
                case "number":
                case "number-of-pages":
                case "number-of-volumes":
                case "volume":
                    return typeof(INumberVariable);
                case "accessed":
                case "issued":
                case "container":
                case "event-date":
                case "original-date":
                case "submitted":
                    return typeof(IDateVariable);
                case "author":
                case "collection-editor":
                case "composer":
                case "container-author":
                case "director":
                case "editor":
                case "editorial-director":
                case "illustrator":
                case "interviewer":
                case "original-author":
                case "recipient":
                case "reviewed-author":
                case "translator":
                    return typeof(INamesVariable);
                default:
                    return null;
            }
        }
    }
}