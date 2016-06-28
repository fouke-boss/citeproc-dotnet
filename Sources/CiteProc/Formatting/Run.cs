using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CiteProc.Formatting
{
    public abstract class Run
    {
        internal Run(bool isEmpty)
        {
            // init
            this.IsEmpty = isEmpty;
        }

        public bool IsEmpty
        {
            get;
            private set;
        }

        internal abstract IEnumerable<TextRun> GetTextRuns();
        public IEnumerable<TextRun> Flatten()
        {
            // get text runs
            var texts = this.GetTextRuns()
                .ToArray();

            // group by formatting
            var index = 0;
            var text = new StringBuilder(4096);
            while (index < texts.Length)
            {
                // init
                var first = texts[index];
                text.Clear();

                // loop
                while (index < texts.Length && texts[index].IsFormatEqual(first))
                {
                    // append
                    text.Append(texts[index].Text);

                    // next
                    index++;
                }

                // done
                yield return new TextRun(text.ToString(), first.FontStyle, first.FontVariant, first.FontWeight, first.TextDecoration, first.VerticalAlign);
            }
        }

        public void ToPlainText(StringBuilder output)
        {
            foreach (var text in this.GetTextRuns())
            {
                output.Append(text.Text);
            }
        }
        public string ToPlainText()
        {
            // init
            var result = new StringBuilder(65536);

            // render
            this.ToPlainText(result);

            // done
            return result.ToString();
        }

        public string ToHtml()
        {
            // init
            var result = new StringBuilder(65536);

            // to html
            this.ToHtml(result);

            // done
            return result.ToString();
        }
        public void ToHtml(XmlWriter writer)
        {
            using (var hw = new HtmlWriter(writer))
            {
                foreach (var text in this.GetTextRuns())
                {
                    hw.Add(text);
                }
            }
        }
        public void ToHtml(TextWriter writer)
        {
            // init
            var settings = new XmlWriterSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            // done
            using (var xw = XmlWriter.Create(writer, settings))
            {
                this.ToHtml(xw);
            }
        }
        public void ToHtml(Stream stream)
        {
            // init
            var settings = new XmlWriterSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            // done
            using (var xw = XmlWriter.Create(stream, settings))
            {
                this.ToHtml(xw);
            }
        }
        public void ToHtml(StringBuilder output)
        {
            // init
            var settings = new XmlWriterSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            // done
            using (var xw = XmlWriter.Create(output, settings))
            {
                this.ToHtml(xw);
            }
        }

        public override string ToString()
        {
            return this.ToPlainText();
        }
    }
}