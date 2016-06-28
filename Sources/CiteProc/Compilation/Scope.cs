using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal abstract class Scope : Snippet
    {
        private List<Action<CodeWriter>> _Actions = new List<Action<CodeWriter>>();

        protected Scope(Snippet parent, Element context)
            : base(parent, context)
        {
        }

        public void Append(string text, params object[] args)
        {
            this._Actions.Add(c => c.Append(string.Format(text, args)));
        }
        public void AppendLine(string text, params object[] args)
        {
            this.AppendIndent();
            this.Append(text, args);
            this.AppendLineBreak();
        }
        public void AppendLineBreak()
        {
            this.Append(Environment.NewLine);
        }

        public void AppendIndent()
        {
#if(DEBUG)
            this._Actions.Add(c => c.AppendIndent());
#endif
        }
//        public void IncreaseIndent()
//        {
//#if(DEBUG)
//            this._Actions.Add(c => c.IncreaseIndent());
//#endif
//        }
//        public void DecreaseIndent()
//        {
//#if(DEBUG)
//            this._Actions.Add(c => c.DecreaseIndent());
//#endif
//        }

        public void AppendComment(string comment, params object[] args)
        {
#if(DEBUG)
            this.AppendLine("// {0}", string.Format(comment, args));
#endif
        }

        public Switch AppendSwitch(string expression)
        {
            return this.AppendSnippet<Switch>(new Switch(this, expression));
        }

        public Block AppendBlock()
        {
            return this.AppendBlock(true, Environment.NewLine);
        }
        public Block AppendBlock(bool indent, string suffix)
        {
            // indent
            if (indent)
            {
                this.AppendIndent();
            }

            // append block
            return this.AppendSnippet<Block>(new Block(this, suffix));
        }
        public MethodInvoke AppendMethodInvoke(string methodName, Element context)
        {
            // append block
            return this.AppendSnippet<MethodInvoke>(new MethodInvoke(this, context, methodName));
        }
        protected T AppendSnippet<T>(T snippet)
            where T : Snippet
        {
            // add
            this._Actions.Add(c => snippet.Render(c));

            // done
            return snippet;
        }

        protected bool HasChildren
        {
            get
            {
                return (this._Actions.Count > 0);
            }
        }

        public override void Render(CodeWriter writer)
        {
            foreach (var action in this._Actions)
            {
                action(writer);
            }
        }
    }
}
