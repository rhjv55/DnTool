using DnTool.Utilities;
using DnTool.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DnTool.Views
{
    /// <summary>
    /// LoginFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class LoginFlyout : Flyout
    {
        public LoginFlyout()
        {
            InitializeComponent();
          
        }

      

        private ICommand closeCmd;

        public ICommand CloseCmd
        {
            get
            {
                return this.closeCmd ?? (this.closeCmd = new SimpleCommand
                {
                    CanExecuteDelegate = x => false,
                    ExecuteDelegate = x => { }
                });
            }
        }

     
    }
}
