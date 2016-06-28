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
        private string _Prefix;

        internal LambdaExpression(Snippet parent, bool addBraces, string prefix)
            : base(parent, null)
        {
            // init
            this._AddBraces = addBraces;
            this._Prefix = prefix;

            // copy parameter name
            this.ParameterIndex++;
            this._ParameterName = this.ParameterName;
        }
        public override void Dispose()
        {
            // decrease
            this.ParameterIndex--;

            // base
            base.Dispose();
        }

        public bool IsArray
        {
            get;
            private set;
        }
        public void AppendArray<T>(IEnumerable<T> children, Action<T> renderer)
        {
            this.AppendArray<T>(children, renderer, null);
        }
        public void AppendArray<T>(IEnumerable<T> children, Action<T> renderer, Action nullRenderer)
        {
            // init
            this.IsArray = true;

            // null?
            if (children != null)
            {
                // init
                var index = children.Count();

                // loop
                foreach (var child in children)
                {
                    // render
                    renderer(child);

                    // comma?
                    if (index > 1)
                    {
                        this.Append(",");
                        this.AppendLineBreak();
                        this.AppendIndent();
                    }

                    // done
                    index--;
                }
            }
            else if (nullRenderer != null)
            {
                nullRenderer();
            }
        }

        public override void Render(CodeWriter writer)
        {
            // init
            writer.Append(this._ParameterName);
            writer.Append(" =>");
            if (this._AddBraces || !string.IsNullOrEmpty(this._Prefix))
            {
                writer.Append(" ");
                writer.Append(this._Prefix);
                if (this._AddBraces)
                {
                    writer.Append("{");
                }
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
            if (this.IsArray)
            {
                writer.Append(Environment.NewLine);
                writer.AppendIndent();
            }
            if (this._AddBraces)
            {
                writer.Append("}");
            }
        }
    }
}