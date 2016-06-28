using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test
{
    public class Item : IDataProvider
    {
        public Culture Culture
        {
            get
            {
                return new Culture("en-US");
            }
        }
        public object GetVariable(string name)
        {
            if (name == "type")
            {
                return "book";
            }
            else if (name == "issued")
            {
                return new DateVariable(new DateTime(2005, 11, 22), new DateTime(2005, 12, 25));
            }
            else if (name == "accessed")
            {
                return "1 jan 2015";
            }
            else if (name == "birthday")
            {
                return new DateVariable(2016, Season.Spring, null, null, true);
            }
            else if (name == "accessed2")
            {
                return null;
            }
            else if (name == "page")
            {
                return new NumberVariable(1, 3, '&');
            }
            else if (name == "author")
            {
                return new object[] {
                    new NameVariable("Boss", "Fouke Frans Janne"),
                    new NameVariable("Boss-Reus", "Carolien"),
                    new NameVariable("Boss", "Pjotr Johan"),
                    new NameVariable("Boss", "Tonke Amélie"),
                    new NameVariable("Reus", "Hendrik Jan Willem"),
                };
            }
            else
            {
                return string.Format("test_{0}", name);
            }
        }
    }
}
