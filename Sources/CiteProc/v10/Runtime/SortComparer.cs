using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class SortComparer : IComparer<string[]>
        {
            public SortComparer(params SortOrder[] orders)
            {
                // init
                this.Orders = orders;
            }

            public SortOrder[] Orders
            {
                get;
                private set;
            }

            public int Compare(string[] a, string[] b)
            {
                var result = this.Compare(a, b, 0);

                return result;
            }
            private int Compare(string[] a, string[] b, int level)
            {
                if (level >= this.Orders.Length)
                {
                    return 0;
                }
                else
                {
                    // init
                    var x = a[level];
                    var y = b[level];

                    // done
                    if (x == null && y == null)
                    {
                        return this.Compare(a, b, level + 1);
                    }
                    else if (x == null && y != null)
                    {
                        return 1;
                    }
                    else if (x != null && y == null)
                    {
                        return -1;
                    }
                    else
                    {
                        // compare string
                        var result = string.Compare(x, y);

                        // equal?
                        if (result == 0)
                        {
                            return this.Compare(a, b, level + 1);
                        }
                        else if (this.Orders[level] == SortOrder.Ascending)
                        {
                            // ascending
                            return result;
                        }
                        else
                        {
                            // descending
                            return -1 * result;
                        }
                    }
                }
            }
        }
    }
}
