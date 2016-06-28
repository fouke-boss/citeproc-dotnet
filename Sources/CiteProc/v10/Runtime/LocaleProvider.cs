using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiteProc.Compilation;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
#warning naar Locales namespace?
        protected internal abstract partial class LocaleProvider
        {
            private static string[][] _RomanNumerals = new string[][]
            {
                new string[]{"", "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix"}, // ones
                new string[]{"", "x", "xx", "xxx", "xl", "l", "lx", "lxx", "lxxx", "xc"}, // tens
                new string[]{"", "c", "cc", "ccc", "cd", "d", "dc", "dcc", "dccc", "cm"}, // hundreds
                new string[]{"", "m", "mm", "mmm"} // thousands
            };

            public static LocaleProvider[] Defaults
            {
                get
                {
                    return typeof(LocaleProvider)
                        .GetNestedTypes()
                        .Select(x => (LocaleProvider)x.GetConstructor(Type.EmptyTypes).Invoke(null))
                        .ToArray();
                }
            }

            protected LocaleProvider(Culture culture)
            {
                // init
                this.Culture = culture;

                // precache
                this.OpenQuote = this.GetTerm(TermName.OpenQuote, TermFormat.Long, false);
                this.CloseQuote = this.GetTerm(TermName.CloseQuote, TermFormat.Long, false);
            }

            public Culture Culture
            {
                get;
                private set;
            }

            public virtual bool LimitDayOrdinalsToDay1
            {
                get
                {
                    return false;
                }
            }
            public virtual bool PunctuationInQuote
            {
                get
                {
                    return false;
                }
            }

            public DatePartParameters[] GetDateParts(DateFormat format, Parameters p)
            {
                switch (format)
                {
                    case DateFormat.Numeric:
                        return this.GetDatePartsAsNumeric(p).ToArray();
                    case DateFormat.Text:
                        return this.GetDatePartsAsText(p).ToArray();
                    default:
                        throw new NotSupportedException();
                }
            }
            public virtual IEnumerable<DatePartParameters> GetDatePartsAsNumeric(Parameters p)
            {
                throw new NotSupportedException();
            }
            public virtual IEnumerable<DatePartParameters> GetDatePartsAsText(Parameters p)
            {
                throw new NotSupportedException();
            }

            public virtual string GetTerm(TermName name, TermFormat format, bool plural)
            {
                return null;
            }
            public virtual Gender? GetTermGender(TermName name)
            {
                return null;
            }

            public string FormatNumber(uint value, NumberFormat format, Gender? gender)
            {
                switch (format)
                {
                    case NumberFormat.Numeric:
                        return value.ToString();
                    case NumberFormat.Ordinal:
                        return this.FormatNumberAsOrdinal(value, gender);
                    case NumberFormat.LongOrdinal:
                        return this.FormatNumberAsLongOrdinal(value, gender);
                    case NumberFormat.Roman:
                        return this.FormatNumberAsRoman(value);
                    default:
                        throw new NotSupportedException();
                }
            }
            public virtual string FormatNumberAsOrdinal(uint value, Gender? gender)
            {
                throw new NotSupportedException();
            }
            public virtual string FormatNumberAsLongOrdinal(uint value, Gender? gender)
            {
                throw new NotSupportedException();
            }
            public string FormatNumberAsRoman(uint number)
            {
                // valid?
                if (number == 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (number >= 4000)
                {
                    throw new ArgumentOutOfRangeException();
                }

                // done
                return string.Join("", number.ToString()
                    .ToCharArray()
                    .Reverse()
                    .Select((c, i) => _RomanNumerals[i][int.Parse(c.ToString())])
                    .Reverse());
            }

            public string OpenQuote
            {
                get;
                private set;
            }
            public string CloseQuote
            {
                get;
                private set;
            }

            public override string ToString()
            {
                return this.Culture;
            }

            #region Compilation
            /// <summary>
            /// Implements locale fallback.
            /// </summary>
            /// <param name="code"></param>
            /// <param name="files"></param>
            /// <param name="independents"></param>
            /// <param name="dependents"></param>
            internal static string[] Compile(Class code, LocaleFile[] files, LocaleElement[] independents, LocaleElement[] dependents)
            {
                // compile files
                var results = CompileLocaleFiles(code, files, "protected");

                // compile locales
                results = CompileLocales(code, results, independents, dependents);

                // done
                return results
                    .OrderBy(x => x.Culture.ToString())
                    .Select(x => x.ClassName)
                    .ToArray();
            }
            /// <summary>
            /// Compiles the locale provider classes for the given locale files.
            /// </summary>
            /// <param name="code"></param>
            /// <param name="files"></param>
            /// <param name="accessor"></param>
            /// <returns></returns>
            internal static LocaleProviderType[] CompileLocaleFiles(Class code, LocaleFile[] files, string accessor)
            {
                // init
                LocaleFile[] defaultFiles = null;

                // find a list of all cultures (ordered by invariant, language, dialect)
                var cultures = files
                    .Select(x => (Culture)x.XmlLang)
                    .Concat(LocaleProvider.Defaults.Select(x => x.Culture))
                    .Distinct()
                    .OrderBy(x => x.ToString())
                    .ToArray();

                // render code
                var results = new List<LocaleProviderType>();
                foreach (var culture in cultures)
                {
                    // base class
                    var baseClass = results.SingleOrDefault(x => x.Culture == culture.Language) ?? results.FirstOrDefault();

                    // find file
                    var file = files.SingleOrDefault(x => x.XmlLang == culture);

                    // compile required?
                    LocaleProviderType result = null;
                    if (file == null && (baseClass == null || baseClass.IsPreCompiled))
                    {
                        // use the precompiled version
                        result = new LocaleProviderType(culture, null);
                    }
                    else
                    {
                        // compile required
                        if (file == null)
                        {
                            // get default files
                            if (defaultFiles == null)
                            {
                                defaultFiles = LocaleFile.Defaults;
                            }

                            // find default file
                            file = defaultFiles.Single(x => x.XmlLang == culture);
                        }

                        // compile
                        result = Compile(code, file, "{0}", baseClass, accessor, culture);
                    }

                    // add
                    results.Add(result);
                }

                // done
                return results.ToArray();
            }
            /// <summary>
            /// Compiles the locale provider classes for the given locale elements in the independent and dependent styles.
            /// </summary>
            /// <param name="code"></param>
            /// <param name="files"></param>
            /// <param name="independents"></param>
            /// <param name="dependents"></param>
            /// <returns></returns>
            private static LocaleProviderType[] CompileLocales(Class code, LocaleProviderType[] files, LocaleElement[] independents, LocaleElement[] dependents)
            {
                // merge dependents and independents
                var locales = independents
                    .Concat(dependents)
                    .Select(x => (Culture)x.XmlLang)
                    .Distinct()
                    .OrderBy(x => x.ToString())
                    .Select(culture =>
                    {
                        return dependents.SingleOrDefault(x => x.XmlLang == culture) ?? independents.SingleOrDefault(x => x.XmlLang == culture);
                    })
                    .ToArray();

                // find a list of all cultures (ordered by invariant, language, dialect)
                var cultures = locales
                    .Select(x => (Culture)x.XmlLang)
                    .Concat(files.Select(x => x.Culture))
                    .Distinct()
                    .OrderBy(x => x.ToString())
                    .ToArray();

                // invariant
                var results1 = cultures
                    .Select(culture =>
                    {
                        // init
                        var type = culture.FindBestMatch(files, x => x.Culture);

                        // invariant
                        var invariant = locales.SingleOrDefault(x => x.XmlLang == Culture.Invariant);
                        if (invariant != null)
                        {
                            // render
                            type = Compile(code, invariant, "{0}_Invariant", type, "protected", culture);
                        }

                        // done
                        return (type.Culture == culture ? type : null);
                    })
                    .Where(x => x != null)
                    .ToArray();

                // languages
                var results2 = cultures
                    .Select(culture =>
                    {
                        // init
                        var type = culture.FindBestMatch(results1, x => x.Culture);

                        // language
                        var language = locales.SingleOrDefault(x => new Culture(x.XmlLang) == culture.Language);
                        if (language != null)
                        {
                            // render
                            type = Compile(code, language, "{0}_Language", type, "protected", culture);
                        }

                        // done
                        return (type.Culture == culture ? type : null);
                    })
                    .Where(x => x != null)
                    .ToArray();

                // dialects
                return cultures
                    .Select(culture =>
                    {
                        // init
                        var type = culture.FindBestMatch(results2, x => x.Culture);

                        // dialect
                        var dialect = locales.SingleOrDefault(x => culture.IsDialect && x.XmlLang == culture);
                        if (dialect != null)
                        {
                            // render
                            type = Compile(code, dialect, "{0}_Dialect", type, "protected", culture);
                        }

                        // done
                        return (type.Culture == culture ? type : null);
                    })
                    .Where(x => x != null).ToArray();
            }
            /// <summary>
            /// Compiles the given locale into a nested class inside the Processor class.
            /// </summary>
            /// <param name="code"></param>
            /// <param name="classNameTemplate"></param>
            /// <param name="baseClass"></param>
            /// <param name="accessor"></param>
            /// <param name="culture"></param>
            /// <returns></returns>
            private static LocaleProviderType Compile(Class code, ILocale locale, string classNameTemplate, LocaleProviderType baseClass, string accessor, Culture culture)
            {
                // init
                var className = "@" + string.Format(classNameTemplate, culture.GetMethodName());

                // class
                using (var @class = code.AppendNestedClass("{0} class {1} : {2}", accessor, className, (baseClass == null ? "LocaleProvider" : baseClass.ClassName)))
                {
                    // summary
                    @class.Summary = (culture.IsInvariant ? "Invariant." : culture.ToString());

                    // ctor()
                    using (var block = @class.AppendMethod("public {0}() : base({1})", className, Compiler.GetLiteral(culture.ToString())))
                    {
                    }

                    // ctor(string culture)
                    using (var block = @class.AppendMethod("protected {0}(Culture culture) : base(culture)", className))
                    {
                    }

                    // style options
                    CompileStyleOptions(@class, locale.StyleOptions);

                    // date parts
                    CompileDatePartParameters(@class, locale.Dates, DateFormat.Numeric);
                    CompileDatePartParameters(@class, locale.Dates, DateFormat.Text);

                    // ordinals
                    CompileOrdinals(@class, locale.Terms);
                    CompileLongOrdinals(@class, locale.Terms);

                    // terms
                    CompileTerms(@class, locale.Terms);
                    CompileTermGender(@class, locale.Terms);
                }

                // done
                return new LocaleProviderType(culture, className);
            }
            /// <summary>
            /// Compiled the style options for this locale.
            /// </summary>
            /// <param name="code"></param>
            private static void CompileStyleOptions(Class code, StyleOptionsElement styleOptions)
            {
                // init
                if (styleOptions != null)
                {
                    // limit-day-ordinals-to-day-1
                    if (styleOptions.LimitDayOrdinalsToDay1Specified)
                    {
                        code.AppendProperty("public override bool LimitDayOrdinalsToDay1", styleOptions.LimitDayOrdinalsToDay1);
                    }

                    // punctuation-in-quote
                    if (styleOptions.PunctuationInQuoteSpecified)
                    {
                        code.AppendProperty("public override bool PunctuationInQuote", styleOptions.PunctuationInQuote);
                    }
                }
            }
            /// <summary>
            /// Compiles the date part formats for this locale.
            /// </summary>
            /// <param name="code"></param>
            /// <param name="format"></param>
            private static void CompileDatePartParameters(Class code, DateElement[] dates, DateFormat format)
            {
                // match
                if (dates != null)
                {
                    var date = dates.SingleOrDefault(x => x.Format == format);
                    if (date != null)
                    {
                        using (var block = code.AppendMethod("public override IEnumerable<DatePartParameters> GetDatePartsAs{0}(Parameters p)", format))
                        {
                            foreach (var datePart in date.DateParts)
                            {
                                block.AppendIndent();
                                using (var m = block.AppendMethodInvoke("yield return new DatePartParameters", date))
                                {
                                    datePart.Compile(m);
                                }
                                block.Append(";");
                                block.AppendLineBreak();
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Compiles the ordinals for this locale.
            /// </summary>
            /// <param name="code"></param>
            private static void CompileOrdinals(Class code, TermElement[] terms)
            {
                // init
                var ordinals = (terms ?? new TermElement[] { })
                    .Where(x => x.Name.ToString().StartsWith("Ordinal"))
                    .ToArray();

                // compile?
                if (ordinals.Length > 0)
                {
                    // override
                    using (var block = code.AppendMethod("public override string FormatNumberAsOrdinal(uint value, Gender? gender)"))
                    {
                        // default ordinal
                        var defaults = ordinals.Where(x => x.Name == TermName.Ordinal).ToArray();

                        // init
                        block.AppendComment("init");
                        block.AppendLine("string suffix = {0};", GetGenderIif(defaults, defaults));

                        // find variants
                        var variants = Enumerable.Range(0, 100)
                            .Reverse()
                            .Select(i =>
                            {
                                // init
                                var result = string.Empty;
                                var name = (TermName)Enum.Parse(typeof(TermName), string.Format("Ordinal{0:00}", i));

                                // find term
                                var term = ordinals
                                    .Where(x => x.Name == name)
                                    .ToArray();

                                // match found?
                                if (term.Length > 0)
                                {
                                    // match
                                    var matches = term
                                        .Select(t => t.MatchSpecified ? t.Match : (i < 10 ? TermMatch.LastDigit : TermMatch.LastTwoDigits))
                                        .Distinct()
                                        .ToArray();
                                    if (matches.Length != 1)
                                    {
#warning Compiler Exception
                                        throw new NotSupportedException();
                                    }

                                    // conditions
                                    var condition = string.Empty;
                                    switch (matches.Single())
                                    {
                                        case TermMatch.LastDigit:
                                            condition = "(value % 10)";
                                            break;
                                        case TermMatch.LastTwoDigits:
                                            condition = "(value % 100)";
                                            break;
                                        case TermMatch.WholeNumber:
                                            condition = "value";
                                            break;
                                    }

                                    // done
                                    result = string.Format("if({0} == {1}) suffix = {2};", condition, i, GetGenderIif(defaults, term));
                                }

                                // done
                                return result;
                            })
                            .Where(x => !string.IsNullOrEmpty(x))
                            .ToArray();


                        // render
                        if (variants.Length > 0)
                        {
                            // init
                            block.AppendLineBreak();
                            block.AppendComment("conditions");

                            // variants
                            int i = 0;
                            foreach (var variant in variants)
                            {
                                block.AppendLine("{0}{1}", (i == 0 ? "" : "else "), variant);
                                i++;
                            }
                        }

                        // done
                        block.AppendLineBreak();
                        block.AppendComment("done");
                        block.AppendLine("return string.Format(\"{{0}}{{1}}\", value, suffix);");
                    }
                }
            }
            /// <summary>
            /// Compiles the long ordinals for this locale.
            /// </summary>
            /// <param name="code"></param>
            private static void CompileLongOrdinals(Class code, TermElement[] terms)
            {
                // init
                var ordinals = (terms ?? new TermElement[] { })
                    .Where(x => x.Name.ToString().StartsWith("LongOrdinal"))
                    .OrderBy(x => x.Name)
                    .ToArray();

                // compile?
                if (ordinals.Length > 0)
                {
                    // override
                    using (var method = code.AppendMethod("public override string FormatNumberAsLongOrdinal(uint value, Gender? gender)"))
                    {
                        // switch
                        using (var sw = method.AppendSwitch("value"))
                        {
                            // cases
                            foreach (var term in ordinals.GroupBy(x => x.Name))
                            {
                                // init
                                var number = int.Parse(term.Key.ToString().Substring(11));

                                // render
                                using (var block = sw.AppendCases(number))
                                {
                                    block.AppendLine("return {0};", GetGenderIif(term.ToArray(), term.ToArray()));
                                }
                            }

                            // default
                            using (var block = sw.AppendDefault())
                            {
                                block.AppendLine("return this.FormatNumberAsOrdinal(value, gender);");
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Compiles a gender based ternary operator.
            /// </summary>
            /// <param name="terms"></param>
            /// <returns></returns>
            private static string GetGenderIif(TermElement[] defaults, TermElement[] terms)
            {
                // valid?
                if (terms.Any(x => !string.IsNullOrEmpty(x.Single) || !string.IsNullOrEmpty(x.Multiple)))
                {
#warning Compiler error!
                    throw new NotSupportedException();
                }

                // render
                if (terms.Length == 0)
                {
                    return "null";
                }
                else if (terms.Length == 1 && !terms.Single().GenderFormSpecified)
                {
                    return Compiler.GetLiteral(terms.Single().Value);
                }
                else
                {
                    // defaults
                    var defaultNeuter = defaults.SingleOrDefault(x => !x.GenderFormSpecified);
                    var defaultMasculine = defaults.SingleOrDefault(x => x.GenderFormSpecified && x.GenderForm == Gender.Masculine);
                    var defaultFeminine = defaults.SingleOrDefault(x => x.GenderFormSpecified && x.GenderForm == Gender.Feminine);

                    // init
                    var neuter = terms.SingleOrDefault(x => !x.GenderFormSpecified);
                    var masculine = terms.SingleOrDefault(x => x.GenderFormSpecified && x.GenderForm == Gender.Masculine);
                    var feminine = terms.SingleOrDefault(x => x.GenderFormSpecified && x.GenderForm == Gender.Feminine);

                    // done
                    return string.Format("(gender.HasValue ? (gender.Value == Gender.Feminine ? {2} : {1}) : {0})",
                        Compiler.GetLiteral((neuter ?? defaultNeuter ?? new TermElement()).Value),
                        Compiler.GetLiteral((masculine ?? neuter ?? defaultMasculine ?? defaultNeuter).Value),
                        Compiler.GetLiteral((feminine ?? neuter ?? defaultFeminine ?? defaultNeuter).Value)
                    );
                }
            }
            /// <summary>
            /// Compiles the terms for this locale.
            /// </summary>
            /// <param name="code"></param>
            private static void CompileTerms(Class code, TermElement[] terms)
            {
                // find terms
                var matches = (terms ?? new TermElement[] { })
                    .Where(x => !x.Name.ToString().Contains("Ordinal"))
                    .GroupBy(x => x.Name)
                    .OrderBy(x => x.Key.ToString())
                    .ToArray();

                // compile?
                if (matches.Length > 0)
                {
                    // render
                    using (var method = code.AppendMethod("public override string GetTerm(TermName name, TermFormat format, bool plural)"))
                    {
                        // switch over name
                        using (var sw = method.AppendSwitch("name"))
                        {
                            // filter terms
                            foreach (var term in matches)
                            {
                                // case
                                using (var block = sw.AppendCases(term.Key))
                                {
                                    Compile(block, term.ToArray());
                                }
                            }

                            // default
                            using (var block = sw.AppendDefault())
                            {
                                block.AppendLine("return base.GetTerm(name, format, plural);");
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Compiles the term genders for this locale.
            /// </summary>
            /// <param name="code"></param>
            private static void CompileTermGender(Class code, TermElement[] terms)
            {
                // collect
                var genders = (terms ?? new TermElement[] { })
                    .Where(x => x.GenderSpecified)
                    .Select(x => new
                    {
                        Term = x.Name,
                        Gender = x.Gender
                    })
                    .GroupBy(x => x.Term)
                    .ToArray();

                // errors?
                var errors = genders
                    .Where(x => x.Distinct().Count() > 1)
                    .ToArray();
                if (errors.Length > 0)
                {
#warning To do!
                    throw new NotSupportedException();
                }

                // 
                if (genders.Length > 0)
                {
                    // render
                    using (var method = code.AppendMethod("public override Gender? GetTermGender(TermName name)"))
                    {
                        // switch over name
                        using (var sw = method.AppendSwitch("name"))
                        {
                            // filter terms
                            foreach (var gender in genders)
                            {
                                // case
                                using (var block = sw.AppendCases(gender.Key))
                                {
                                    block.AppendLine("return {0};", Compiler.GetLiteral(gender.First().Gender));
                                }
                            }

                            // default
                            using (var block = sw.AppendDefault())
                            {
                                block.AppendLine("return base.GetTermGender(name);");
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Compiles the term formats for the given term.
            /// </summary>
            /// <param name="code"></param>
            /// <param name="terms"></param>
            private static void Compile(Case code, TermElement[] terms)
            {
                if (terms.Length == 1)
                {
                    // single term
                    Compile(code, terms.Single());
                }
                else
                {
                    // fallback by format
                    var dic = new Dictionary<TermFormat, TermElement>();
                    dic.Add(TermFormat.Long, terms.SingleOrDefault(x => x.Format == TermFormat.Long));
                    dic.Add(TermFormat.Short, terms.SingleOrDefault(x => x.Format == TermFormat.Short) ?? dic[TermFormat.Long]);
                    dic.Add(TermFormat.Verb, terms.SingleOrDefault(x => x.Format == TermFormat.Verb) ?? dic[TermFormat.Long]);
                    dic.Add(TermFormat.Symbol, terms.SingleOrDefault(x => x.Format == TermFormat.Symbol) ?? dic[TermFormat.Short]);
                    dic.Add(TermFormat.VerbShort, terms.SingleOrDefault(x => x.Format == TermFormat.VerbShort) ?? dic[TermFormat.Verb]);

                    // switch
                    using (var sw = code.AppendSwitch("format"))
                    {
                        // cases
                        foreach (var term in terms)
                        {
                            // find used
                            var uses = dic
                                .Where(x => x.Value == term)
                                .Select(x => x.Key)
                                .ToArray();
                            if (uses.Length > 0)
                            {
                                using (var block = sw.AppendCases(uses.Cast<object>().ToArray()))
                                {
                                    Compile(block, term);
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }

                        // default
                        using (var block = sw.AppendDefault())
                        {
                            block.AppendLine("throw new NotSupportedException();");
                        }
                    }
                }
            }
            /// <summary>
            /// Compiles single/plural or value for the given term.
            /// </summary>
            /// <param name="block"></param>
            /// <param name="term"></param>
            private static void Compile(Scope block, TermElement term)
            {
                // single/plural?
                if (!string.IsNullOrEmpty(term.Single) && !string.IsNullOrEmpty(term.Multiple))
                {
                    // single/plural
                    block.AppendLine("return (plural ? {0} : {1});", Compiler.GetLiteral(term.Multiple), Compiler.GetLiteral(term.Single));
                }
                else
                {
                    // value
                    block.AppendLine("return {0};", Compiler.GetLiteral(term.Value ?? term.Single));
                }
            }
            #endregion
        }
    }
}