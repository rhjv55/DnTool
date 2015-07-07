using System;
using DnTool.Utilities;
using Utilities.Dm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.CommandWpf;
namespace DnTool.ViewModels
{
    public class MainViewModel:NotifyPropertyChanged
    {
        public static int Hwnd = 0;
        ViewModelLocator Locator = new ViewModelLocator();
        public MainViewModel()
        {
            this.SettingsCommand = new RelayCommand(() =>
            {
                Locator.Settings.IsOpen = true;
            });

            this.ExitLoginCommand = new RelayCommand(()=>
            {
                Locator.Login.IsOpen = true;
                SoftContext.IsLogin = false;
            }
            ,()=>SoftContext.IsLogin);

            this.LoadedCommand = new RelayCommand(() =>
            {
               // WinIo.Initialize();
            });

            this.ClosedCommand = new RelayCommand(() =>
            {
                TeleportViewModel.timer.Stop();
            });
        }

        public RelayCommand SettingsCommand { get; set; }
        public RelayCommand ExitLoginCommand { get; set; }
        public RelayCommand ClosedCommand { get; set; }
        public RelayCommand LoadedCommand { get; set; }

      

    }
}
