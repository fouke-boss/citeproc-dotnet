using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CiteProc
{
    /// <summary>
    /// Base class for all style and locale files.
    /// </summary>
    public abstract class File : Element, ICloneable
    {
        [ThreadStatic]
        private static XmlReader _Current;
        internal static IXmlLineInfo Current
        {
            get
            {
                return _Current as IXmlLineInfo;
            }
        }

        /// <summary>
        /// Public Citation Style Language namespace.
        /// </summary>
        public const string NAMESPACE_URI = "http://purl.org/net/xbiblio/csl";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="specification"></param>
        internal File(Version specification)
        {
            // init
            this.Specification = specification;
        }

        /// <summary>
        /// Indicates the specification version of the CSL file.
        /// </summary>
        [XmlIgnore]
        public Version Specification
        {
            get;
            private set;
        }
        /// <summary>
        /// The version of the CSL file.
        /// </summary>
        [XmlAttribute("version")]
        public string Version
        {
            get
            {
                return this.Specification.ToString(2);
            }
            set
            {
                // ignore
            }
        }

        /// <summary>
        /// Parses the given xml into a file instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static File Parse(string xml)
        {
            using (var sr = new StringReader(xml))
            {
                return Load(sr);
            }
        }
        /// <summary>
        /// Parses the given xml into a file instance of the given type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Parse<T>(string xml)
            where T : File
        {
            return (T)Parse(xml);
        }

        /// <summary>
        /// Loads a file from the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static File Load(string path)
        {
            using (var fs = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Load(fs);
            }
        }
        /// <summary>
        /// Loads a file from the given stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static File Load(Stream stream)
        {
            using (var xr = XmlReader.Create(stream))
            {
                return Load(xr);
            }
        }
        /// <summary>
        /// Loads a file from the given text reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static File Load(TextReader reader)
        {
            // done
            using (var xr = XmlReader.Create(reader))
            {
                return Load(xr);
            }
        }
        /// <summary>
        /// Loads a file from the given xml reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static File Load(XmlReader reader)
        {
            // init
            File result = null;

            // locked?
            if (_Current != null)
            {
                throw new NotSupportedException();
            }

            // lock
            _Current = reader;

            // try/catch
            try
            {
                // read to first element
                while (reader.NodeType != XmlNodeType.Element && !reader.EOF)
                {
                    reader.Read();
                }

                // element present?
                if (reader.NodeType == XmlNodeType.Element)
                {
                    // namespace valid?
                    if (reader.NamespaceURI != NAMESPACE_URI)
                    {
                        throw new XmlException(string.Format("Unexpected namespace '{0}' encountered.", reader.NamespaceURI));
                    }

                    // version
                    var version = reader.GetAttribute("version");
                    System.Type type = null;
                    switch (version)
                    {
                        case "1.0":
                            // type
                            switch (reader.LocalName)
                            {
                                case "style":
                                    type = typeof(v10.StyleFile);
                                    break;
                                case "locale":
                                    type = typeof(v10.LocaleFile);
                                    break;
                            }
                            break;
                        default:
                            throw new XmlException(string.Format("Version '{0}' is not supported.", version));
                    }

                    // type found?
                    if (type == null)
                    {
                        throw new XmlException(string.Format("Unexpected element '{0}' of version '{1}' encountered.", reader.LocalName, version));
                    }

                    // deserialize
                    var xs = new XmlSerializer(type);
                    xs.UnknownNode += XmlSerializer_UnknownNode;
                    xs.UnknownAttribute += xs_UnknownAttribute;
                    result = (File)xs.Deserialize(reader);
                }
            }
            finally
            {
                // unlock
                _Current = null;
            }

            // done
            return result;
        }

        static void xs_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            // unknown attribute encountered
            throw new XmlException(string.Format("Unexpected attribute '{0}' encountered.", e.Attr.LocalName), null, e.LineNumber, e.LinePosition - 2 - e.Attr.Name.Length);

        }
        static void XmlSerializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            if (e.NodeType != XmlNodeType.Attribute)
            {
                // init
                var value = (string.IsNullOrEmpty(e.Text) ? e.LocalName : e.Text)
                    .Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();

                // throw
                throw new XmlException(string.Format("Unexpected {0} '{1}' encountered.", e.NodeType.ToString().ToLower(), value), null, e.LineNumber, e.LinePosition);
            }
        }

        /// <summary>
        /// Loads a file from the given path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path)
            where T : File
        {
            return (T)Load(path);
        }
        /// <summary>
        /// Loads a file from the given text reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T Load<T>(Stream stream)
            where T : File
        {
            return (T)Load(stream);
        }
        /// <summary>
        /// Loads a file from the given text reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Load<T>(TextReader reader)
            where T : File
        {
            return (T)Load(reader);
        }
        /// <summary>
        /// Loads a file from the given xml reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Load<T>(XmlReader reader)
            where T : File
        {
            return (T)Load(reader);
        }

        /// <summary>
        /// Saves the content of this file to the given path.
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                this.Save(fs);
            }
        }
        /// <summary>
        /// Saves the content of this file to the given stream.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            using (var xw = XmlWriter.Create(stream))
            {
                this.Save(xw);
            }
        }
        /// <summary>
        /// Saves the content of this file to the given text writer.
        /// </summary>
        /// <param name="writer"></param>
        public void Save(TextWriter writer)
        {
            using (var xw = XmlWriter.Create(writer))
            {
                this.Save(xw);
            }
        }
        /// <summary>
        /// Saves the content of this file to the xml writer.
        /// </summary>
        /// <param name="writer"></param>
        public void Save(XmlWriter writer)
        {
            // serialize
            var xs = new XmlSerializer(this.GetType());
            xs.Serialize(writer, this);
        }

        /// <summary>
        /// Returns a deep clone of this file.
        /// </summary>
        /// <returns></returns>
        public File Clone()
        {
            // memory stream
            using (var ms = new MemoryStream(2 * 65536))
            {
                // save
                this.Save(ms);

                // reset
                ms.Position = 0;

                // load
                return Load(ms);
            }
        }
        /// <summary>
        /// Returns a deep clone of this file.
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns the xml of this file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // init
            var result = new StringBuilder(2 * 65536);

            // save
            using (var sw = new StringWriter(result))
            {
                this.Save(sw);
            }

            // done
            return result.ToString();
        }
    }
}