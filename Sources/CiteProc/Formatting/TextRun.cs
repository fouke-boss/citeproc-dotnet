using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Formatting
{
    [System.Diagnostics.DebuggerDisplay("{Text}")]
    public sealed class TextRun : Run
    {
        internal TextRun(string text, FontStyle fontStyle, FontVariant fontVariant, FontWeight fontWeight, TextDecoration textDecoration, VerticalAlign verticalAlign)
            : base(string.IsNullOrEmpty(text))
        {
            // init
            this.Text = text;
            this.FontStyle = fontStyle;
            this.FontVariant = fontVariant;
            this.FontWeight = fontWeight;
            this.TextDecoration = textDecoration;
            this.VerticalAlign = verticalAlign;
        }

        public string Text
        {
            get;
            internal set;
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

        public bool IsFormatEqual(TextRun other)
        {
            return (this.FontStyle == other.FontStyle &&
                this.FontVariant == other.FontVariant &&
                this.FontWeight == other.FontWeight &&
                this.TextDecoration == other.TextDecoration &&
                this.VerticalAlign == other.VerticalAlign);
        }

        internal override IEnumerable<TextRun> GetTextRuns()
        {
            yield return this;
        }
    }
}
