using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Namespace : Snippet
    {
        private string _Name;
        private List<Class> _Classes = new List<Class>();

        public Namespace(Snippet parent, string name)
            : base(parent, null)
        {
            // init
            this._Name = name;
        }

        public Class AppendClass(string signature, params object[] args)
        {
            // init
            var result = new Class(this, string.Format(signature, args));

            // add
            this._Classes.Add(result);

            // done
            return result;
        }

        public override void Render(CodeWriter writer)
        {
            // namespace
            writer.AppendIndent();
            writer.Append("namespace ");
            writer.Append(this._Name);
            writer.Append(Environment.NewLine);

            // {
            writer.AppendIndent();
            writer.Append("{");
            writer.Append(Environment.NewLine);

            // base
            writer.IncreaseIndent();
            foreach (var c in this._Classes)
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