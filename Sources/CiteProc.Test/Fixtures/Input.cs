using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiteProc.Compilation;
using Newtonsoft.Json.Linq;

namespace CiteProc.Test.Fixtures
{
    public class Input : IDataProvider
    {
        private Dictionary<string, object> _Variables = new Dictionary<string, object>();
        private static readonly string[] DATE_PROPERTIES = new string[] { "date-parts", "season", "circa", "raw", "literal" };
        private static readonly string[] NAME_PROPERTIES = new string[] { "family", "given", "suffix", "dropping-particle", "non-dropping-particle", "comma-suffix", "isInstitution"};

        public static Input[] LoadJson(string json)
        {
            // deserialize array
            var array = JArray.Parse(json)
                .OfType<JObject>()
                .ToArray();

            // done
            return JArray.Parse(json)
                .OfType<JObject>()
                .Select(obj => new Input(obj))
                .ToArray();
        }

        private Input(JObject obj)
        {
            // properties
            foreach (var property in obj.Properties())
            {
                switch (property.Value.Type)
                {
                    case JTokenType.String:
                        // language?
                        if (string.Compare(property.Name, "language", true) == 0)
                        {
                            // init
                            var culture = property.Value.Value<JValue>().Value.ToString().Split(' ').First();

                            // culture
                            this.Culture = new Culture(culture);
                        }
                        else
                        {
                            // init
                            var value = property.Value.Value<JValue>().Value;

                            // inline html?
                            if (value is string && ((string)value).Contains("<"))
                            {
                                throw new FeatureNotSupportedException("inline html");
                            }

                            // add
                            this._Variables.Add(property.Name, value);
                        }
                        break;
                    case JTokenType.Object:
                        this._Variables.Add(property.Name, this.ParseObject(property.Value.Value<JObject>()));
                        break;
                    case JTokenType.Array:
                        this._Variables.Add(property.Name, this.ParseArray(property.Value.Value<JArray>()));
                        break;
                    case JTokenType.Integer:
                        this._Variables.Add(property.Name, property.Value.Value<JValue>().Value);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
        private object[] ParseArray(JArray array)
        {
            return array
                .Cast<JObject>()
                .Select(x => this.ParseObject(x))
                .ToArray();
        }
        private object ParseObject(JObject obj)
        {
            // property names
            var properties = obj.Properties()
                .Select(x => x.Name)
                .ToArray();

            if (properties.All(x => DATE_PROPERTIES.Contains(x)))
            {
                // raw?
                if (obj["raw"] != null)
                {
                    // raw
                    return this.GetValueAsString(obj["raw"]);
                    //throw new FeatureNotSupportedException("json-date-raw");
                }
                if (obj["literal"] != null)
                {
                    // literal
                    return this.GetValueAsString(obj["literal"]);
                }
                else
                {
                    // date
                    var dates = obj["date-parts"].Value<JArray>()
                        .Cast<JArray>()
                        .Select(d =>
                        {
                            return d.Values<JValue>()
                                .Select(x => (int?)int.Parse(x.Value.ToString()))
                                .Concat(new int?[] { null, null })
                                .Take(3)
                                .ToArray();
                        })
                        .ToArray();

                    // season
                    var season = this.GetValueAsSeason(obj["season"]);

                    // circa
                    var circa = this.GetValueAsBool(obj["circa"]);

                    // done
                    switch (dates.Length)
                    {
                        case 0:
                            return null;
                        case 1:
                            return new DateVariable(dates[0][0].Value, season, dates[0][1], dates[0][2], circa);
                        case 2:
                            return new DateVariable(dates[0][0].Value, season, dates[0][1], dates[0][2], dates[1][0].Value, season, dates[1][1], dates[1][2], circa);
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
            else if (properties.All(x => NAME_PROPERTIES.Contains(x)))
            {
                // name
                if (obj["isInstitution"] != null && obj["isInstitution"].Type == JTokenType.Boolean && this.GetValueAsBool(obj["isInstitution"]))
                {
                    return this.GetValueAsString(obj["family"]);
                }
                else
                {
                    // personal name
                    return new NameVariable(
                        this.GetValueAsString(obj["family"]),
                        this.GetValueAsString(obj["given"]),
                        this.GetValueAsString(obj["suffix"]),
                        this.GetValueAsBool(obj["comma-suffix"]),
                        this.GetValueAsString(obj["dropping-particle"]),
                        this.GetValueAsString(obj["non-dropping-particle"])
                    );
                }
            }
            else
            {
                throw new FeatureNotSupportedException(string.Format("json-{0}", string.Join(":", properties)));
            }
        }
        private string GetValueAsString(JToken property)
        {
            return (property == null ? null : property.Value<JValue>().Value.ToString());
        }
        private bool GetValueAsBool(JToken property)
        {
            // init
            var result = false;

            // parse
            if (property != null)
            {
                // cast
                var value = property.Value<JValue>().Value;

                // type?
                if (value is bool)
                {
                    result = (bool)value;
                }
                else if (value is string)
                {
                    result = ((string)value == "1" || string.Compare((string)value, "true", true) == 0);
                }
                else if (value is long)
                {
                    result = ((long)value == 1L);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            // done
            return result;
        }
        private Season? GetValueAsSeason(JToken property)
        {
            // init
            if (property == null)
            {
                return null;
            }

            // parse
            var value = property.Value<JValue>().Value;
            if (value is long)
            {
                return (Season)(int)(long)value;
            }
            else if (value is string)
            {
                return (Season)Enum.Parse(typeof(Season), (string)value, true);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public Culture Culture
        {
            get;
            private set;
        }
        public object GetVariable(string name)
        {
            // init
            var matches = this._Variables
                .Where(x => string.Compare(x.Key, name, true) == 0)
                .ToArray();

            // done
            return (matches.Length > 0 ? matches.Single().Value : null);
        }
    }
}