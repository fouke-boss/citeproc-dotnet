using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class Item : IDisposable, IDataProvider
        {
            private Dictionary<string, IVariable> _Variables;

            public Item(Processor owner, IDataProvider dataProvider)
            {
                // init
                this.Owner = owner;
                this.DataProvider = dataProvider;

                // add event handler
                if (this.DataProvider is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)this.DataProvider).PropertyChanged += DataProvider_PropertyChanged;
                }

                // update state
                this.UpdateState();
            }
            public void Dispose()
            {
                // remove event handler
                if (this.DataProvider is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)this.DataProvider).PropertyChanged -= DataProvider_PropertyChanged;
                }
            }

            public Processor Owner
            {
                get;
                private set;
            }
            public IDataProvider DataProvider
            {
                get;
                private set;
            }

            public Culture Culture
            {
                get
                {
                    return this.DataProvider.Culture;
                }
            }
            public Dictionary<string, IVariable> GetVariables()
            {
                // init
                var results = this._Variables
                    .ToDictionary(x => x.Key.ToLower(), x => x.Value);

                // special variables
                results.Add("citation-number", new Data.NumberVariable((uint)this.CitationNumber));

                // done
                return results;                    
            }

            private void DataProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                // update state
                this.UpdateState();

                // update owner
                this.Owner.UpdateState();
            }
            private void UpdateState()
            {
                // update variables
                this._Variables = this.DataProvider.GetVariables()
                    .Where(x => x.Key != "citation-number")
                    .ToDictionary(x => x.Key, x => x.Value);

                // get bib sort key
                this.BibliographySortKey = this.Owner.GenerateBibliographySort(new ExecutionContext(this, this.Owner.InvariantLocale, DisambiguationContext.Default), Parameters.Default);

                // default citation
                this.DefaultCite = this.Owner.GenerateCitationEntry(new ExecutionContext(this, this.Owner.InvariantLocale, DisambiguationContext.Default), Parameters.Default)
                    .Layout
                    .ToPlainText();
            }

            public string[] BibliographySortKey
            {
                get;
                private set;
            }
            public string[] CitationSortKey
            {
                get;
                private set;
            }
            public string DefaultCite
            {
                get;
                private set;
            }

            public int CitationNumber
            {
                get;
                internal set;
            }
            public DisambiguationContext DisambiguationContext
            {
                get;
                internal set;
            }
        }
    }
}
