using IPlugin.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HPlugin.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPlugin.Main.HPlugin p = new IPlugin.Main.HPlugin();
            p.BindWindow(0,"","input","input",0);
            p.KeyPress(VirtualKeyCode.LWin);
            p.LeftClick();
        }
    }
}
