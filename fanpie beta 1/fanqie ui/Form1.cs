using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using code2text;

namespace fanqie_ui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            code2text.code2text coder = new code2text.code2text(textBox1.Text);
            textBox2.Text = coder.Convert2TEXT(Int32.Parse(textBox3.Text));
        }
    }
}
