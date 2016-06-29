using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Property : Scope
    {
        private string _Signature;

        public Property(Class parent, string signature)
            : base(parent, null)
        {
            // init
            this._Signature = signature;

            // getter and setter
            this.Getter = new Scope(this, null);
        }

        public Scope Getter
        {
            get;
            private set;
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
            writer.IncreaseIndent();

            // getter
            writer.AppendIndent();
            writer.Append("get");
            writer.Append(Environment.NewLine);
            writer.AppendIndent();
            writer.Append("{");
            writer.Append(Environment.NewLine);
            writer.IncreaseIndent();
            this.Getter.Render(writer);
            writer.DecreaseIndent();
            writer.AppendIndent();
            writer.Append("}");
            writer.Append(Environment.NewLine);

            // }
            writer.DecreaseIndent();
            writer.AppendIndent();
            writer.Append("}");
            writer.Append(Environment.NewLine);
        }
    }
}
