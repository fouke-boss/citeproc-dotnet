using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class LambdaExpression : Scope
    {
        private string _ParameterName;
        private bool _AddBraces;

        internal LambdaExpression(Snippet parent, bool addBraces)
            : base(parent, null)
        {
            // init
            this._AddBraces = addBraces;

            // copy parameter name
            this.Root.ParameterIndex++;
            this._ParameterName = this.ParameterName;
        }
        public override void Dispose()
        {
            // decrease
            this.Root.ParameterIndex--;

            // base
            base.Dispose();
        }

        public override void Render(CodeWriter writer)
        {
            // init
            writer.Append(this._ParameterName);
            writer.Append(" =>");
            if (this._AddBraces)
            {
                writer.Append(" {");
            }

            // base
            writer.IncreaseIndent();
            if (this.HasChildren)
            {
                writer.Append(Environment.NewLine);
                writer.AppendIndent();
            }
            base.Render(writer);
            writer.DecreaseIndent();

            // end block
            if (this._AddBraces)
            {
                writer.Append("}");
            }
        }
    }
}