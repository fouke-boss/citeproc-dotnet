using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal abstract class Snippet : IDisposable
    {
        protected Snippet(Snippet parent, Element context)
        {
            // init
            this.Parent = parent;
            this.Context = (context == null && parent != null ? parent.Context : context);

            // root
            this.Root = (parent == null ? (Compiler)this : parent.Root);
        }
        public virtual void Dispose()
        {
        }

        public Snippet Parent
        {
            get;
            private set;
        }
        public Compiler Root
        {
            get;
            private set;
        }
        public Element Context
        {
            get;
            private set;
        }

        public string ParameterName
        {
            get
            {
                return string.Format("{0}{1}", Compiler.PARAMETER_NAME, (this.Root.ParameterIndex == -1 ? "" : this.Root.ParameterIndex.ToString()));
            }
        }

        public abstract void Render(CodeWriter writer);

        public override string ToString()
        {
            // init
            var cw = new CodeWriter();

            // render
            this.Render(cw);

            // done
            return cw.ToString();
        }
    }
}