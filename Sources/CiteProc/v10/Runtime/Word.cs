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
#warning Transfer to the CitProc.Formatting namespace?
        protected class Word
        {
            private static readonly Regex SPLITTER = new Regex("([A-Za-z0-9]+)");
            private static readonly string[] STOP_WORDS = new string[] { "a", "an", "and", "as", "at", "but", "by", "down", "for", "from", "in", "into", "nor", "of", "on", "onto", "or", "over", "so", "the", "till", "to", "up", "via", "with", "yet" };

            private TextRun _Run;
            private System.Text.RegularExpressions.Match _Match;

            private Word(TextRun run, System.Text.RegularExpressions.Match match)
            {
                this._Run = run;
                this._Match = match;
            }
            public static IEnumerable<Word> Split(TextRun run)
            {
                return SPLITTER.Matches(run.Text)
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(match => new Word(run, match));
            }

            public bool IsLower
            {
                get
                {
                    return this._Match.Value.All(x => char.IsLower(x));
                }
            }
            public bool IsUpper
            {
                get
                {
                    return this._Match.Value.All(x => char.IsUpper(x));
                }
            }
            public bool IsMixed
            {
                get
                {
                    return !(this.IsLower || this.IsUpper);
                }
            }
            public bool IsStopWord
            {
                get
                {
                    return STOP_WORDS.Any(x => string.Compare(x, this._Match.Value, true) == 0); 
                }
            }

            public void Capitalize()
            {
                // done
                this.Replace(string.Format("{0}{1}", this._Match.Value.Substring(0, 1).ToUpper(), this._Match.Value.Substring(1).ToLower()));
            }
            public void ToLower()
            {
                // done
                this.Replace(this._Match.Value.ToLower());
            }
            private void Replace(string text)
            {

                // done
                this._Run.Text = string.Format("{0}{1}{2}",
                    this._Run.Text.Substring(0, this._Match.Index),
                    text,
                    this._Run.Text.Substring(this._Match.Index + this._Match.Length)
                );
            }

            public override string ToString()
            {
                return this._Match.Value;
            }
        }
    }
}
