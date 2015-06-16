using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Dm;

namespace Utilities.Tasks
{
    public interface IRole
    {
        /// <summary>
        /// 获取 角色所在窗口
        /// </summary>
        DmWindow Window { get; }
        /// <summary>
        /// 角色是否存在
        /// </summary>
        bool IsAlive { get; }

        bool FindNpc(string p1, string p2);

        void FindDialogButtonAndClick(string p);

        void CloseDialogBoard();

        bool HasDialogButton(string p);

        int Empirical { get; set; }

    }
}
