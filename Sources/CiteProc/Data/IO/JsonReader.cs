using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data.IO
{
    internal class JsonReader : IDisposable, ILineInfo
    {
        private CharReader _Reader;
        private Stack<Mode> _Stack = new Stack<Mode>();

        public JsonReader(TextReader reader)
        {
            // init
            this._Reader = new CharReader(reader);
            this.NodeType = JsonNodeType.None;

            // read
            this.Read();
        }
        public void Dispose()
        {
            // dispose
            this._Reader.Dispose();
            this._Reader = null;
        }

        public bool Eof
        {
            get
            {
                return this._Reader.Eof;
            }
        }
        public bool Read()
        {
            // skip whitespace
            this.SkipWhitespace();

            // line number and position
            this.CopyLineInfo();

            // state
            if (this.Eof)
            {
                // end of file
                this.Update(JsonNodeType.None, null);
            }
            else if (this._Stack.Count == 0)
            {
                // start of file
                this.ReadStartOfFile();
            }
            else
            {
                // array, object or property
                switch (this._Stack.Peek())
                {
                    case Mode.Array:
                        this.ReadArray();
                        break;
                    case Mode.Object:
                        this.ReadObject();
                        break;
                    case Mode.PropertyValue:
                        // read value
                        this.ReadPropertyValue();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            // done
            return (this.NodeType != JsonNodeType.None);
        }
        public bool Read(JsonNodeType nodeType)
        {
            // validate node type
            this.Validate(NodeType);

            // read
            return this.Read();
        }

        public bool? ReadAsBoolean()
        {
            // init
            var result = (bool?)null;

            // per type
            switch (this.NodeType)
            {
                case JsonNodeType.Boolean:
                    result = (bool)this.Value;
                    break;
                case JsonNodeType.Integer:
                    switch ((int)this.Value)
                    {
                        case 0:
                            result = false;
                            break;
                        case 1:
                            result = true;
                            break;
                        default:
                            throw new DataFormatException(this, "Invalid boolean value '{0}'.", this.Value);
                    }
                    break;
                case JsonNodeType.String:
                    switch (((string)this.Value).ToLower())
                    {
                        case "false":
                            result = false;
                            break;
                        case "true":
                            result = true;
                            break;
                        case "":
                            result = null;
                            break;
                        default:
                            throw new DataFormatException(this, "Invalid boolean value '{0}'.", this.Value);
                    }
                    break;
                default:
                    this.Validate(JsonNodeType.Boolean);
                    break;
            }

            // skip
            this.Read();

            // done
            return result;
        }
        public string ReadAsString()
        {
            // init
            string result = null;

            // valid type?
            switch (this.NodeType)
            {
                case JsonNodeType.String:
                    result = (string)this.Value;
                    break;
                case JsonNodeType.Integer:
                    result = ((int)this.Value).ToString();
                    break;
                default:
                    this.Validate(JsonNodeType.String);
                    break;
            }

            // skip
            this.Read();

            // done
            return result;
        }
        public int ReadAsInteger()
        {
            // init
            int result = 0;

            // valid type?
            switch (this.NodeType)
            {
                case JsonNodeType.Integer:
                    result = (int)this.Value;
                    break;
                case JsonNodeType.String:
                    int integer;
                    if (int.TryParse((string)this.Value, out integer))
                    {
                        result = integer;
                    }
                    else
                    {
                        throw new DataFormatException(this, "'{0}' is not a valid integer value.", this.Value);
                    }
                    break;
                default:
                    this.Validate(JsonNodeType.String);
                    break;
            }

            // skip
            this.Read();

            // done
            return result;
        }

        public void Validate(JsonNodeType nodeType)
        {
            // node type correct?
            if (nodeType != this.NodeType)
            {
                // throw exception
                switch (nodeType)
                {
                    case JsonNodeType.Boolean:
                        throw new DataFormatException(this, "Boolean value expected.");
                    case JsonNodeType.EndOfArray:
                        throw new DataFormatException(this, "End of array expected.");
                    case JsonNodeType.EndOfObject:
                        throw new DataFormatException(this, "End of object expected.");
                    case JsonNodeType.Integer:
                        throw new DataFormatException(this, "Integer value expected.");
                    case JsonNodeType.None:
                        throw new DataFormatException(this, "None expected.");
                    case JsonNodeType.Null:
                        throw new DataFormatException(this, "Null value expected.");
                    case JsonNodeType.PropertyName:
                        throw new DataFormatException(this, "Property name expected.");
                    case JsonNodeType.StartOfArray:
                        throw new DataFormatException(this, "Start of array expected.");
                    case JsonNodeType.StartOfObject:
                        throw new DataFormatException(this, "Start of object expected.");
                    case JsonNodeType.String:
                        throw new DataFormatException(this, "String value expected.");
                    default:
                        throw new NotSupportedException();
                }
            }
        }
        private void ReadStartOfFile()
        {
            // array, object or property?
            if (this._Reader.Current == '[')
            {
                // array
                this._Stack.Push(Mode.Array);

                // update
                this.Update(JsonNodeType.StartOfArray, null);

                // skip
                this._Reader.Pop();
            }
            else if (this._Reader.Current == '{')
            {
                // object
                this._Stack.Push(Mode.Object);

                // update
                this.Update(JsonNodeType.StartOfArray, null);

                // skip
                this._Reader.Pop();
            }
            else
            {
                throw new DataFormatException(this._Reader, "Unexpected character '{0}'.", this._Reader.Current);
            }
        }
        private void ReadArray()
        {
            // expect ']' or value
            if (this._Reader.Current == ']')
            {
                // end array
                this._Stack.Pop();

                // update
                this.Update(JsonNodeType.EndOfArray, null);

                // pop
                this._Reader.Pop();
            }
            else
            {
                // comma?
                this.SkipComma();

                // value
                this.ReadValue();
            }
        }
        private void ReadObject()
        {
            // '"' or '}'
            if (this._Reader.Current == '}')
            {
                // end object
                this._Stack.Pop();

                // update
                this.Update(JsonNodeType.EndOfObject, null);

                // done
                this._Reader.Pop();
            }
            else
            {
                // comma?
                this.SkipComma();

                // property?
                if (this._Reader.Current == '"')
                {
                    // skip '"'
                    this._Reader.Pop();

                    // read name
                    var name = this.ReadText();

                    // read :
                    this.SkipWhitespace();
                    if (this._Reader.Current != ':')
                    {
                        // error
                        throw new DataFormatException(this._Reader, "Unexpected character '{0}'. ':' expected.", this._Reader.Current);
                    }
                    this._Reader.Pop();

                    // mode
                    this._Stack.Push(Mode.PropertyValue);

                    // property name
                    this.Update(JsonNodeType.PropertyName, name);
                }
                else
                {
                    // error
                    throw new DataFormatException(this._Reader, "Unexpected character '{0}'. '\"' expected.", this._Reader.Current);
                }
            }
        }
        private void ReadPropertyValue()
        {
            // pop
            this._Stack.Pop();

            // read value
            this.ReadValue();
        }
        private void ReadValue()
        {
            // '"', '[', '{' or literal
            if (this._Reader.Current == '"')
            {
                // skip "
                this._Reader.Pop();

                // text
                var text = this.ReadText();

                // update
                this.Update(JsonNodeType.String, text);
            }
            else if (this._Reader.Current == '[')
            {
                // start array
                this._Stack.Push(Mode.Array);

                // update
                this.Update(JsonNodeType.StartOfArray, null);

                // pop
                this._Reader.Pop();
            }
            else if (this._Reader.Current == '{')
            {
                // start array
                this._Stack.Push(Mode.Object);

                // update
                this.Update(JsonNodeType.StartOfObject, null);

                // pop
                this._Reader.Pop();
            }
            else
            {
                // value
                var value = this._Reader.Pop(x => char.IsLetterOrDigit(x) || "-".Contains(x));

                // int?
                int integer;
                if (value == "null")
                {
                    this.Update(JsonNodeType.Null, null);
                }
                else if (value == "true")
                {
                    this.Update(JsonNodeType.Boolean, true);
                }
                else if (value == "false")
                {
                    this.Update(JsonNodeType.Boolean, false);
                }
                else if (int.TryParse(value, out integer))
                {
                    this.Update(JsonNodeType.Integer, integer);
                }
                else
                {
                    throw new DataFormatException(this._Reader, "Unexpected value '{0}'. '\"' expected.", value);
                }
            }
        }
        private string ReadText()
        {
            // init
            var text = new StringBuilder(1024);
            var escaped = false;

            // read
            while (!this._Reader.Eof)
            {
                // init
                var c = this._Reader.Pop();

                // process
                if (escaped)
                {
                    // which one?
                    switch (c)
                    {
                        case '"':
                        case '\\':
                        case '/':
                            text.Append(c);
                            break;
                        case 'b':
                            text.Append('\b');
                            break;
                        case 'n':
                            text.Append('\n');
                            break;
                        case 'r':
                            text.Append('\r');
                            break;
                        case 't':
                            text.Append('\t');
                            break;
                        case 'u':
                            var hex = new string(new char[] { this._Reader.Pop(), this._Reader.Pop(), this._Reader.Pop(), this._Reader.Pop() });
                            throw new NotImplementedException();
                        default:
                            throw new NotSupportedException();
                    }

                    // done
                    escaped = false;
                }
                else if (c == '\\')
                {
                    // escape
                    escaped = true;
                }
                else if (c == '"')
                {
                    break;
                }
                else
                {
                    text.Append(c);
                }
            }

            // done
            return text.ToString();
        }

        private void Update(JsonNodeType nodeType, object value)
        {
            // update
            this.NodeType = nodeType;
            this.Value = value;
        }
        public JsonNodeType NodeType
        {
            get;
            private set;
        }
        public object Value
        {
            get;
            private set;
        }
        public int LineNumber
        {
            get;
            private set;
        }
        public int LinePosition
        {
            get;
            private set;
        }

        private void SkipWhitespace()
        {
            // done
            this._Reader.Pop(c => " \n\r\t".Contains(c));
        }
        private void SkipComma()
        {
            // comma?
            if (this._Reader.Current == ',')
            {
                // skip
                this._Reader.Pop();
                this.SkipWhitespace();

                // copy line
                this.CopyLineInfo();
            }
        }
        private void CopyLineInfo()
        {
            this.LineNumber = this._Reader.LineNumber;
            this.LinePosition = this._Reader.LinePosition;
        }

        private enum Mode
        {
            StartOfFile,
            Array,
            Object,
            PropertyValue,
        }

        public override string ToString()
        {
            return string.Format("{0}{1} ({2}, {3})", this.NodeType, (this.Value == null ? null : string.Format(": '{0}'", this.Value)), this.LineNumber, this.LinePosition);
        }
    }
}