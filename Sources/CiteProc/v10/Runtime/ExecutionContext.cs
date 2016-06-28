using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class ExecutionContext : IDataProvider
        {
            private IDataProvider _DataProvider;
            private Dictionary<string, object> _Cache = new Dictionary<string, object>();

            public ExecutionContext(IDataProvider dataProvider, LocaleProvider locale)
            {
                // init
                this._DataProvider = dataProvider;
                this.Culture = dataProvider.Culture;
                this.Locale = locale;
            }

            public LocaleProvider Locale
            {
                get;
                private set;
            }
            public Culture Culture
            {
                get;
                private set;
            }

            /// <summary>
            /// Get the value from the underlying data provider, and casts it into one of four types:
            /// - IDateVariable
            /// - INumberVariable
            /// - object[], either string or INameVariable
            /// - a string
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public object GetVariable(string name)
            {
                // cache?
                if (!this._Cache.ContainsKey(name))
                {
                    // init
                    object result = null;

                    // first?
                    if (name.Length > 6 && string.Compare(name.Substring(name.Length - 6), "-first", true) == 0)
                    {
                        // get page variable
                        object value = this.GetVariable(name.Substring(0, name.Length - 6));

                        // done
                        result = (value is INumberVariable ? (object)((INumberVariable)value).Min : null);
                    }
                    else
                    {
                        // get variable
                        object value = this._DataProvider.GetVariable(name);

                        // parse
                        if (value == null)
                        {
                            result = null;
                        }
                        else if (value is IDateVariable || value is INumberVariable)
                        {
                            result = value;
                        }
                        else if (value is INameVariable)
                        {
                            result = new object[] { value };
                        }
                        else if (value is IEnumerable && !(value is string))
                        {
                            // names
                            result = ((IEnumerable)value)
                                .Cast<object>()
                                .Select(v => (v is INameVariable ? v : v.ToString()))
                                .ToArray();
                        }
                        else
                        {
                            // init
                            var text = value.ToString().Trim();

                            // number?
                            if (result == null)
                            {
                                result = this.ParseVariableAsNumber(value, text);
                            }

                            // date?
                            if (result == null)
                            {
                                result = this.ParseVariableAsDate(value, text);
                            }

                            // string
                            if (result == null)
                            {
                                // done
                                result = (string.IsNullOrEmpty(text) ? null : text);
                            }
                        }
                    }

                    // done
                    this._Cache.Add(name, result);
                }

                // done
                return this._Cache[name];
            }
            /// <summary>
            /// Returns the variable with the given name as a string.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public string GetVariableAsText(string name)
            {
                // init
                var result = this.GetVariable(name);

                // done
                return (result is string || result == null ? (string)result : result.ToString());
            }
            /// <summary>
            /// Returns the variable with the given name as either a IDateVariable or a string.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public object GetVariableAsDate(string name)
            {
                return this.GetVariableAsT<IDateVariable>(name);
            }
            /// <summary>
            /// Returns the variable with the given name as either a INumberVariable or a string.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public object GetVariableAsNumber(string name)
            {
                return this.GetVariableAsT<INumberVariable>(name);
            }
            private object GetVariableAsT<T>(string name)
            {
                // init
                var result = this.GetVariable(name);

                // done
                return (result is T || result == null ? result : result.ToString());
            }
            /// <summary>
            /// Returns the variable with the given name as object[].
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public object[] GetVariableAsNames(string name)
            {
                // init
                var result = this.GetVariable(name);

                // done
                return (result is object[] || result == null ? (object[])result : new object[] { result.ToString() });
            }

            public bool IsNotNull(string name)
            {
                return (this.GetVariable(name) != null);
            }
            public bool IsNumeric(string name)
            {
                return (this.GetVariableAsNumber(name) is INumberVariable);
            }
            public bool IsUncertainDate(string name)
            {
                // init
                var date = this.GetVariableAsDate(name);

                // done
                return (date is IDateVariable && ((IDateVariable)date).IsApproximate);
            }
            public bool IsType(string type)
            {
                return (string.Compare(this.GetVariableAsText("type"), type, true) == 0);
            }
            public bool IsLocator(string locator)
            {
                return (string.Compare(this.GetVariableAsText("locator"), locator, true) == 0);
            }
            public bool IsPosition(Position position)
            {
                return (position == Position.First);
            }

            private IDateVariable ParseVariableAsDate(object value, string text)
            {
                // init
                DateTime date;

                // done
                if (value is DateTime)
                {
                    return new DateVariable((DateTime)value);
                }
                else if (value is IEnumerable<DateTime>)
                {
                    // init
                    var min = ((IEnumerable<DateTime>)value).Min();
                    var max = ((IEnumerable<DateTime>)value).Max();

                    // done
                    return new DateVariable(min, max);
                }
                else if (DateTime.TryParse(text, out date))
                {
                    return new DateVariable(date);
                }
                else
                {
                    return null;
                }
            }
            private INumberVariable ParseVariableAsNumber(object value, string text)
            {
                // parse
                if (value is uint)
                {
                    return new NumberVariable((uint)value);
                }
                else if (value is int)
                {
                    return new NumberVariable((uint)(int)value);
                }
                else if (value is ulong)
                {
                    return new NumberVariable((uint)(ulong)value);
                }
                else if (value is long)
                {
                    return new NumberVariable((uint)(long)value);
                }
                else if (value is ushort)
                {
                    return new NumberVariable((ushort)value);
                }
                else if (value is short)
                {
                    return new NumberVariable((uint)(short)value);
                }
                else if (value is byte)
                {
                    return new NumberVariable((byte)value);
                }
                else if (value is sbyte)
                {
                    return new NumberVariable((uint)(sbyte)value);
                }
                else
                {
                    // number?
                    var single = this.TryParseNumber(text);
                    if (single.HasValue)
                    {
                        return new NumberVariable(single.Value);
                    }

                    // init
                    INumberVariable result = null;

                    // separator?
                    if (result == null)
                    {
                        result = this.TrySplitNumber(text, ",");
                    }
                    if (result == null)
                    {
                        result = this.TrySplitNumber(text, "&");
                    }
                    if (result == null)
                    {
                        result = this.TrySplitNumber(text, "-–");
                    }

                    // done
                    return result;
                }
            }
            private NumberVariable? TrySplitNumber(string text, string separators)
            {
                // init
                var parts = text
                    .Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => this.TryParseNumber(x.Trim()))
                    .Where(x => x.HasValue)
                    .ToArray();

                // done
                return (parts.Length == 2 ? new NumberVariable(parts[0].Value, parts[1].Value, separators.First()) : (NumberVariable?)null);
            }
            private uint? TryParseNumber(string text)
            {
                // init
                var result = (uint?)null;

                // try/parse
                uint dummy;
                if (uint.TryParse(text, out dummy))
                {
                    result = dummy;
                }

                // done
                return result;
            }
        }
    }
}