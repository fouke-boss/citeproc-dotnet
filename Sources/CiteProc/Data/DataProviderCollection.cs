using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    public class DataProviderCollection : IList<DataProvider>, INotifyCollectionChanged
    {
        private List<DataProvider> _Items = new List<DataProvider>();

        public DataProviderCollection()
        {
        }
        public DataProviderCollection(IEnumerable<DataProvider> items)
        {
        }

        public DataProvider this[int index]
        {
            get
            {
                return this._Items[index];
            }
            set
            {
                // set
                this._Items[index] = value;

                // changed
                this.OnCollectionChanged();
            }
        }
        public IEnumerator<DataProvider> GetEnumerator()
        {
            return this._Items.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(DataProvider item)
        {
            this.AddRange(new DataProvider[] { item });
        }
        public void AddRange(IEnumerable<DataProvider> items)
        {
            // add
            this._Items.AddRange(items);

            // done
            this.OnCollectionChanged();
        }

        public void Insert(int index, DataProvider item)
        {
            this.InsertRange(index, new DataProvider[] { item });
        }
        public void InsertRange(int index, IEnumerable<DataProvider> items)
        {
            // add
            this._Items.AddRange(items);

            // event
            this.OnCollectionChanged();
        }

        public bool Remove(DataProvider item)
        {
            // remove
            var result = this._Items.Remove(item);

            // changed
            if (result)
            {
                this.OnCollectionChanged();
            }

            // done
            return result;
        }
        public void RemoveAt(int index)
        {
            // remove
            this._Items.RemoveAt(index);

            // changed
            this.OnCollectionChanged();
        }
        public void RemoveRange(int index, int count)
        {
            // remove
            this._Items.RemoveRange(index, count);

            // changed
            this.OnCollectionChanged();
        }

        public void Clear()
        {
            // clear
            this._Items.Clear();

            // event
            this.OnCollectionChanged();
        }
        public int Count
        {
            get
            {
                return this._Items.Count;
            }
        }

        public int IndexOf(DataProvider item)
        {
            return this._Items.IndexOf(item);
        }
        public bool Contains(DataProvider item)
        {
            return this._Items.Contains(item);
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public void CopyTo(DataProvider[] array)
        {
            this._Items.CopyTo(array);
        }
        public void CopyTo(DataProvider[] array, int arrayIndex)
        {
            this._Items.CopyTo(array, arrayIndex);
        }

        public DataProvider[] Load(string path, DataFormat format)
        {
            using (var tr = new StreamReader(path))
            {
                return this.Load(tr, format);
            }
        }
        public DataProvider[] Load(Stream stream, DataFormat format)
        {
            using (var tr = new StreamReader(stream))
            {
                return this.Load(tr, format);
            }
        }
        public DataProvider[] Load(TextReader reader, DataFormat format)
        {
            // parse
            var results = format.Parse(reader).ToArray();

            // add
            this.AddRange(results);

            // done
            return results;
        }

        public void Save(StringBuilder builder, DataFormat format)
        {
            using (var sw = new StringWriter(builder))
            {
                this.Save(sw, format);
            }
        }
        public void Save(string path, DataFormat format)
        {
            using (var sw = new StreamWriter(path))
            {
                this.Save(sw, format);
            }
        }
        public void Save(Stream stream, DataFormat format)
        {
            using (var sw = new StreamWriter(stream))
            {
                this.Save(sw, format);
            }
        }
        public void Save(TextWriter writer, DataFormat format)
        {
            throw new NotImplementedException();
        }

        public DataProvider[] Parse(string value, DataFormat format)
        {
            using (var tr = new StringReader(value))
            {
                return this.Load(tr, format);
            }
        }
        public string Format(DataFormat format)
        {
            using (var sw = new StringWriter())
            {
                // save
                this.Save(sw, format);

                // done
                return sw.GetStringBuilder().ToString();
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}