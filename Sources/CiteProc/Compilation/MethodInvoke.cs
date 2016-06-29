using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class MethodInvoke : Snippet
    {
        private List<object> _Parameters = new List<object>();

        internal MethodInvoke(Snippet parent, Element context, string methodName)
            : base(parent, context)
        {
            // init
            this.Name = methodName;
        }

        public string Name
        {
            get;
            set;
        }

        public void AddCode(string text, params object[] args)
        {
            // init
            this._Parameters.Add(string.Format(text, args));
        }
        public void AddLiteral(object literal)
        {
            // init
            this._Parameters.Add(Compiler.GetLiteral(literal));
        }
        public void AddElement(Element element)
        {
            // done
            this.AddLiteral(element.Tag);
        }

        public MethodInvoke AddMethodInvoke(string methodName, Element context)
        {
            // init
            var result = new MethodInvoke(this, context, methodName);

            // add
            this._Parameters.Add(result);

            // done
            return result;
        }
        public LambdaExpression AddLambdaExpression(bool addBraces)
        {
            // init
            var result = new LambdaExpression(this, addBraces);

            // add
            this._Parameters.Add(result);

            // done
            return result;
        }
        public Array AddArray<T>(string elementType, IEnumerable<T> items, Action<T, Scope> expression)
        {
            // init
            var result = Array.Create(this, elementType, items, expression, null);

            // add
            this._Parameters.Add(result);

            // done
            return result;
        }

        public override void Render(CodeWriter writer)
        {
            // init
            writer.Append(this.Name);
            writer.Append("(");

            // init
            int index = 0;

            // parameters
            foreach (var value in this._Parameters)
            {
                // comma
                if (index > 0)
                {
                    writer.Append(", ");
                }

                // render
                if (value is string)
                {
                    // append value
                    writer.Append((string)value);
                }
                else if (value is Snippet)
                {
                    // append child
                    ((Snippet)value).Render(writer);
                }
                else
                {
                    throw new NotSupportedException();
                }

                // next
                index++;
            }

            // done
            writer.Append(")");
        }
    }
}