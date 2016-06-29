using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Scope : Snippet
    {
        private List<Action<CodeWriter>> _Actions = new List<Action<CodeWriter>>();

        public Scope(Snippet parent, Element context)
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

        public MethodInvoke AppendMethodInvoke(string methodName, Element context)
        {
            // append block
            return this.AppendSnippet<MethodInvoke>(new MethodInvoke(this, context, methodName));
        }
        public Array AppendArray<T>(string elementType, IEnumerable<T> items, Action<T, Scope> expression)
        {
            // done
            return this.AppendArray<T>(elementType, items, expression, null);
        }
        public Array AppendArray<T>(string elementType, IEnumerable<T> items, Action<T, Scope> expression, Action<Scope> nullExpression)
        {
            //add
            return this.AppendSnippet(Array.Create(this, elementType, items, expression, nullExpression));
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
