using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CiteProc.CodeGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLocalesV10_Click(object sender, EventArgs e)
        {
#if(DEBUG)
            // render code
            var code = CiteProc.v10.Runtime.Processor.GenerateDefaultLocaleProviders();

            // clipboard
            Clipboard.SetText(code);

            // done
            MessageBox.Show("The generated code is placed on the clipboard.");
#else
            throw new NotSupportedException("Not supported in RELEASE mode.");
#endif
        }
    }
}
