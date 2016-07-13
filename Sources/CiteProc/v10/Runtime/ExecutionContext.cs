using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class ExecutionContext
        {
            private IDataProvider _DataProvider;
            private Dictionary<string, IVariable> _Variables;

            public ExecutionContext(IDataProvider dataProvider, LocaleProvider locale, DisambiguationContext disambiguationContext)
                : this(dataProvider, locale, disambiguationContext, null)
            {
            }
            public ExecutionContext(IDataProvider dataProvider, LocaleProvider locale, DisambiguationContext disambiguationContext, ExecutionContext previous)
            {
                // init
                this._DataProvider = dataProvider;
                this.Locale = locale;
                this.DisambiguationContext = disambiguationContext;
                this.Previous = previous;

                // get variables
                var variables = dataProvider.GetVariables();

                // add -first variables for numbers
                var variablesFirst = variables
                    .Where(x => x.Value is INumberVariable)
                    .ToDictionary(x => string.Format("{0}-first", x.Key), x => (IVariable)new Data.NumberVariable(((INumberVariable)x.Value).Min));

                // done
                this._Variables = variables.Concat(variablesFirst)
                    .ToDictionary(x => x.Key.ToLower(), x => x.Value);
            }

            public Culture Culture
            {
                get
                {
                    return this._DataProvider.Culture;
                }
            }
            public LocaleProvider Locale
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the variable with the given name from the underlying data provider as one of 4 types
            /// - ITextVariable.
            /// - IDateVariable.
            /// - INumberVariable.
            /// - INamesVariable.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public IVariable GetVariable(string name)
            {
                return (this._Variables.ContainsKey(name) ? this._Variables[name] : null);
            }
            public IVariable GetVariable(string name, Predicate<IVariable> valid, string error)
            {
                // init
                var result = this.GetVariable(name);

                // valid?
                if (result == null || valid(result))
                {
                    // done
                    return result;
                }
                else
                {
                    // error
                    throw new ArgumentOutOfRangeException(string.Format("'{0}' is not a valid {1} variable.", name, error));
                }
            }
            public void SuppressVariable(string name)
            {
                if (this._Variables.ContainsKey(name))
                {
                    this._Variables[name] = null;
                }
                else
                {
                    this._Variables.Add(name, null);
                }
            }

            public bool IsNotNull(string name)
            {
                return (this.GetVariable(name) != null);
            }
            public bool IsNumeric(string name)
            {
                return (this.GetVariable(name) is INumberVariable);
            }
            public bool IsUncertainDate(string name)
            {
                // init
                var date = this.GetVariable(name);

                // done
                return (date is IDateVariable && ((IDateVariable)date).IsApproximate);
            }
            public bool IsType(string type)
            {
                // init
                var text = this.GetVariable("type") as ITextVariable;

                // done
                return (text != null && (string.Compare(text.Value, type, true) == 0));
            }
            public bool IsLocator(string locator)
            {
                // init
                var text = this.GetVariable("locator") as ITextVariable;

                // done
                return (text != null && (string.Compare(text.Value, locator, true) == 0));
            }
            public bool IsPosition(Position position)
            {
                return (position == Position.First);
            }

            public DisambiguationContext DisambiguationContext
            {
                get;
                private set;
            }
            public NameGroup[] FirstBibliographyNameGroups
            {
                get;
                set;
            }
            public ExecutionContext Previous
            {
                get;
                private set;
            }
        }
    }
}