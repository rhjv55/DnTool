using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Tasks;

namespace DnTool.ViewModels
{
    public class GameRoleListViewModel
    {
        private static async Task GameRolesTaskStop(IEnumerable<GameRoleViewModel> roleModels)
        {
            foreach (var roleModel in roleModels)
            {
                TaskEngine engine = roleModel.TaskEngine;
                if (engine != null)
                {
                    //await TaskEx.Run(()=>engine.Stop());
                }
            }
        }
    }
}
