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
            p.BindWindow(592236, "", "sendmsg", "sendmsg", 0);

          //  p.KeyPress(VirtualKeyCode.LWin);
           // p.MoveTo(2000,2000);
           // p.Delay(500);
           // GetCurrentPos();
          //  p.Delay(500);
          // // p.RightClick();
          ////  p.Delay(1000);
          //  p.MoveTo(500, 500);
          //  GetCurrentPos();
            p.LeftClick();
            //p.KeyPress(VirtualKeyCode.CapsLock);
            //p.KeyPress(VirtualKeyCode.E);
            //p.KeyPress(VirtualKeyCode.A);
            //p.KeyPress(VirtualKeyCode.A);
            p.RightClick();

           


            //p.MoveTo(10, 10);
            //GetCurrentPos();
            //p.MoveTo(120, 300);
            //GetCurrentPos();
            //p.MoveTo(590, 500);
            //GetCurrentPos();
            //p.MoveTo(1700, 300);
            //GetCurrentPos();
            //p.MoveTo(2000, 2000);
            //GetCurrentPos();
            //p.LeftClick();
            //p.WheelDown();
            //p.WheelDown();
            //p.WheelDown();
            //p.WheelDown();

            //p.WheelDown();
            //p.WheelDown();
            //p.WheelDown();
            //p.WheelDown();
            //p.WheelDown();
            //p.WheelUp();
            //p.WheelUp();
            //p.WheelUp();
            //p.WheelUp();
            //p.WheelUp();
            //p.WheelUp();

            //p.WheelUp();
            //p.WheelUp();

            //p.WheelUp();
            //p.Delay(50);
            //p.RightClick();
           
        }
        public void GetCurrentPos()
        {
            int x, y;
            new IPlugin.Main.HPlugin().GetCursorPos(out x,out y);
            Debug.WriteLine("当前坐标："+x + " " + y);
        }
    }
}
