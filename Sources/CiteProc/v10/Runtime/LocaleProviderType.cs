using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    internal class LocaleProviderType
    {
        private string _ClassName;

        public LocaleProviderType(Culture culture, string className)
        {
            this.Culture = culture;
            this._ClassName = className;
        }

        public Culture Culture
        {
            get;
            private set;
        }
        public string ClassName
        {
            get
            {
                return (this.IsPreCompiled ? string.Format("LocaleProvider.@{0}", this.Culture.GetMethodName()) : this._ClassName);
            }
        }
        public bool IsPreCompiled
        {
            get
            {
                return (string.IsNullOrEmpty(this._ClassName));
            }
        }

        public override string ToString()
        {
            return this.Culture;
        }
    }
}