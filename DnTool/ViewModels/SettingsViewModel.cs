using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using DnTool.Utilities;
using System.Windows.Media;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace DnTool.ViewModels
{
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }
    }

    public class SettingsViewModel : FlyoutBaseViewModel
    {
        public RelayCommand<AccentColorMenuData> ChangeAccentCommand { get; set; }

        public SettingsViewModel()
        {
            this.Header = "设置";
            this.Position = Position.Right;
            string hotkey = INIHelper.IniReadValue("BaseConfig", "HotKey", AppDomain.CurrentDomain.BaseDirectory + "\\config.ini");
            if (string.IsNullOrEmpty(hotkey))
                this.CurrentOption = "1";
            else
                this.CurrentOption = hotkey;


            this.AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                                            .ToList();
            this.AccentColors.Add(new AccentColorMenuData() { Name = "random" });
            this.ChangeAccentCommand = new RelayCommand<AccentColorMenuData>((item) =>
            {
                if (item == null)
                    return;
                this.SetTheme(item.Name);
                INIHelper.IniWriteValue("BaseConfig", "Accent", item.Name, AppDomain.CurrentDomain.BaseDirectory + "\\config.ini");
            });

            string accentName = INIHelper.IniReadValue("BaseConfig", "Accent", AppDomain.CurrentDomain.BaseDirectory + "\\config.ini");
            this.SetTheme(accentName);
            this.SelectedItem = this.AccentColors.FirstOrDefault(x=>x.Name==accentName);
        }
        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="name">主题名字</param>
        private void SetTheme(string name)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            Accent accent;
            if (name == "random")
            {
                int num = new Random().Next(ThemeManager.Accents.Count());
                accent = ThemeManager.Accents.ElementAt(num);
            }
            else
            {
                accent = ThemeManager.Accents.FirstOrDefault(m => m.Name == name);
            }
            if (accent == null)
                return;
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
        private AccentColorMenuData _selectedItem;

        public AccentColorMenuData SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                base.SetProperty(ref _selectedItem, value, () => this.SelectedItem);
            }
        }
        
 
        public List<AccentColorMenuData> AccentColors { get; set; }
        
        private string _currentOption;

        public string CurrentOption
        {
            get { return _currentOption; }
            set
            {
                if (value != null)
                {
                    base.SetProperty(ref _currentOption, value, () => this.CurrentOption);
                    INIHelper.IniWriteValue("BaseConfig","HotKey",value,AppDomain.CurrentDomain.BaseDirectory+"\\config.ini");
                }
            }
        }
        
    }
}
