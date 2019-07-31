using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AchtungDieCurve
{
    public partial class WindowOfMainMenu : Form
    {
        Form f;
        public WindowOfMainMenu()
        {
            
            InitializeComponent();
            f = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowOfPlayerSettings wops = new WindowOfPlayerSettings(this);
            wops.Show();
            this.Hide();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Close();
        }
    }
}
