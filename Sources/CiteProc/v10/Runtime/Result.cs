using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CiteProc.Formatting;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected class Result
        {
            private static readonly Regex PUNCTUATION_REGEX = new Regex("^([.,]+)");

            private TextCase? _TextCase;

            public Result(string tag, IEnumerable<Run> children, bool byVariable, string prefix, string suffix, bool quotes, TextCase? textCase)
            {
                // init
                this.Tag = tag;
                this.Children = children.ToArray();
                this.IsEmpty = (this.Children.Length == 0 || this.Children.All(x => x.IsEmpty));
                this.ByVariable = byVariable;
                this.Prefix = prefix;
                this.Suffix = suffix;
                this.Quotes = quotes;
                this._TextCase = textCase;
            }

            public string Tag
            {
                get;
                private set;
            }

            public Run[] Children
            {
                get;
                private set;
            }
            public bool ByVariable
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
            public bool Quotes
            {
                get;
                private set;
            }

            public bool IsEmpty
            {
                get;
                private set;
            }

            public ComposedRun ToComposedRun(ExecutionContext c, Parameters p)
            {
                // init
                var children = this.Children
                    .ToList();

                // children?
                if (children.Where(x => !x.IsEmpty).Count() > 0)
                {
                    // apply text case
                    this.ApplyPunctuationInQuote(c, children);
                    this.ApplyTextCase(c, children);

                    // quotes
                    if (this.Quotes)
                    {
                        // open
                        children.InsertRange(0, p.GenerateText(c.Locale.OpenQuote));

                        // close
                        children.AddRange(p.GenerateText(c.Locale.CloseQuote));
                    }

                    // prefix
                    if (!string.IsNullOrEmpty(this.Prefix))
                    {
                        children.InsertRange(0, p.GenerateText(this.Prefix));
                    }

                    // suffix
                    if (!string.IsNullOrEmpty(this.Suffix))
                    {
                        children.AddRange(p.GenerateText(this.Suffix));
                    }
                }

                // done
                return new ComposedRun(this.Tag, children, this.ByVariable);
            }
            private void ApplyPunctuationInQuote(ExecutionContext c, IEnumerable<Run> children)
            {
                // punctuation in quote
                if (c.Locale.PunctuationInQuote)
                {
                    // init
                    var texts = children
                        .SelectMany(x => x.GetTextRuns())
                        .ToArray();

                    // loop
                    for (var i = 0; i < texts.Length - 1; i++)
                    {
                        if (texts[i].Text == c.Locale.CloseQuote)
                        {
                            // match
                            var match = PUNCTUATION_REGEX.Match(texts[i + 1].Text);
                            if (match.Success)
                            {
                                // remove punctuation
                                texts[i + 1].Text = texts[i + 1].Text.Substring(match.Length);

                                // insert punctuation
                                texts[i].Text = string.Format("{0}{1}", match.Value, texts[i].Text);
                            }
                        }
                    }
                }
            }
            private void ApplyTextCase(ExecutionContext c, IEnumerable<Run> children)
            {
                // text case
                if (this._TextCase.HasValue)
                {
                    // flatten
                    var texts = children
                            .SelectMany(x => x.GetTextRuns());

                    // switch
                    switch (this._TextCase.Value)
                    {
                        case TextCase.Lowercase:
                            // lowercase
                            foreach (var text in texts)
                            {
                                text.Text = text.Text.ToLower();
                            }
                            break;
                        case TextCase.Uppercase:
                            // uppercase
                            foreach (var text in texts)
                            {
                                text.Text = text.Text.ToUpper();
                            }
                            break;
                        case TextCase.CapitalizeFirst:
                            {
                                // capitalize the first word if lower case
                                var word = this.GetWords().FirstOrDefault();
                                if (word != null && word.IsLower)
                                {
                                    word.Capitalize();
                                }
                            }
                            break;
                        case TextCase.CapitalizeAll:
                            {
                                // capitalize all lower case words
                                foreach (var word in this.GetWords().Where(x => x.IsLower))
                                {
                                    word.Capitalize();
                                }
                            }
                            break;
                        case TextCase.Sentence:
                            {
                                // get words
                                var words = this.GetWords()
                                    .ToArray();

                                // all upper?
                                if (words.All(w => w.IsUpper))
                                {
                                    // string is uppercase
                                    foreach (var text in texts)
                                    {
                                        text.Text = text.Text.ToLower();
                                    }

                                    // capitalize first word
                                    if (words.Length > 0)
                                    {
                                        words[0].Capitalize();
                                    }
                                }
                                else
                                {
                                    // capitalize first word if lowercase
                                    if (words.Length > 0 && words[0].IsLower)
                                    {
                                        words[0].Capitalize();
                                    }
                                }
                            }
                            break;
                        case TextCase.Title:
                            if (c.Culture.Language == "en" || (c.Culture.IsInvariant && (c.Locale.Culture.IsInvariant || c.Locale.Culture.Language == "en")))
                            {
                                // get words
                                var words = this.GetWords()
                                    .ToArray();

                                // string is uppercase?
                                if (words.All(w => w.IsUpper) && words.Length > 1)
                                {
                                    // string is uppercase
                                    foreach (var text in texts)
                                    {
                                        text.Text = text.Text.ToLower();
                                    }
                                }

                                // capitalize
                                int index = 0;
                                foreach (var word in words)
                                {
                                    // fix casing
                                    if (word.IsStopWord && index > 0 && index < (words.Length - 1))
                                    {
                                        word.ToLower();
                                    }
                                    else if (word.IsLower)
                                    {
                                        word.Capitalize();
                                    }

                                    // next
                                    index++;
                                }
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
            private IEnumerable<Word> GetWords()
            {
                return this.Children
                    .SelectMany(r => r.GetTextRuns())
                    .SelectMany(t => Word.Split(t));
            }
        }
    }
}
