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
    /// Represents a part of a date - day, month or year.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class DatePartElement : FormattingElement, IStripPeriods
    {
        /// <summary>
        /// Specifies the name or the part = day, month or year.
        /// </summary>
        [XmlAttribute("name")]
        public DatePartName Name
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the format of this date part.
        /// </summary>
        [XmlAttribute("form")]
        public DatePartFormat Format
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool FormatSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the delimiter used for rendering a date range. When a date range is rendered, the range delimiter is
        /// drawn from the cs:date-part element matching the largest date part (“year”, “month”, or “day”) that
        /// differs between the two dates.
        /// </summary>
        [XmlAttribute("range-delimiter")]
        public string RangeDelimiter
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'range-delimiter' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool RangeDelimiterSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When set to “true” (“false” is the default), any periods in the rendered text are removed.
        /// </summary>
        [XmlAttribute("strip-periods")]
        public bool StripPeriods
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'strip-periods' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool StripPeriodsSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When specified, converts the rendered date part to the given case.
        /// </summary>
        [XmlAttribute("text-case")]
        public TextCase TextCase
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'text-case' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool TextCaseSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles this DatePart.
        /// </summary>
        /// <param name="method"></param>
        internal void Compile(MethodInvoke method)
        {
            // allowed formats?
            if (this.FormatSpecified)
            {
                // init
                var allowed = true;

                // allowed?
                switch (this.Name)
                {
                    case DatePartName.Day:
                        allowed = (this.Format == DatePartFormat.Numeric || this.Format == DatePartFormat.NumericLeadingZeros || this.Format == DatePartFormat.Ordinal);
                        break;
                    case DatePartName.Month:
                        allowed = (this.Format == DatePartFormat.Long || this.Format == DatePartFormat.Short || this.Format == DatePartFormat.Numeric || this.Format == DatePartFormat.NumericLeadingZeros);
                        break;
                    case DatePartName.Year:
                        allowed = (this.Format == DatePartFormat.Long || this.Format == DatePartFormat.Short);
                        break;
                }

                // done
                if (!allowed)
                {
                    throw new CompilerException(this, "Format '{0}' is not allowed for '{1}' date-parts.", this.Format, this.Name);
                }
            }

            // strip periods allowed?
            if (this.StripPeriodsSpecified && this.Name != DatePartName.Month)
            {
                throw new CompilerException(this, "StripPeriods is only allowed for 'Month' date-parts.");
            }

            // name
            method.AddElement(this);
            method.AddCode(method.ParameterName);
            method.AddLiteral(this.Name);

            // format
            if (this.FormatSpecified)
            {
                // specified format
                method.AddLiteral(this.Format);
            }
            else
            {
                // use default
                switch (this.Name)
                {
                    case DatePartName.Day:
                        method.AddLiteral(DatePartFormat.Numeric);
                        break;
                    case DatePartName.Month:
                        method.AddLiteral(DatePartFormat.Long);
                        break;
                    case DatePartName.Year:
                        method.AddLiteral(DatePartFormat.Long);
                        break;
                    default:
                        throw new CompilerException("Invalid date-part name.");
                }
            }

            // affixes
            method.AddLiteral(this.Prefix);
            method.AddLiteral(this.Suffix);
            method.AddLiteral(this.TextCaseSpecified ? (object)this.TextCase : null);
            method.AddDefaultParameters();
        }

    }
}
