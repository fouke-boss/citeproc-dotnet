using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Switch : Snippet
    {
        private string _Expression;
        private List<Case> _Cases = new List<Case>();

        public Switch(Snippet parent, string expression)
            : base(parent, null)
        {
            // init
            this._Expression = expression;
        }

        internal Case AppendDefault()
        {
            return this.AppendCases(new object[] { null });
        }
        internal Case AppendCases(params object[] cases)
        {
            // init
            var variables = cases
                .Select(x => (x == null ? null : Compiler.GetLiteral(x)))
                .ToArray();

            // init
            var result = new Case(this, variables);

            // add
            this._Cases.Add(result);

            // done
            return result;
        }

        public override void Render(CodeWriter writer)
        {
            // class
            writer.AppendIndent();
            writer.Append("switch(");
            writer.Append(this._Expression);
            writer.Append(")");
            writer.Append(Environment.NewLine);

            // {
            writer.AppendIndent();
            writer.Append("{");
            writer.Append(Environment.NewLine);

            // base
            writer.IncreaseIndent();
            foreach (var c in this._Cases)
            {
                c.Render(writer);
            }
            writer.DecreaseIndent();

            // {
            writer.AppendIndent();
            writer.Append("}");
            writer.Append(Environment.NewLine);
        }
    }
}