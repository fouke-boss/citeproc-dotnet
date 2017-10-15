using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiteProc.Compilation;
using CiteProc.Formatting;

namespace CiteProc
{
    /// <summary>
    /// Base class for processors of any version of the CSl specifications.
    /// </summary>
    public abstract class Processor
    {
        /// <summary>
        /// Compiles the given style and locale files into a Processor instance.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static Processor Compile(params File[] files)
        {
            // init
            Compiler compiler = null;

            // single version
            var versions = files
                .Select(x => x.Version)
                .Distinct()
                .ToArray();
            if (versions.Length != 1)
            {
                throw new CompilerException("All files are required to have the same version.");
            }

            // find version
            switch (versions.Single().ToString())
            {
                case "1.0":
                    compiler = v10.Runtime.Processor.Compile(files);
                    break;
                default:
                    throw new CompilerException("Version '{0}' is not supported.", versions.Single());
            }

            // compile
            var result = compiler.Compile();
            if (result.Errors.Count > 0)
            {
                System.IO.File.AppendAllText(@"D:\templog.txt", result.Errors[0].ErrorText);
                throw new NotSupportedException();
            }

            // done
            var type = result.CompiledAssembly.GetTypes()
                .Where(x => typeof(Processor).IsAssignableFrom(x))
                .Single();

            // done
            return (Processor)type.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id"></param>
        /// <param name="title"></param>
        protected Processor(Version version, string id, string title)
        {
            // init
            this.Version = version;
            this.Id = id;
            this.Title = title;
        }

        /// <summary>
        /// Returns the ID of the compiled style.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }
        /// <summary>
        /// Returns the title of the compiled style.
        /// </summary>
        public string Title
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the version of the CSL specification used for the compiled style.
        /// </summary>
        public Version Version
        {
            get;
            private set;
        }

        public abstract IEnumerable<IDataProvider> DataProviders
        {
            get;
            set;
        }

        public ComposedRun[] GenerateBibliography()
        {
            return this.GenerateBibliography("en-US", false);
        }
        public abstract ComposedRun[] GenerateBibliography(string locale, bool forceLocale);

        public ComposedRun GenerateBibliographyEntry(IDataProvider dataProvider)
        {
            return this.GenerateBibliographyEntry(dataProvider, "en-US", false);
        }
        public abstract ComposedRun GenerateBibliographyEntry(IDataProvider dataProvider, string locale, bool forceLocale);

        public ComposedRun GenerateCitation(IDataProvider[] items)
        {
            return this.GenerateCitation(items, "en-US", false);
        }
        public abstract ComposedRun GenerateCitation(IDataProvider[] items, string locale, bool forceLocale);
    }
}