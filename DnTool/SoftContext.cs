using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Dm;
using MahApps.Metro.Controls;
using System.Net.Http;
using DnTool.ViewModels;
using Utilities.Tasks;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
namespace DnTool
{
    public class SoftContext
    {
        public SoftContext(MetroWindow window)
        {
            DmSystem = new DmSystem(new DmPlugin());
            MainWindow = window;
            HttpClient = new HttpClient();
            IsLogin = false;
            TaskEngine = new TaskEngine();
            TaskEngine.OutMessage = ShowMessage;
        }
        private void ShowMessage(string title, string message)
        {
            if(SoftContext.MainWindow!=null)
                SoftContext.MainWindow.Dispatcher.Invoke(new ThreadStart(delegate { SoftContext.MainWindow.ShowMessageAsync(title, message); }));
        }
        public static DmSystem DmSystem { get; set; }
        public static MetroWindow MainWindow { get; set; }
        public static HttpClient HttpClient { get; set; }
        public static bool IsLogin { get; set; }
        public static TaskEngine TaskEngine { get; set; }
        public static IRole Role { get; set; }
    }
}
