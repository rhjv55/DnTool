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
            this.DataContext = new TeleportViewModel();
            UserActivityHook choosesc = new UserActivityHook();
            choosesc.KeyDown += new System.Windows.Forms.KeyEventHandler(MyKeyDown);
        }

        private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            if (e.Row.GetIndex() > 9)
            {
                e.Row.Header = "";
                return;
            }
            e.Row.Header = e.Row.GetIndex();

            // Debug.WriteLine(e.Row.GetIndex()+1);
        }

        public void MyKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var viewmodel = this.DataContext as MainViewModel;
            int i = -1;
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad0)
            {
                i = 0;
                Debug.WriteLine("执行热键" + 0);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad1)
            {
                i = 1;
                Debug.WriteLine("执行热键" + 1);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad2)
            {
                i = 2;
                Debug.WriteLine("执行热键" + 2);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad3)
            {
                i = 3;
                Debug.WriteLine("执行热键" + 3);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad4)
            {
                i = 4;
                Debug.WriteLine("执行热键" + 4);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad5)
            {
                i = 5;
                Debug.WriteLine("执行热键" + 5);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad6)
            {
                i = 6;
                Debug.WriteLine("执行热键" + 6);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad7)
            {
                i = 7;
                Debug.WriteLine("执行热键" + 7);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad8)
            {
                i = 8;
                Debug.WriteLine("执行热键" + 8);
            }
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad9)
            {
                i = 9;
                Debug.WriteLine("执行热键" + 9);
            }

            if (this.dg.Items.Count <= i)
            {
                this.dg.SelectedIndex = -1;
                return;
            }
            if (i == -1)
                return;
            //viewmodel.Move(viewmodel.CurrentHwnd, ((InfoViewModel)this.dg.Items[i]).CurrentPoint);
            //this.dg.SelectedIndex = i;
        }

        private void ShowModifyView(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


}
