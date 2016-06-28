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
            // init
            var name = element.GetType().Name.ToLower();

            // done
            this.AddLiteral(string.Format("<{0}>{1}", name.Substring(0, name.Length - 7), element.Tag));
        }

//#warning deze 3 methods naar v1.0 verplaatsen.
//        public void AddContextAndParameters()
//        {
//            // context
//            this.AddCode(Compiler.CONTEXT_NAME);

//            // parameters
//            using (var method = this.AddMethodInvoke("new Parameters", null))
//            {
//                method.AddCode(method.ParameterName);
//                method.AddDefaultParameters();
//            }
//        }
//        public void AddDefaultParameters()
//        {
//            // init
//            var context = this.Context;

//            // IFormattable
//            this.AddFromContext<IFormatting>(context, "fontStyle", x => x.FontStyleSpecified, x => x.FontStyle);
//            this.AddFromContext<IFormatting>(context, "fontVariant", x => x.FontVariantSpecified, x => x.FontVariant);
//            this.AddFromContext<IFormatting>(context, "fontWeight", x => x.FontWeightSpecified, x => x.FontWeight);
//            this.AddFromContext<IFormatting>(context, "textDecoration", x => x.TextDecorationSpecified, x => x.TextDecoration);
//            this.AddFromContext<IFormatting>(context, "verticalAlign", x => x.VerticalAlignSpecified, x => x.VerticalAlign);

//            // strip periods
//            this.AddFromContext<IStripPeriods>(context, "stripPeriods", x => x.StripPeriodsSpecified, x => x.StripPeriods);

//            // INamesOptions
//            this.AddFromContext<INamesOptions>(context, "namesDelimiter", x => x.Delimiter != null, x => x.Delimiter);

//            // INameOptions
//            this.AddFromContext<INameOptions>(context, "and", x => x.AndSpecified, x => x.And);
//            this.AddFromContext<INameOptions>(context, "nameDelimiter", x => x.Delimiter != null, x => x.Delimiter);
//            this.AddFromContext<INameOptions>(context, "delimiterPrecedesEtAl", x => x.DelimiterPrecedesEtAlSpecified, x => x.DelimiterPrecedesEtAl);
//            this.AddFromContext<INameOptions>(context, "delimiterPrecedesLast", x => x.DelimiterPrecedesLastSpecified, x => x.DelimiterPrecedesLast);

//            this.AddFromContext<IEtAlOptions>(context, "etAlMin", x => x.EtAlMinSpecified, x => x.EtAlMin);
//            this.AddFromContext<IEtAlOptions>(context, "etAlUseFirst", x => x.EtAlUseFirstSpecified, x => x.EtAlUseFirst);
//            this.AddFromContext<IEtAlOptions>(context, "etAlSubsequentMin", x => x.EtAlSubsequentMinSpecified, x => x.EtAlSubsequentMin);
//            this.AddFromContext<IEtAlOptions>(context, "etAlSubsequentUseFirst", x => x.EtAlSubsequentUseFirstSpecified, x => x.EtAlSubsequentUseFirst);
//            this.AddFromContext<IEtAlOptions>(context, "etAlUseLast", x => x.EtAlUseLastSpecified, x => x.EtAlUseLast);

//            this.AddFromContext<INameOptions>(context, "nameFormat", x => x.FormatSpecified, x => x.Format);
//            this.AddFromContext<INameOptions>(context, "initialize", x => x.InitializeSpecified, x => x.Initialize);
//            this.AddFromContext<INameOptions>(context, "initializeWith", x => x.InitializeWith != null, x => x.InitializeWith);
//            this.AddFromContext<INameOptions>(context, "nameAsSortOrder", x => x.NameAsSortOrderSpecified, x => x.NameAsSortOrder);
//            this.AddFromContext<INameOptions>(context, "sortSeparator", x => x.SortSeparator != null, x => x.SortSeparator);
//        }
//        private void AddFromContext<T>(object context, string parameterName, Predicate<T> specified, Func<T, object> value)
//        {
//            if (context is T && specified((T)context))
//            {
//                this.AddCode("{0}: {1}", parameterName, Compiler.GetLiteral(value((T)context)));
//            }
//        }

        public MethodInvoke AddMethodInvoke(string methodName, Element context)
        {
            // init
            var result = new MethodInvoke(this, context, methodName);

            // add
            this._Parameters.Add(result);

            // done
            return result;
        }
        public LambdaExpression AddLambdaExpression(bool addBraces, string prefix = null)
        {
            // init
            var result = new LambdaExpression(this, addBraces, prefix);

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
            bool multiline = false;

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

                // multiline?
                multiline |= (value is LambdaExpression && !((LambdaExpression)value).IsArray);

                // next
                index++;
            }

            // end
            if (multiline)
            {
                writer.Append(Environment.NewLine);
                writer.AppendIndent();
            }

            // done
            writer.Append(")");
        }
    }
}