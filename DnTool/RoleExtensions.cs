using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Tasks;
using Utilities.Dm;
using System.Diagnostics;
using DnTool.Models;
namespace DnTool
{
    public class RoleExtensions : IRole
    {

        private RoleExtensions role;
        public DmWindow Window
        {
            get { throw new NotImplementedException(); }
        }


        public bool aaa()
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
                OverweightDialogClose(role);
            }
            return flag;
        }

        private void OverweightDialogClose(RoleExtensions role)
        {
            throw new NotImplementedException();
        }


        public bool IsAlive
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 获取背包中指定名称的物品,不存在返回null
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Thing GetBagThing(this IRole role, string name, int page = 0)
        {
            if (name == null) throw new Exception("");
            Debug.WriteLine("在背包0中查找物品1" + GetBagPageName(page), name);
            return GetBagThings(role, page).FirstOrDefault(m => m.Name == name);
        }

        public static string GetBagPageName(int page)
        {
            throw new NotImplementedException();
        }

        private static Thing[] GetBagThings(IRole role, int page)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 背包中是否存在指定名称的物品
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name">物品名称</param>
        /// <param name="page">页码，为0时取整个背包数据</param>
        /// <returns></returns>
        public static bool HasBagThing(this IRole role, string name, int page = 0)
        {
            if (name == null) return false;
            Debug.WriteLine("检测背包0中是否存在物品1" + GetBagPageName(page) + name);
            return GetBagThings(role, page).Any(m => m.Name == name);
        }
        public static Blank[] GetBagBlanks(int page)
        {
            return new Blank[10];
        }
        public static Blank[] GetBagBlanks(this IRole role)
        {
            return GetBagBlanks(1).Union(GetBagBlanks(2)).ToArray();
        }


        public void BagCleanup()
        {
        }
        public string GetBagPageName(Thing thing)
        {
            return "";
        }

        public string GetRepertoryPageName(Thing thing)
        {
            return "";
        }
        public string GetRepertoryPageName(int page)
        {
            return "";
        }

        public Thing[] GetRepertoryThings()
        {
            return null;
        }
        public Thing GetRepertoryThing(string name, int page = 0)
        {
            return null;
        }
        public Thing GetRepertoryThing(string name)
        {
            return null;
        }

        //public Blank[] GetRepertoryBlanks()
        //{
        //}
        public void RepertoryCleanup()
        {
        }
        public bool PutBagThingsToRepertory()
        {
            return true;
        }
        public bool GetRepertoryThingsToBag()
        {
            return true;
        }
        public bool GetRepertoryMoneyToBag()
        {
            return true;
        }
        public bool MoveThing(Thing thing)
        {
            return true;
        }
        public bool PickupThingToMouse()
        {
            return true;
        }
        public bool PutdownMouseThing()
        {
            return true;
        }
        public bool PutMouseThingToBag()
        {
            return true;
        }
        public Thing GetMouseThing()
        {
            return null;
        }
        public Thing GetMousePointThing()
        {
            return null;
        }

        public void HasDialogDetail(string name, string color)
        {
            //role.HasDialogDetail("任务道具",Resources.Fsb_Color_纯红色)

        }
        public void HasDialogButton(string name)
        {

        }
    }
}
