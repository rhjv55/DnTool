using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Dm;
using MahApps.Metro.Controls;
using System.Net.Http;
namespace DnTool
{
    public class SoftContext
    {
        public SoftContext(MetroWindow window)
        {
            DmSystem = new DmSystem(new DmPlugin());
            MainWindow = window;
            HttpClient = new HttpClient();
        }
        public static DmSystem DmSystem { get; set; }
        public static MetroWindow MainWindow { get; set; }
        public static HttpClient HttpClient { get; set; }

    }
}
