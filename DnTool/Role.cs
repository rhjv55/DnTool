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
        public Money BagMoney { get { return this.GetBagMoney(); } }
        public Money RepertoryMoney { get { return this.GetRepertoryMoney(); } }
        public uint MallVolume { get { return this.GetMallVolume(); } }
        public uint MallLB { get { return this.GetMallLB(); } }

        private readonly DmWindow _window;
        public Role(int hwnd)
        {
            _window = new DmWindow(hwnd,new DmPlugin());
           
        }
        public DmWindow Window
        {
            get { return _window; }
        }

        #region 私有方法
        /// <summary>
        /// 获取背包金钱
        /// </summary>
        /// <returns></returns>
        private Money GetBagMoney()
        {
            DmPlugin dm = Window.Dm;
            int hwnd = Window.Hwnd;
            int val = dm.ReadInt(hwnd, "[16D1E50]+68", 0);
            Money money = new Money();
            money.Gold = (uint)val / 10000;
            money.Silver = (uint)val % 10000 / 100;
            money.Copper = (uint)val % 10000 % 100;
            return money;
        }
        /// <summary>
        /// 获取仓库金钱
        /// </summary>
        /// <returns></returns>
        private Money GetRepertoryMoney()
        {
            DmPlugin dm = Window.Dm;
            int hwnd = Window.Hwnd;
            int val = dm.ReadInt(hwnd, "[16D1E50]+70", 0);
            Money money = new Money();
            money.Gold = (uint)val / 10000;
            money.Silver = (uint)val % 10000 / 100;
            money.Copper = (uint)val % 10000 % 100;
            return money;
        }

        /// <summary>
        /// 获得商城点卷数
        /// </summary>
        /// <returns></returns>
        private uint GetMallVolume()
        {
            DmPlugin dm = Window.Dm;
            int hwnd = Window.Hwnd;
            int val = dm.ReadInt(hwnd, "[16D208C]+234", 0);
            return (uint)val;
        }
        /// <summary>
        /// 获得商城龙币数
        /// </summary>
        /// <returns></returns>
        private uint GetMallLB()
        {
            DmPlugin dm =Window.Dm;
            int hwnd =Window.Hwnd;
            int val = dm.ReadInt(hwnd, "[16D208C]+238", 0);
            return (uint)val;
        }
        #endregion





        public bool IsAlive
        {
            get { throw new NotImplementedException(); }
        }

        public bool FindNpc(string p1, string p2)
        {
            throw new NotImplementedException();
        }

        public bool FindDialogButtonAndClick(string button)
        {
            return _window.Dm.FindStrE_LeftClick(0, 0, 2000, 2000, button, "BEBEBE-414141", 0, 0, 0.9);
        }

        public void CloseDialogBoard()
        {
            throw new NotImplementedException();
        }

        public bool HasDialogButton(string p)
        {
            return _window.Dm.FindStr(0, 0, 2000, 2000, p, "BEBEBE-414141",0.9);
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
