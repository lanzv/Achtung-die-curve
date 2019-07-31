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
    public partial class Form1 : Form
    {
        public RadioButton[] r = new RadioButton[3];
        public TextBox[] tb = new TextBox[3];
        public Label label = new Label();
        public TextBox text = new TextBox();
        public Form1(String s)
        {
            InitializeComponent();
            r[0] = this.radioButton1;
            r[1] = this.radioButton2;
            r[2] = this.radioButton3;

            tb[0] = this.textBox1;
            tb[1] = this.textBox2;
            tb[2] = this.textBox3;

            label = this.label1;
            label.Text = s;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
