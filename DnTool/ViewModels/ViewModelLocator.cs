using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System.Diagnostics;

namespace DnTool.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<TeleportViewModel>();
            SimpleIoc.Default.Register<GameRoleListViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }
        public TeleportViewModel Teleport
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TeleportViewModel>();
            }
        }
        public GameRoleListViewModel SetXiaohao
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GameRoleListViewModel>();
            }
        }
        public static void Cleanup()
        {
            
        }
    }
}