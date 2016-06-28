using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CiteProc.WpfDemo
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_Navigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            // init
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // close
            this.Close();
        }
    }
}
