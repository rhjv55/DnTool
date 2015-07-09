using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FSLib.App.SimpleUpdater;
using Utilities.Log;

namespace DnTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Updater.CheckUpdateSimple("http://qxw1099210298.my3w.com/updates/client/dn/{0}", "update_c.xml");

            var updater = Updater.CreateUpdaterInstance("http://qxw1099210298.my3w.com/updates/client/dn/{0}", "update_c.xml");
            updater.Error += (s, ex) =>
            {
                Logger.Error("更新发生错误：" + updater.Context.Exception.Message);
            };
            updater.UpdatesFound += (s, ex) =>
            {
                MessageBox.Show("发现了新版本：" + updater.Context.UpdateInfo.AppVersion);
            };
           
            updater.MinmumVersionRequired += (s, ex) =>
            {
                MessageBox.Show("当前版本过低无法使用请到官网下载最新版本！");
            };
            Updater.CheckUpdateSimple();



            //var updater = Updater.CreateUpdaterInstance("http://127.0.0.1/path/{0}", "update.xml");
            //updater.Error += (s, ex) =>
            //{
            //    MessageBox.Show("更新发生错误：" + updater.Context.Exception.Message);
            //};
            //updater.UpdatesFound += (s, ex) =>
            //{
            //    MessageBox.Show("发现了新版本：" + updater.Context.UpdateInfo.AppVersion);
            //    updater.StartExternalUpdater();
            //};
            //updater.NoUpdatesFound += (s, ex) =>
            //{
            //    MessageBox.Show("没有新版本！");
            //};
            //updater.MinmumVersionRequired += (s, ex) =>
            //{
            //    MessageBox.Show("当前版本过低无法使用请到官网下载最新版本！");
            //};
            //updater.Context.EnableEmbedDialog = false;
            //updater.BeginCheckUpdateInProcess();

            //var updater = Updater.CreateUpdaterInstance("http://127.0.0.1/path/{0}","update.xml");
            //updater.EnsureNoUpdate();



          //  Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            //int num = UpdaterHelper.CheckNewFiles();
            //if (num > 0)
            //{
            //    if (MessageBox.Show(string.Format("发现{0}个文件需要更新，更新后才能使用！", num), "更新提示!", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            //    {
            //        UpdaterHelper.RunUpdater();
            //    }
            //    this.Shutdown();
            //}


           // LoginView loginView = new LoginView();
          //  loginView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           // bool? ret = loginView.ShowDialog();

            //if ((ret.HasValue) && (ret.Value))
            //{
            //    base.OnStartup(e);

            //    Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //}
            //else
            //{
            //    this.Shutdown();
            //}
        }
    }
}
