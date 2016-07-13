using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    internal class JsonDataFormat : DataFormat
    {
        protected internal override IEnumerable<DataProvider> Parse(System.IO.TextReader reader)
        {
            // use reader
            using (var jr = new IO.JsonReader(reader))
            {
                switch (jr.NodeType)
                {
                    case IO.JsonNodeType.StartOfArray:
                        return this.ParseArray(jr).ToArray();
                    case IO.JsonNodeType.StartOfObject:
                        return new DataProvider[] { this.ParseObject(jr) };
                    default:
                        throw new DataFormatException(jr, "Array or object expected.");
                }
            }
        }
        private IEnumerable<DataProvider> ParseArray(IO.JsonReader jr)
        {
            // next
            jr.Read();

            // read objects
            while (jr.NodeType == IO.JsonNodeType.StartOfObject)
            {
                yield return this.ParseObject(jr);
            }

            // ok?
            jr.Read(IO.JsonNodeType.EndOfArray);
        }
        private DataProvider ParseObject(IO.JsonReader jr)
        {
            // init
            var result = new DataProvider();

            // skip start of object
            jr.Read();

            // read properties
            while (jr.NodeType == IO.JsonNodeType.PropertyName)
            {
                // get property name
                var name = (string)jr.Value;

                // translate
                if (name == "journalAbbreviation")
                {
                    name = "container-title-short";
                }
                else if (name == "shortTitle")
                {
                    name = "title-short";
                }

                // read value
                jr.Read();

                // culture?
                if (name == "language")
                {
                    // culture
                    result.Culture = new Culture(jr.ReadAsString());
                }
                else
                {
                    // value
                    IVariable value = null;

                    // null?
                    if (jr.NodeType != IO.JsonNodeType.Null)
                    {
                        // variable type?
                        var variableType = DataProvider.GetVariableType(name);
                        if (variableType == typeof(ITextVariable))
                        {
                            // text
                            value = this.ParseTextVariable(jr);
                        }
                        else if (variableType == typeof(IDateVariable))
                        {
                            // date
                            value = this.ParseDateVariable(jr);
                        }
                        else if (variableType == typeof(INumberVariable))
                        {
                            // number
                            value = this.ParseNumberVariable(jr);
                        }
                        else if (variableType == typeof(INamesVariable))
                        {
                            // names
                            value = this.ParseNamesVariable(jr);
                        }
                        else if (variableType == null)
                        {
                            throw new Data.DataFormatException(jr, "Unsupported variable '{0}'.", name);
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }

                    // set property
                    result[name] = value;
                }
            }

            // done
            jr.Read(IO.JsonNodeType.EndOfObject);

            // done
            return result;
        }
        private IVariable ParseTextVariable(IO.JsonReader jr)
        {
            // done
            return new TextVariable(jr.ReadAsString());
        }
        private IVariable ParseNumberVariable(IO.JsonReader jr)
        {
            // init
            IVariable result = null;

            // type?
            switch (jr.NodeType)
            {
                case IO.JsonNodeType.Integer:
                    result = new NumberVariable((uint)(int)jr.Value);
                    break;
                case IO.JsonNodeType.String:
                    result = NumberVariable.TryParse((string)jr.Value);
                    if (result == null)
                    {
                        result = new TextVariable((string)jr.Value);
                    }
                    break;
                default:
                    throw new DataFormatException(jr, "Integer or string value expected.");
            }

            // read
            jr.Read();

            // done
            return result;
        }
        private INamesVariable ParseNamesVariable(IO.JsonReader jr)
        {
            // init
            var results = new List<Name>();

            // start of array
            jr.Read(IO.JsonNodeType.StartOfArray);

            // objects are names
            while (jr.NodeType == IO.JsonNodeType.StartOfObject)
            {
                // skip
                jr.Read();

                // init
                string family = null;
                string given = null;
                string suffix = null;
                bool? commaSuffix = null;
                string droppingParticles = null;
                string nonDroppingParticles = null;
                bool? isInstitution = null;
                string literal = null;

                // properties
                while (jr.NodeType == IO.JsonNodeType.PropertyName)
                {
                    // init
                    var propertyName = (string)jr.Value;

                    // skip
                    jr.Read();

                    // read value
                    switch (propertyName.ToLower())
                    {
                        case "family":
                            family = jr.ReadAsString();
                            break;
                        case "given":
                            given = jr.ReadAsString();
                            break;
                        case "suffix":
                            suffix = jr.ReadAsString();
                            break;
                        case "dropping-particle":
                            droppingParticles = jr.ReadAsString();
                            break;
                        case "non-dropping-particle":
                            nonDroppingParticles = jr.ReadAsString();
                            break;
                        case "comma-suffix":
                            commaSuffix = jr.ReadAsBoolean();
                            break;
                        case "isinstitution":
                            isInstitution = jr.ReadAsBoolean();
                            break;
                        case "literal":
                            literal = jr.ReadAsString();
                            break;
                        default:
                            throw new DataFormatException(jr, "Unexpected property '{0}'.", propertyName);
                    }
                }

                // done
                if (isInstitution ?? false)
                {
                    // institution
                    if (family == null)
                    {
                        throw new DataFormatException(jr, "The family property is required for institutional names.");
                    }
                    else if (!string.IsNullOrEmpty(given) || !string.IsNullOrEmpty(suffix) || commaSuffix.HasValue || !string.IsNullOrEmpty(droppingParticles) || !string.IsNullOrEmpty(nonDroppingParticles))
                    {
                        throw new DataFormatException(jr, "Only the family property is allowed for institutional names.");
                    }

                    // done
                    results.Add(new InstitutionalName(family));
                }
                else if (literal != null)
                {
                    // institiution
                    if (!string.IsNullOrEmpty(family) || !string.IsNullOrEmpty(given) || !string.IsNullOrEmpty(suffix) || commaSuffix.HasValue || !string.IsNullOrEmpty(droppingParticles) || !string.IsNullOrEmpty(nonDroppingParticles))
                    {
                        throw new DataFormatException(jr, "No other proeprties are allowed for literal names.");
                    }

                    // done
                    results.Add(new InstitutionalName(literal));
                }
                else
                {
                    // personal
                    results.Add(new PersonalName(family, given, suffix, commaSuffix ?? false, droppingParticles, nonDroppingParticles));
                }

                // read end of object
                jr.Read(IO.JsonNodeType.EndOfObject);
            }

            // end of array
            jr.Read(IO.JsonNodeType.EndOfArray);

            // done
            return new NamesVariable(results);
        }
        private IVariable ParseDateVariable(IO.JsonReader jr)
        {
            // init
            IVariable result = null;

            // node type
            switch (jr.NodeType)
            {
                case IO.JsonNodeType.String:
                    result = new TextVariable((string)jr.Value);
                    jr.Read();
                    break;
                case IO.JsonNodeType.StartOfObject:
                    // init
                    int?[][] dateParts = new int?[][] { };
                    bool? circa = null;
                    DateVariable? raw = null;
                    string literal = null;
                    Season? season = null;

                    // skip
                    jr.Read();

                    // read properties
                    while (jr.NodeType == IO.JsonNodeType.PropertyName)
                    {
                        // init
                        var propertyName = (string)jr.Value;
                        jr.Read();

                        // property
                        switch (propertyName.ToLower())
                        {
                            case "date-parts":
                                dateParts = this.ParseDateVariableDateParts(jr);
                                break;
                            case "raw":
                                raw = ParseRawDate(jr);
                                break;
                            case "literal":
                                literal = jr.ReadAsString();
                                break;
                            case "circa":
                                circa = jr.ReadAsBoolean();
                                break;
                            case "season":
                                season = this.ParseSeason(jr);
                                break;
                            default:
                                throw new DataFormatException(jr, "Unexpected property '{0}'.", propertyName);
                        }
                    }

                    // valid date?
                    if (raw.HasValue)
                    {
                        // raw
                        if (dateParts.Length > 0 || literal != null)
                        {
                            throw new DataFormatException(jr, "Date parts and literal properties are not allowed for raw date variables.");
                        }

                        // done
                        result = new DateVariable(raw.Value.YearFrom, raw.Value.SeasonFrom, raw.Value.MonthFrom, raw.Value.DayFrom, raw.Value.YearTo, raw.Value.SeasonTo, raw.Value.MonthTo, raw.Value.DayTo, circa ?? false);
                    }
                    else if (literal != null)
                    {
                        // literal
                        if (dateParts.Length > 0 || raw != null || circa != null)
                        {
                            throw new DataFormatException(jr, "Date parts, raw and circa properties are not allowed for literal date variables.");
                        }

                        // done
                        result = new TextVariable(literal);
                    }
                    else
                    {
                        // dateparts
                        switch (dateParts.Length)
                        {
                            case 0:
                                result = null;
                                break;
                            case 1:
                                result = new DateVariable(dateParts[0][0].Value, season, dateParts[0][1], dateParts[0][2], circa ?? false);
                                break;
                            case 2:
                                result = new DateVariable(dateParts[0][0].Value, season, dateParts[0][1], dateParts[0][2], dateParts[1][0].Value, season, dateParts[1][1], dateParts[1][2], circa ?? false);
                                break;
                            default:
                                throw new DataFormatException(jr, "Invalid date variable.");
                        }
                    }

                    // end object
                    jr.Read(IO.JsonNodeType.EndOfObject);

                    break;
                default:
                    throw new DataFormatException(jr, "Json token '{0}' cannot be parsed into a text variable.", jr.NodeType);
            }

            // done
            return result;
        }
        private int?[][] ParseDateVariableDateParts(IO.JsonReader jr)
        {
            // start of outer array expected
            jr.Read(IO.JsonNodeType.StartOfArray);

            // init
            var results = new List<int?[]>();
            while (jr.NodeType == IO.JsonNodeType.StartOfArray)
            {
                // max 2
                if (results.Count > 2)
                {
                    throw new DataFormatException(jr, "At most 2 inner arrays are allowed.");
                }

                // skip
                jr.Read();

                // read values
                var value = new List<int?>();
                while (jr.NodeType == IO.JsonNodeType.Integer || jr.NodeType == IO.JsonNodeType.String)
                {
                    // max 3
                    if (value.Count > 3)
                    {
                        throw new DataFormatException(jr, "At most 3 date part values are allowed.");
                    }

                    // parse
                    value.Add(jr.ReadAsInteger());
                }

                // add
                results.Add(value.Concat(new int?[] { null, null }).Take(3).ToArray());

                // read end of inner array
                jr.Read(IO.JsonNodeType.EndOfArray);
            }

            // read end of outer array
            jr.Read(IO.JsonNodeType.EndOfArray);

            // done
            return results.ToArray();
        }
        private Season ParseSeason(IO.JsonReader jr)
        {
            switch (jr.NodeType)
            {
                case IO.JsonNodeType.Integer:
                    switch ((int)jr.Value)
                    {
                        case 1:
                            return Season.Spring;
                        case 2:
                            return Season.Summer;
                        case 3:
                            return Season.Autumn;
                        case 4:
                            return Season.Winter;
                        default:
                            throw new DataFormatException(jr, "Invalid season: '{0}'", jr.Value);
                    }
                case IO.JsonNodeType.String:
                    switch (((string)jr.Value).ToLower())
                    {
                        case "spring":
                            return Season.Spring;
                        case "summer":
                            return Season.Summer;
                        case "autumn":
                            return Season.Autumn;
                        case "winter":
                            return Season.Winter;
                        default:
                            throw new DataFormatException(jr, "Invalid season: '{0}'", jr.Value);
                    }
                default:
                    throw new DataFormatException(jr, "Invalid season.");
            }
        }
        private static DateVariable? ParseRawDate(IO.JsonReader jr)
        {
            // init
            DateVariable? result = null;

            // string expected
            jr.Validate(IO.JsonNodeType.String);
            var text = (string)jr.Value;

            // try parse
#warning Expand and improve raw date parsing
            DateTime date;
            int year;
            if (DateTime.TryParse(text, out date))
            {
                // success
                result = new DateVariable(date);
            }
            else if (int.TryParse(text, out year))
            {
                result = new DateVariable(year, null, null, null, false);
            }
            else
            {
                // failure
                throw new DataFormatException(jr, "Invalid raw date value '{0}'.", jr.Value);
            }

            // skip value
            jr.Read();

            // done
            return result;
        }
    }
}