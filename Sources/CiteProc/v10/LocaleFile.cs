using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents a locale file. Each locale file contains localization data for a single language dialect.
    /// </summary>
    [XmlRoot("locale", Namespace = File.NAMESPACE_URI)]
    public class LocaleFile : File, ILocale
    {
        public LocaleFile()
            : base(new Version(1, 0, 1))
        {
        }

        /// <summary>
        /// Returns an array of all currently available locale files.
        /// </summary>
        public static LocaleFile[] Defaults
        {
            get
            {
                return typeof(Processor).Assembly
                    .GetManifestResourceNames()
                    .Where(x => x.StartsWith("CiteProc.v10.Locales"))
                    .Select(x =>
                    {
                        using (var stream = typeof(Processor).Assembly.GetManifestResourceStream(x))
                        {
                            return File.Load<LocaleFile>(stream);
                        }
                    })
                    .Select(l =>
                    {
                        // fix bad naming convention
                        if (l.XmlLang == "en-US")
                        {
                            l.XmlLang = "";
                        }
                        else if (l.XmlLang == "de-DE")
                        {
                            l.XmlLang = "de";
                        }
                        else if (l.XmlLang == "pt-PT")
                        {
                            l.XmlLang = "pt";
                        }
                        else if (l.XmlLang == "zh-CN")
                        {
                            l.XmlLang = "zh";
                        }
                        else
                        {
                            // split
                            var parts = l.XmlLang.Split('-');
                            if (parts.Length == 2 && parts[0].Length == 2 && string.Compare(parts[0], parts[1], true) == 0)
                            {
                                // default
                                l.XmlLang = parts[0].ToLower();
                            }
                        }

                        // done
                        return l;
                    })
                    .OrderBy(x => x.XmlLang)
                    .ToArray();
            }
        }

        /// <summary>
        /// The locale code of this locale.
        /// </summary>
        [XmlAttribute("xml:lang")]
        public string XmlLang
        {
            get;
            set;
        }

        /// <summary>
        /// The cs:info element may be used to give metadata on the locale file.
        /// </summary>
        [XmlElement("info")]
        public LocaleInfoElement Info
        {
            get;
            set;
        }

        /// <summary>
        /// Style options of the locale.
        /// </summary>
        [XmlElement("style-options")]
        public StyleOptionsElement StyleOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Two localized date formats can be defined with cs:date elements: a “numeric” (e.g. “12-15-2005”) and a “text” format.
        /// </summary>
        [XmlElement("date")]
        public DateElement[] Dates
        {
            get;
            set;
        }
        /// <summary>
        /// Terms are localized strings (e.g. by using the “and” term, “Doe and Smith” automatically becomes “Doe und Smith”
        /// when the style locale is switched from English to German). Terms are either directly defined in the content of cs:term,
        /// or, in cases where singular and plural variants are needed (e.g. “page” and “pages”), in the content of the child
        /// elements cs:single and cs:multiple, respectively.
        /// </summary>
        [XmlArray("terms")]
        [XmlArrayItem("term")]
        public TermElement[] Terms
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a clone of this locale file.
        /// </summary>
        /// <returns></returns>
        public new LocaleFile Clone()
        {
            return (LocaleFile)base.Clone();
        }
    }
}