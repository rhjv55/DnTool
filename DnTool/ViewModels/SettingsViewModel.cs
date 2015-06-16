using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DnTool.ViewModels
{
    public class SettingsViewModel
    {
        public static void SetTheme(string name)
        {
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
            Accent accent = ThemeManager.Accents.FirstOrDefault(m=>m.Name==name);
            if (accent == null)
                return;
            ThemeManager.ChangeAppStyle(Application.Current,accent,theme.Item1);
        }
    }
}
