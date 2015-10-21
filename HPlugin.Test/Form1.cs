using IPlugin.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            p.BindWindow(591876, "", "postmsg", "postmsg", 0);

            Show(() => p.GetOsVersion());
            
        }
        public void GetCurrentPos()
        {
            int x, y;
            new IPlugin.Main.HPlugin().GetCursorPos(out x,out y);
            Debug.WriteLine("当前坐标："+x + " " + y);
        }
        public void Show(Func<object> fun)
        {
            object ret = fun();
            MessageBox.Show(ret.ToString());
        }
    }
}
