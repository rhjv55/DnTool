using DnTool.Utilities;
using DnTool.Utilities.Hook;
using DnTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DnTool.Views
{
    /// <summary>
    /// TeleportView.xaml 的交互逻辑
    /// </summary>
    public partial class TeleportView : UserControl
    {
        public TeleportView()
        {
            InitializeComponent();
            UserActivityHook choosesc = new UserActivityHook();
            choosesc.KeyDown += new System.Windows.Forms.KeyEventHandler(MyKeyDown);
            try
            {
                this.cbUseHotKey.IsChecked = bool.Parse(INIHelper.IniReadValue("BaseConfig", "UseHotKey", AppDomain.CurrentDomain.BaseDirectory + "\\config.ini"));
            }
            catch
            {
            }
        }

        private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            if (Locator.Settings.CurrentOption.Equals("0"))
            {
                if (e.Row.GetIndex() >= 12)
                {
                    e.Row.Header = "";
                    return;
                }
                int row = e.Row.GetIndex();
                if (row == 0) e.Row.Header = "F1";
                if (row == 1) e.Row.Header = "F2";
                if (row == 2) e.Row.Header = "F3";
                if (row == 3) e.Row.Header = "F4";
                if (row == 4) e.Row.Header = "F5";
                if (row == 5) e.Row.Header = "F6";
                if (row == 6) e.Row.Header = "F7";
                if (row == 7) e.Row.Header = "F8";
                if (row == 8) e.Row.Header = "F9";
                if (row == 9) e.Row.Header = "F10";
                if (row == 10) e.Row.Header = "F11";
                if (row == 11) e.Row.Header = "F12";

            }
            else
            {
                if (e.Row.GetIndex() > 9)
                {
                    e.Row.Header = "";
                    return;
                }
                e.Row.Header = e.Row.GetIndex();
            }
        }
        ViewModelLocator Locator=new ViewModelLocator();
        public void MyKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var viewmodel = this.DataContext as TeleportViewModel;
            int i = -1;
            if (Locator.Settings.CurrentOption.Equals("0"))
            {
                if ((int)e.KeyCode >= 112 || (int)e.KeyCode <= 123)
                    i = (int)e.KeyCode - 112;
            }
            else
            {
                if ((int)e.KeyCode >= 96 || (int)e.KeyCode <= 105)
                    i = (int)e.KeyCode - 96;
            }
            if (i == -1)
                return;

            if (this.dg.Items.Count <= i)
            {
                this.dg.SelectedIndex = -1;
                return;
            }
           
            if (this.cbUseHotKey.IsChecked == true&&SoftContext.Role!=null)
            {
                viewmodel.Teleport((Models.Point)this.dg.Items[i]);
                this.dg.SelectedIndex = i;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            INIHelper.IniWriteValue("BaseConfig", "Topmost", "true", AppDomain.CurrentDomain.BaseDirectory + "\\config.ini");
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            INIHelper.IniWriteValue("BaseConfig", "Topmost", "false", AppDomain.CurrentDomain.BaseDirectory + "\\config.ini");
        }

        private void cbUseHotKey_Checked(object sender, RoutedEventArgs e)
        {
            INIHelper.IniWriteValue("BaseConfig", "UseHotKey", "true", AppDomain.CurrentDomain.BaseDirectory + "\\config.ini");
        }

        private void cbUseHotKey_Unchecked(object sender, RoutedEventArgs e)
        {
            INIHelper.IniWriteValue("BaseConfig", "UseHotKey", "false", AppDomain.CurrentDomain.BaseDirectory + "\\config.ini");
        }


        
    }


}
