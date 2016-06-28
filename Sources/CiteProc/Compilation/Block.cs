using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
#warning Deze class nog in gebruik?
    internal class Block : Scope
    {
        private string _Suffix;

        internal Block(Snippet parent, string suffix)
            : base(parent, null)
        {
            this._Suffix = suffix;
        }

        public override void Render(CodeWriter writer)
        {
            // {
            writer.Append("{");
            writer.Append(Environment.NewLine);

            // base
            writer.IncreaseIndent();
            base.Render(writer);
            writer.DecreaseIndent();

            // {
            writer.AppendIndent();
            writer.Append("}");
            writer.Append(this._Suffix);
        }
    }
}
