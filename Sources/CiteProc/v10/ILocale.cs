using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents localization data which can be included either in styles or in locale files (which conveniently provide sets of default
    /// localization data), consisting of terms, date formats and grammar options. Each locale element contains localization data for a single
    /// language dialect. This locale code is set on the required xml:lang attribute on the cs:locale root element.
    /// </summary>
    public interface ILocale
    {
        /// <summary>
        /// The locale code of this locale.
        /// </summary>
        string XmlLang
        {
            get;
        }
        /// <summary>
        /// Style options of the locale.
        /// </summary>
        StyleOptionsElement StyleOptions
        {
            get;
        }
        /// <summary>
        /// Two localized date formats can be defined with cs:date elements: a “numeric” (e.g. “12-15-2005”) and a “text” format.
        /// </summary>
        DateElement[] Dates
        {
            get;
        }
        /// <summary>
        /// Terms are localized strings (e.g. by using the “and” term, “Doe and Smith” automatically becomes “Doe und Smith”
        /// when the style locale is switched from English to German). Terms are either directly defined in the content of cs:term,
        /// or, in cases where singular and plural variants are needed (e.g. “page” and “pages”), in the content of the child
        /// elements cs:single and cs:multiple, respectively.
        /// </summary>
        TermElement[] Terms
        {
            get;
        }
    }
}
