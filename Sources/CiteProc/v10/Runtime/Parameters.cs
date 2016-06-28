using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiteProc.Formatting;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class Parameters
        {
            public Parameters(
                Parameters original,
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
            {
                // formatting
                this.FontStyle = fontStyle ?? original.FontStyle;
                this.FontVariant = fontVariant ?? original.FontVariant;
                this.FontWeight = fontWeight ?? original.FontWeight;
                this.TextDecoration = textDecoration ?? original.TextDecoration;
                this.VerticalAlign = verticalAlign ?? original.VerticalAlign;

                // strip periods
                this.StripPeriods = stripPeriods ?? original.StripPeriods;

                // names
                this.NamesDelimiter = namesDelimiter ?? original.NamesDelimiter;

                // name
                this.And = and ?? original.And;
                this.NameDelimiter = nameDelimiter ?? original.NameDelimiter;
                this.DelimiterPrecedesEtAl = delimiterPrecedesEtAl ?? original.DelimiterPrecedesEtAl;
                this.DelimiterPrecedesLast = delimiterPrecedesLast ?? original.DelimiterPrecedesLast;

                this.EtAlMin = etAlMin ?? original.EtAlMin;
                this.EtAlUseFirst = etAlUseFirst ?? original.EtAlUseFirst;
                this.EtAlSubsequentMin = etAlSubsequentMin ?? original.EtAlSubsequentMin;
                this.EtAlSubsequentUseFirst = etAlSubsequentUseFirst ?? original.EtAlSubsequentUseFirst;
                this.EtAlUseLast = etAlUseLast ?? original.EtAlUseLast;

                this.NameFormat = nameFormat ?? original.NameFormat;
                this.Initialize = initialize ?? original.Initialize;
                this.InitializeWith = initializeWith ?? (original == null ? null : original.InitializeWith);
                this.NameAsSortOrder = nameAsSortOrder ?? original.NameAsSortOrder;
                this.SortSeparator = sortSeparator ?? original.SortSeparator;
            }
            internal Parameters()
                : this(
                    null,
                    FontStyle.Normal,
                    FontVariant.Normal,
                    FontWeight.Normal,
                    TextDecoration.None,
                    VerticalAlign.Baseline,
                    false,
                    "; ",
                    And.Delimiter,
                    ", ",
                    DelimiterBehavior.Contextual,
                    DelimiterBehavior.Contextual,
                    int.MaxValue,
                    1,
                    int.MaxValue,
                    1,
                    false,
                    NameFormat.Long,
                    true,
                    null,
                    NameSortOptions.None,
                    ", "
                )
            {
            }

            public FontStyle FontStyle
            {
                get;
                private set;
            }
            public FontVariant FontVariant
            {
                get;
                private set;
            }
            public FontWeight FontWeight
            {
                get;
                private set;
            }
            public TextDecoration TextDecoration
            {
                get;
                private set;
            }
            public VerticalAlign VerticalAlign
            {
                get;
                private set;
            }

            public bool StripPeriods
            {
                get;
                private set;
            }

            public string NamesDelimiter
            {
                get;
                private set;
            }

            public And And
            {
                get;
                private set;
            }
            public string NameDelimiter
            {
                get;
                private set;
            }
            public DelimiterBehavior DelimiterPrecedesEtAl
            {
                get;
                private set;
            }
            public DelimiterBehavior DelimiterPrecedesLast
            {
                get;
                private set;
            }

            public int EtAlMin
            {
                get;
                private set;
            }
            public int EtAlUseFirst
            {
                get;
                private set;
            }
            public int EtAlSubsequentMin
            {
                get;
                private set;
            }
            public int EtAlSubsequentUseFirst
            {
                get;
                private set;
            }
            public bool EtAlUseLast
            {
                get;
                private set;
            }

            public NameFormat NameFormat
            {
                get;
                private set;
            }
            public bool Initialize
            {
                get;
                private set;
            }
            public string InitializeWith
            {
                get;
                private set;
            }

            public NameSortOptions NameAsSortOrder
            {
                get;
                private set;
            }
            public string SortSeparator
            {
                get;
                private set;
            }

            public IEnumerable<TextRun> GenerateText(string text)
            {
                // null
                if (!string.IsNullOrEmpty(text))
                {
                    // replace single quote
                    text = text.Replace('\'', '’');

                    // strip periods?
                    if (this.StripPeriods)
                    {
                        text = text.Replace(".", "");
                    }

                    // replace superscript characters
                    if (this.VerticalAlign == VerticalAlign.Baseline)
                    {
                        // split and replace
                        var results = SuperscriptHelper.SplitAndReplace(text)
                            .Select((x, i) =>
                            {
                                return this.GetTextRun(x, ((i % 2) == 0 ? VerticalAlign.Baseline : VerticalAlign.Superscript));
                            })
                            .Where(x => !string.IsNullOrEmpty(x.Text));

                        // done
                        foreach (var result in results)
                        {
                            yield return result;
                        }
                    }
                    else
                    {
                        // return single
                        yield return this.GetTextRun(text, this.VerticalAlign);
                    }
                }
            }
            private TextRun GetTextRun(string text, VerticalAlign verticalAlign)
            {
                // cast
                return new TextRun(text, this.Map(this.FontStyle), this.Map(this.FontVariant), this.Map(this.FontWeight), this.Map(this.TextDecoration), this.Map(verticalAlign));
            }
            private CiteProc.Formatting.FontStyle Map(FontStyle style)
            {
                switch (style)
                {
                    case v10.FontStyle.Italic:
                        return CiteProc.Formatting.FontStyle.Italic;
                    case v10.FontStyle.Normal:
                        return CiteProc.Formatting.FontStyle.Normal;
                    case v10.FontStyle.Oblique:
                        return CiteProc.Formatting.FontStyle.Oblique;
                    default:
                        throw new NotSupportedException();
                }
            }
            private CiteProc.Formatting.FontVariant Map(FontVariant variant)
            {
                switch (variant)
                {
                    case v10.FontVariant.Normal:
                        return CiteProc.Formatting.FontVariant.Normal;
                    case v10.FontVariant.SmallCaps:
                        return CiteProc.Formatting.FontVariant.SmallCaps;
                    default:
                        throw new NotSupportedException();
                }
            }
            private CiteProc.Formatting.FontWeight Map(FontWeight weight)
            {
                switch (weight)
                {
                    case v10.FontWeight.Normal:
                        return CiteProc.Formatting.FontWeight.Normal;
                    case v10.FontWeight.Bold:
                        return CiteProc.Formatting.FontWeight.Bold;
                    case v10.FontWeight.Light:
                        return CiteProc.Formatting.FontWeight.Light;
                    default:
                        throw new NotSupportedException();
                }
            }
            private CiteProc.Formatting.TextDecoration Map(TextDecoration decoration)
            {
                switch (decoration)
                {
                    case v10.TextDecoration.None:
                        return CiteProc.Formatting.TextDecoration.None;
                    case v10.TextDecoration.Underline:
                        return CiteProc.Formatting.TextDecoration.Underline;
                    default:
                        throw new NotSupportedException();
                }
            }
            private CiteProc.Formatting.VerticalAlign Map(VerticalAlign valign)
            {
                switch (valign)
                {
                    case v10.VerticalAlign.Baseline:
                        return CiteProc.Formatting.VerticalAlign.Baseline;
                    case v10.VerticalAlign.Subscript:
                        return CiteProc.Formatting.VerticalAlign.Subscript;
                    case v10.VerticalAlign.Superscript:
                        return CiteProc.Formatting.VerticalAlign.Superscript;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}