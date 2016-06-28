using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Class : Snippet
    {
        private string _Signature;
        private List<Snippet> _Members = new List<Snippet>();

        public Class(Snippet parent, string signature)
            : base(parent, null)
        {
            // init
            this._Signature = signature;
        }

        public Method AppendMethod(string signature, params object[] args)
        {
            // init
            var result = new Method(this, string.Format(signature, args));

            // add
            this._Members.Add(result);

            // done
            return result;
        }
        public void AppendProperty(string signature, object literal)
        {
            // init
            var result = new Property(this, signature);

            // create getter
            result.Getter.AppendLine("return {0};", Compiler.GetLiteral(literal));

            // add
            this._Members.Add(result);
        }
        public Class AppendNestedClass(string signature, params object[] args)
        {
            // init
            var result = new Class(this, string.Format(signature, args));

            // add
            this._Members.Add(result);

            // done
            return result;
        }

        public override void Render(CodeWriter writer)
        {
            // summary
            if (!string.IsNullOrEmpty(this.Summary))
            {
                // <summary>
                writer.AppendIndent();
                writer.Append("/// <summary>");
                writer.Append(Environment.NewLine);

                // text
                writer.AppendIndent();
                writer.Append("/// ");
                writer.Append(this.Summary);
                writer.Append(Environment.NewLine);

                // </summary>
                writer.AppendIndent();
                writer.Append("/// </summary>");
                writer.Append(Environment.NewLine);
            }

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
            foreach (var c in this._Members)
            {
                c.Render(writer);
            }
            writer.DecreaseIndent();

            // {
            writer.AppendIndent();
            writer.Append("}");
            writer.Append(Environment.NewLine);
        }

        public string Summary
        {
            get;
            set;
        }
    }
}