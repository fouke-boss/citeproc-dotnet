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
        }
        public virtual void Dispose()
        {
        }

        public Snippet Parent
        {
            get;
            private set;
        }
        public Element Context
        {
            get;
            private set;
        }

        public virtual void RegisterMacros(IEnumerable<string> macros)
        {
            this.Parent.RegisterMacros(macros);
        }
        public virtual string GetMacro(string name)
        {
            return this.Parent.GetMacro(name);
        }

        public virtual int ParameterIndex
        {
            get
            {
                return this.Parent.ParameterIndex;
            }
            set
            {
                this.Parent.ParameterIndex = value;
            }
        }
        public string ParameterName
        {
            get
            {
                return string.Format("{0}{1}", Compiler.PARAMETER_NAME, (this.ParameterIndex == -1 ? "" : this.ParameterIndex.ToString()));
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