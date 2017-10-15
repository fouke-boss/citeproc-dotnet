using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    /// <summary>
    /// Represents the root of the snippet tree.
    /// </summary>
    internal class Compiler : Snippet
    {
        public const string CONTEXT_NAME = "c";
        public const string PARAMETER_NAME = "p";

        private static Microsoft.CSharp.CSharpCodeProvider _CodeProvider;

        static Compiler()
        {
            // code provider
            _CodeProvider = new Microsoft.CSharp.CSharpCodeProvider();
        }

        private List<string> _Usings = new List<string>();
        private List<Namespace> _Namespaces = new List<Namespace>();
        private Dictionary<string, string> _MacroMappings = new Dictionary<string, string>();
        private Dictionary<string, Element> _SpecialElements = new Dictionary<string, Element>();

        public Compiler()
            : base(null, null)
        {
            // init
            this.ParameterIndex = -1;
        }

        public void AppendUsing(string @namespace)
        {
            this._Usings.Add(@namespace);
        }
        public Namespace AppendNamespace(string name)
        {
            // init
            var result = new Namespace(this, name);

            // add
            this._Namespaces.Add(result);

            // done
            return result;
        }

        public void RegisterMacros(IEnumerable<string> macros)
        {
            // loop
            foreach (var macro in macros)
            {
                // init
                var name = string.Format("GenerateMacro{0:00}", this._MacroMappings.Count);

                // add
                this._MacroMappings.Add(macro, name);
            }
        }
        public string GetMacro(string name)
        {
            // find
            if (!this._MacroMappings.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException(name, string.Format("Macro '{0}' is not defined.", name));
            }

            // done
            return this._MacroMappings[name];
        }
        public int ParameterIndex
        {
            get;
            set;
        }

        public void SetSpecialElement<T>(T element)
            where T : Element
        {
            this.SetSpecialElement<T>(element, "");
        }
        public void SetSpecialElement<T>(T element, string name)
            where T : Element
        {
            // init
            var key = string.Format("{0}::{1}", typeof(T).FullName, name);

            // done
            this._SpecialElements.Add(key, element);
        }

        public T GetSpecialElement<T>()
            where T : Element
        {
            return this.GetSpecialElement<T>("");
        }
        public T GetSpecialElement<T>(string name)
            where T : Element
        {
            // init
            var key = string.Format("{0}::{1}", typeof(T).FullName, name);

            // done
            return (this._SpecialElements.ContainsKey(key) ? (T)this._SpecialElements[key] : null);
        }


        public static string GetLiteral(object value)
        {
            // generate code
            if (value == null)
            {
                return "null";
            }
            else if (value is string)
            {
                return string.Format("\"{0}\"", ((string)value)
                    .Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\tt")
                    );
            }
            else if (value is bool)
            {
                return ((bool)value ? "true" : "false");
            }
            else if (value is int)
            {
                return value.ToString();
            }
            else if (value.GetType().IsEnum)
            {
                return string.Format("{0}.{1}", value.GetType().Name, value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override void Render(CodeWriter writer)
        {
            // usings
            foreach (var u in this._Usings)
            {
                writer.AppendIndent();
                writer.Append("using ");
                writer.Append(u);
                writer.Append(";");
                writer.Append(Environment.NewLine);
            }

            // empty line
            writer.Append(Environment.NewLine);

            // namespaces
            foreach (var ns in this._Namespaces)
            {
                ns.Render(writer);
            }
        }
        public CompilerResults Compile()
        {
            // init
            var guid = Guid.NewGuid().ToString();
            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), guid);

            // create temp path
            System.IO.Directory.CreateDirectory(tempPath);

            // options
            var options = new System.CodeDom.Compiler.CompilerParameters()
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                OutputAssembly = System.IO.Path.Combine(tempPath, string.Format("{0}.dll", guid)),
                TempFiles = new TempFileCollection(tempPath, false)
            };

            // references
            var references = new Type[] { typeof(string), typeof(Enumerable), typeof(Processor) };
            options.ReferencedAssemblies.AddRange(references
                .Select(x => x.Assembly)
                .Where(x => !x.IsDynamic)
                .GroupBy(x => x.FullName)
                .Select(x => x.First().Location)
                .ToArray());

            // compile
            return _CodeProvider.CompileAssemblyFromSource(options, this.ToString());
        }
    }
}
