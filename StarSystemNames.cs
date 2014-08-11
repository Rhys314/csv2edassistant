using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csv2edassistant
{
    public partial class starSystemNames_Input : Form
    {
        public starSystemNames_Input()
        {
            InitializeComponent();
        }

        public starSystemNames_Input(string value)
        {
            InitializeComponent();
            stationName.Text = value;
        }

        private void starSystemName_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
