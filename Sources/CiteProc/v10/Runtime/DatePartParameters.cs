using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class DatePartParameters : Parameters
        {
            public DatePartParameters(
                string tag,
                Parameters original,
                DatePartName name,
                DatePartFormat? format,
                string prefix,
                string suffix,
                TextCase? textCase,
                FontStyle? fontStyle = null,
                FontVariant? fontVariant = null,
                FontWeight? fontWeight = null,
                TextDecoration? textDecoration = null,
                VerticalAlign? verticalAlign = null,
                bool? stripPeriods = null,
                string namesDelimiter = null,
                And? and = null,
                string nameDelimiter = null,
                DelimiterBehavior? delimiterPrecedesEtAl = null,
                DelimiterBehavior? delimiterPrecedesLast = null,
                int? etAlMin = null,
                int? etAlUseFirst = null,
                int? etAlSubsequentMin = null,
                int? etAlSubsequentUseFirst = null,
                bool? etAlUseLast = null,
                NameFormat? nameFormat = null,
                bool? initialize = null,
                string initializeWith = null,
                NameSortOptions? nameAsSortOrder = null,
                string sortSeparator = null
            )
                : base(original, fontStyle, fontVariant, fontWeight, textDecoration, verticalAlign, stripPeriods, namesDelimiter, and, nameDelimiter, delimiterPrecedesEtAl, delimiterPrecedesLast, etAlMin, etAlUseFirst, etAlSubsequentMin, etAlSubsequentUseFirst, etAlUseLast, nameFormat, initialize, initializeWith, nameAsSortOrder, sortSeparator)
            {
                // init
                this.Tag = tag;
                this.Name = name;
                this.Format = format;
                this.Prefix = prefix;
                this.Suffix = suffix;
                this.TextCase = textCase;
            }

            public string Tag
            {
                get;
                private set;
            }

            public DatePartName Name
            {
                get;
                private set;
            }
            public DatePartFormat? Format
            {
                get;
                private set;
            }
            public string Prefix
            {
                get;
                private set;
            }
            public string Suffix
            {
                get;
                private set;
            }
            public TextCase? TextCase
            {
                get;
                private set;
            }
        }
    }
}
