using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies conditions on the position of a cite in a citation.
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// “first”: position of cites that are the first to reference an item.
        /// </summary>
        [XmlEnum("first")]
        First,
        /// <summary>
        /// Cites referencing previously cited items, for which either
        /// a. the current cite immediately follows on another cite, within the same citation, that references the same item, or
        /// b. the current cite is the first cite in the citation, and the previous citation consists of a single cite referencing the same item
        /// and the item either has no locator or the locator is the same as the previously cited item.
        /// </summary>
        [XmlEnum("ibid")]
        Ibid,
        /// <summary>
        /// Cites referencing previously cited items, for which either
        /// a. the current cite immediately follows on another cite, within the same citation, that references the same item, or
        /// b. the current cite is the first cite in the citation, and the previous citation consists of a single cite referencing the same item
        /// and the item has a different locator from the previously cited item.
        /// </summary>
        [XmlEnum("ibid-with-locator")]
        IbidWithLocator,
        /// <summary>
        /// Cites referencing previously cited items have the “subsequent” position.
        /// </summary>
        [XmlEnum("subsequent")]
        Subsequent,
        /// <summary>
        /// “near-note”: position of a cite following another cite referencing the same item. Both cites have to be located
        /// in foot or endnotes, and the distance between both cites may not exceed the maximum distance (measured in number
        /// of foot or endnotes) set with the near-note-distance option.
        /// </summary>
        [XmlEnum("near-note")]
        NearNote
    }
}
