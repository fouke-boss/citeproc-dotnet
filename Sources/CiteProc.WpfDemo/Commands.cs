using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CiteProc.WpfDemo
{
    public static class Commands
    {
        public static readonly RoutedUICommand About = new RoutedUICommand
        (
            "About...",
            "About",
            typeof(Commands),
            null
        );
    }
}
