using DnTool.Models;
using DnTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.ViewModels
{
    public class InfoViewModel:ViewModelBase
    {
        public Point CurrentPoint { get; set; }
        public int PID { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
