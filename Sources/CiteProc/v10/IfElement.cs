using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CiteProc.Compilation;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents a cs:if or cs:else-if conditional branch inside a cs:choose element.
    /// </summary>
    public class IfElement : ElseElement
    {
        /// <summary>
        /// When set to “true” (the only allowed value), the element content is only rendered if it disambiguates two otherwise
        /// identical citations. This attempt at disambiguation is only made when all other disambiguation methods have failed
        /// to uniquely identify the target source.
        /// </summary>
        [XmlAttribute("disambiguate")]
        public bool Disambiguate
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'disambiguate' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DisambiguateSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Tests whether the given variable(s) contain numeric content. Content is considered numeric if it solely consists
        /// of numbers. Numbers may have prefixes and suffixes (“D2”, “2b”, “L2d”), and may be separated by a comma, hyphen,
        /// or ampersand, with or without spaces (“2, 3”, “2-4”, “2 & 4”). For example, “2nd” tests “true” whereas “second”
        /// and “2nd edition” test “false”.
        /// </summary>
        [XmlAttribute("is-numeric")]
        public string IsNumeric
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the given date variables contain approximate dates.
        /// </summary>
        [XmlAttribute("is-uncertain-date")]
        public string IsUncertainDate
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the locator matches the given locator types (see Locators). Use “sub-verbo” to test for the “sub verbo” locator type.
        /// </summary>
        [XmlAttribute("locator")]
        public string Locator
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the cite position matches the given positions (terminology: citations consist of one or more cites to individual items). When called within the scope of cs:bibliography, position tests “false”.
        /// </summary>
        [XmlAttribute("position")]
        public string Position
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the item matches the given types.
        /// </summary>
        [XmlAttribute("type")]
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the default (long) forms of the given variables contain non-empty values.
        /// </summary>
        [XmlAttribute("variable")]
        public string Variable
        {
            get;
            set;
        }

        /// <summary>
        /// Controls the testing logic.
        /// </summary>
        [XmlAttribute("match")]
        public Match Match
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'match' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool MatchSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles this If element.
        /// </summary>
        /// <param name="code"></param>
        internal void Compile(Scope code)
        {
            // variable
            var byVariable = (this.Variable ?? "")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(v => string.Format("{0}.IsNotNull({1})", Compiler.CONTEXT_NAME, Compiler.GetLiteral(v)))
                .ToArray();

            // is_numeric
            var byIsNumeric = (this.IsNumeric ?? "")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(v => string.Format("{0}.IsNumeric({1})", Compiler.CONTEXT_NAME, Compiler.GetLiteral(v)))
                .ToArray();

            // type
            var byType = (this.Type ?? "")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(v => string.Format("{0}.IsType({1})", Compiler.CONTEXT_NAME, Compiler.GetLiteral(v)))
                .ToArray();

            // uncertain date
            var byUncertainDate = (this.IsUncertainDate ?? "")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(v => string.Format("{0}.IsUncertainDate({1})", Compiler.CONTEXT_NAME, Compiler.GetLiteral(v)))
                .ToArray();

            // locator
            var byLocator = (this.Locator ?? "")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(v => string.Format("{0}.IsLocator({1})", Compiler.CONTEXT_NAME, Compiler.GetLiteral(v)))
                .ToArray();

            // position
            var byPosition = (this.Position ?? "")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Enum.Parse(typeof(v10.Position), x))
                .Cast<v10.Position>()
                .Select(v => string.Format("{0}.IsPosition({1})", Compiler.CONTEXT_NAME, Compiler.GetLiteral(v)))
                .ToArray();

            // conditions
            var conditions = byVariable
                .Concat(byIsNumeric)
                .Concat(byType)
                .Concat(byUncertainDate)
                .Concat(byLocator)
                .Concat(byPosition)
                .ToArray();

            // valid?
            if (conditions.Length == 0)
            {
                throw new CompilerException(this, "No conditions.");
            }

            // position not yet supported
            if (byPosition.Length > 0)
            {
                throw new FeatureNotSupportedException("position condition");
            }

            // disambiguate not yet supported
            if (this.DisambiguateSpecified)
            {
                throw new FeatureNotSupportedException("disambiguate condition");
            }

            // match
            var delimiter = string.Empty;
            var prefix = string.Empty;
            switch (this.MatchSpecified ? this.Match : Match.All)
            {
                case Match.All:
                    delimiter = " && ";
                    break;
                case Match.Any:
                    delimiter = " || ";
                    break;
                case Match.None:
                    delimiter = " && ";
                    prefix = "!";
                    break;
            }

            // render
            code.Append("({0})", string.Join(delimiter, conditions.Select(x => string.Format("{0}{1}", prefix, x))));

            // render children
            base.Compile(code);
            code.AppendLineBreak();
        }
    }
}
