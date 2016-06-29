using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal class Array : Snippet
    {
        private string _ElementType;

        internal static Array Create<T>(Snippet parent, string elementType, IEnumerable<T> items, Action<T, Scope> expression, Action<Scope> nullExpression)
        {
            // init
            var result = new Array(parent, elementType);

            // null
            if (items == null)
            {
                // null expression?
                if (nullExpression == null)
                {
                    // done
                    result.Items = new Scope[] { };
                }
                else
                {
                    // init
                    var scope = new Scope(result, null);

                    // execute
                    nullExpression(scope);

                    // done
                    result.Items = new Scope[]
                    {
                        scope 
                    };
                }
            }
            else
            {
                // render expressions
                result.Items = items
                    .Select(x =>
                    {
                        // create block
                        var scope = new Scope(result, null);

                        // expression
                        expression(x, scope);

                        // done
                        return scope;
                    })
                    .ToArray();
            }


            // done
            return result;
        }
        internal Array(Snippet parent, string elementType)
            : base(parent, null)
        {
            // init
            this._ElementType = elementType;
        }

        public Scope[] Items
        {
            get;
            private set;
        }

        public override void Render(CodeWriter writer)
        {
            // {
            writer.Append("new ");
            writer.Append(this._ElementType);
            writer.Append("[]{");
            writer.Append(Environment.NewLine);

            // base
            writer.IncreaseIndent();
            int index = this.Items.Length - 1;
            foreach (var item in this.Items)
            {
                // write scope
                writer.AppendIndent();
                item.Render(writer);

                // comma?
                if (index > 0)
                {
                    writer.Append(",");
                    index--;
                }

                // done
                writer.Append(Environment.NewLine);
            }
            writer.DecreaseIndent();

            // }
            writer.AppendIndent();
            writer.Append("}");
        }
    }
}
