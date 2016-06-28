using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Method : Scope
    {
        private string _Signature;

        public Method(Class parent, string signature)
            : base(parent, null)
        {
            // init
            this._Signature = signature;
        }

        public override void Render(CodeWriter writer)
        {
            // class
            writer.AppendIndent();
            writer.Append(this._Signature);
            writer.Append(Environment.NewLine);

            // {
            writer.AppendIndent();
            writer.Append("{");
            writer.Append(Environment.NewLine);

            // base
            writer.IncreaseIndent();
            base.Render(writer);
            writer.DecreaseIndent();

            // {
            writer.AppendIndent();
            writer.Append("}");
            writer.Append(Environment.NewLine);
        }
    }
}
