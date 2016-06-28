using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Formatting
{
    public sealed class ComposedRun : Run
    {
        internal ComposedRun(string tag, IEnumerable<Run> children, bool byVariable)
            : base(children.All(x => x.IsEmpty))
        {
            // init
            this.Tag = tag;
            this.Children = children.ToArray();
            this.ByVariable = byVariable;
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
        internal bool ByVariable
        {
            get;
            private set;
        }

        internal override IEnumerable<TextRun> GetTextRuns()
        {
            return this.Children
                .SelectMany(x => x.GetTextRuns());
        }
        internal IEnumerable<ComposedRun> GetComposedRuns()
        {
            // done
            return new ComposedRun[] { this }.Concat(this.Children.OfType<ComposedRun>().SelectMany(x => x.GetComposedRuns()));
        }
    }
}
