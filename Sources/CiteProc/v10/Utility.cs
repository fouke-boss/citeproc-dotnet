using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CiteProc.Compilation;

namespace CiteProc.v10
{
    internal static class Utility
    {
        private static Dictionary<Type, Dictionary<string, object>> _XmlEnumCache = new Dictionary<Type, Dictionary<string, object>>();

        public static Method AppendRenderingMethod(this Class @class, string methodName, string returnType, string accessor)
        {
            return @class.AppendMethod("{0} {1} {2}(ExecutionContext {3}, Parameters {4})", accessor, returnType, methodName, Compiler.CONTEXT_NAME, Compiler.PARAMETER_NAME);
        }

        public static void AddSortComparerParameters(this MethodInvoke method, SortElement sort)
        {
            // find orders
            var keys = sort.Keys ?? new KeyElement[] { };

            // add
            foreach (var key in keys)
            {
                method.AddLiteral(key.SortOrderSpecified ? key.SortOrder : SortOrder.Ascending);
            }
        }
#warning Overbodig?
        public static void AddSortComparer(this MethodInvoke method, EntryElement element)
        {
            // find orders
            var orders = (element == null || element.Sort == null || element.Sort.Keys == null ? new KeyElement[] { } : element.Sort.Keys)
            .Select(key =>
            {
                // init
                var order = (key.SortOrderSpecified ? key.SortOrder : SortOrder.Ascending);

                // done
                return Compiler.GetLiteral(order);
            })
            .ToArray();

            // done
            method.AddCode("new SortComparer({0})", string.Join(", ", orders));
        }

        public static T? GetXmlEnum<T>(this string value)
            where T : struct
        {
            // init
            var type = typeof(T);

            // from cache
            if (!_XmlEnumCache.ContainsKey(type))
            {
                // enum?
                if (!type.IsEnum)
                {
                    throw new NotSupportedException();
                }

                // get values
                var values = type
                    .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .ToDictionary(field =>
                    {
                        // get xml name
                        return field
                            .GetCustomAttributes(true)
                            .OfType<XmlEnumAttribute>()
                            .Single()
                            .Name;
                    }, field =>
                    {
                        // get enum value
                        return field.GetValue(null);
                    });

                // done
                _XmlEnumCache.Add(type, values);
            }

            // find
            var dictionary = _XmlEnumCache[type];

            // done
            return (dictionary.ContainsKey(value) ? (T)dictionary[value] : (T?)null);
        }

        public static TermName? GetTerm(string name)
        {
            return name.GetXmlEnum<TermName>();
            //// find field
            //var field = typeof(TermName)
            //    .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            //    .SingleOrDefault(x => string.Compare(x.GetCustomAttributes(true).OfType<XmlEnumAttribute>().Single().Name, name, true) == 0);

            //// done
            //return (field == null ? (TermName?)null : (TermName)field.GetValue(null));
        }

        public static string GetMethodName(this Culture culture)
        {
            return (culture.IsInvariant ? "Invariant" : culture.ToString().Replace('-', '_'));
        }
        public static T FindBestMatch<T>(this Culture culture, IEnumerable<T> items, Func<T, Culture> selector)
        {
            // init
            var dict = items.ToDictionary(x => selector(x), x => x);

            // done
            if (dict.ContainsKey(culture))
            {
                return dict[culture];
            }
            else if (dict.ContainsKey(culture.Language))
            {
                return dict[culture.Language];
            }
            else
            {
                return dict[Culture.Invariant];
            }
        }

        public static void AddContextAndParameters(this MethodInvoke owner)
        {
            // context
            owner.AddCode(Compiler.CONTEXT_NAME);

            // parameters
            using (var method = owner.AddMethodInvoke("new Parameters", null))
            {
                method.AddCode(method.ParameterName);
                method.AddDefaultParameters();
            }
        }
        public static void AddDefaultParameters(this MethodInvoke owner)
        {
            // init
            var context = owner.Context;

            // IFormattable
            owner.AddFromContext<IFormatting>(context, "fontStyle", x => x.FontStyleSpecified, x => x.FontStyle);
            owner.AddFromContext<IFormatting>(context, "fontVariant", x => x.FontVariantSpecified, x => x.FontVariant);
            owner.AddFromContext<IFormatting>(context, "fontWeight", x => x.FontWeightSpecified, x => x.FontWeight);
            owner.AddFromContext<IFormatting>(context, "textDecoration", x => x.TextDecorationSpecified, x => x.TextDecoration);
            owner.AddFromContext<IFormatting>(context, "verticalAlign", x => x.VerticalAlignSpecified, x => x.VerticalAlign);

            // strip periods
            owner.AddFromContext<IStripPeriods>(context, "stripPeriods", x => x.StripPeriodsSpecified, x => x.StripPeriods);

            // INamesOptions
            owner.AddFromContext<INamesOptions>(context, "namesDelimiter", x => x.Delimiter != null, x => x.Delimiter);

            // INameOptions
            owner.AddFromContext<INameOptions>(context, "and", x => x.AndSpecified, x => x.And);
            owner.AddFromContext<INameOptions>(context, "nameDelimiter", x => x.Delimiter != null, x => x.Delimiter);
            owner.AddFromContext<INameOptions>(context, "delimiterPrecedesEtAl", x => x.DelimiterPrecedesEtAlSpecified, x => x.DelimiterPrecedesEtAl);
            owner.AddFromContext<INameOptions>(context, "delimiterPrecedesLast", x => x.DelimiterPrecedesLastSpecified, x => x.DelimiterPrecedesLast);

            owner.AddFromContext<IEtAlOptions>(context, "etAlMin", x => x.EtAlMinSpecified, x => x.EtAlMin);
            owner.AddFromContext<IEtAlOptions>(context, "etAlUseFirst", x => x.EtAlUseFirstSpecified, x => x.EtAlUseFirst);
            owner.AddFromContext<IEtAlOptions>(context, "etAlSubsequentMin", x => x.EtAlSubsequentMinSpecified, x => x.EtAlSubsequentMin);
            owner.AddFromContext<IEtAlOptions>(context, "etAlSubsequentUseFirst", x => x.EtAlSubsequentUseFirstSpecified, x => x.EtAlSubsequentUseFirst);
            owner.AddFromContext<IEtAlOptions>(context, "etAlUseLast", x => x.EtAlUseLastSpecified, x => x.EtAlUseLast);

            owner.AddFromContext<INameOptions>(context, "nameFormat", x => x.FormatSpecified, x => x.Format);
            owner.AddFromContext<INameOptions>(context, "initialize", x => x.InitializeSpecified, x => x.Initialize);
            owner.AddFromContext<INameOptions>(context, "initializeWith", x => x.InitializeWith != null, x => x.InitializeWith);
            owner.AddFromContext<INameOptions>(context, "nameAsSortOrder", x => x.NameAsSortOrderSpecified, x => x.NameAsSortOrder);
            owner.AddFromContext<INameOptions>(context, "sortSeparator", x => x.SortSeparator != null, x => x.SortSeparator);
        }
        private static void AddFromContext<T>(this MethodInvoke owner, object context, string parameterName, Predicate<T> specified, Func<T, object> value)
        {
            if (context is T && specified((T)context))
            {
                owner.AddCode("{0}: {1}", parameterName, Compiler.GetLiteral(value((T)context)));
            }
        }
    }
}
