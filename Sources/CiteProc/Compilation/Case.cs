using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Case : Scope
    {
        private string[] _Variables;

        public Case(Switch parent, string[] variables)
            : base(parent, null)
        {
            // init
            this._Variables = variables;
        }

        public override void Render(CodeWriter writer)
        {
            // variables
            foreach (var v in this._Variables)
            {
                writer.AppendIndent();
                if (v == null)
                {
                    // default
                    writer.Append("default");
                }
                else
                {
                    // case
                    writer.Append("case ");
                    writer.Append(v);
                }
                writer.Append(":");
                writer.Append(Environment.NewLine);
            }

            // code
            writer.IncreaseIndent();
            base.Render(writer);
            writer.DecreaseIndent();
        }
    }
}
