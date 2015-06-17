using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Tasks;

namespace DnTool.ViewModels
{
    public class GameRoleViewModel
    {
        public GameRoleViewModel()
        {
            this.TaskEngine = new TaskEngine();
            this.Role = new Role(MainViewModel.Hwnd);
            TaskContext context = new TaskContext(Role);
           
        }
        public TaskEngine TaskEngine { get; set; }
        public IRole Role { get; set; }
    }
}
