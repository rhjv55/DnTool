using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Tasks;
using Utilities.Dm;
using DnTool.Models;

namespace DnTool
{
    public class Role:IRole
    {



        public DmWindow Window
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAlive
        {
            get { throw new NotImplementedException(); }
        }

        public bool FindNpc(string p1, string p2)
        {
            throw new NotImplementedException();
        }

        public void FindDialogButtonAndClick(string p)
        {
            throw new NotImplementedException();
        }

        public void CloseDialogBoard()
        {
            throw new NotImplementedException();
        }

        public bool HasDialogButton(string p)
        {
            throw new NotImplementedException();
        }

        public int Empirical
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public bool aaa(IRole role)
        {
            bool wusun = false;
            DmPlugin dm = role.Window.Dm;
            bool flag = Delegater.WaitTrue(() => role.IsAlive,
                () =>
                {
                    dm.MoveToClick(wusun ? 400 : 280, 270);
                    dm.Delay(2000);
                },
                5);
            if (flag)
            {
             //   OverweightDialogClose(role);
            }
            return flag;
        }
    }
}
