using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;

namespace DnTool.ViewModels
{
    public class SettingsViewModel : FlyoutBaseViewModel
    {
        public SettingsViewModel()
        {
            this.Header = "设置";
            this.Position = Position.Right;
            this.CurrentOption = "1";
        }
        //public static void SetTheme(string name)
        //{
        //    Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
        //    Accent accent = ThemeManager.Accents.FirstOrDefault(m=>m.Name==name);
        //    if (accent == null)
        //        return;
        //    ThemeManager.ChangeAppStyle(Application.Current,accent,theme.Item1);
        //}

       
        
        private string _currentOption;

        public string CurrentOption
        {
            get { return _currentOption; }
            set
            {
                if(value!=null)
                  base.SetProperty(ref _currentOption, value, () => this.CurrentOption);
            }
        }
        
    }
}
