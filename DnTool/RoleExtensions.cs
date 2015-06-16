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
    public static class RoleExtensions 
    {
        /// <summary>
        /// 获取背包金钱
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static Money GetBagMoney(this IRole role)
        {
           DmPlugin dm=role.Window.Dm;
           int hwnd = role.Window.Hwnd;
           int val = dm.ReadInt(hwnd, "[16D1E50]+68", 0);
           Money money = new Money();
           money.Gold = val / 10000;
           money.Silver = val % 10000 / 100;
           money.Copper = val % 10000 % 100;
           return money;
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


        public static void BagCleanup(this IRole role)
        {
        }
        public static string GetBagPageName(Thing thing)
        {
            return "";
        }

        public static string GetRepertoryPageName(Thing thing)
        {
            return "";
        }
        public static string GetRepertoryPageName(int page)
        {
            return "";
        }

        public static Thing[] GetRepertoryThings()
        {
            return null;
        }
        public static Thing GetRepertoryThing(string name, int page = 0)
        {
            return null;
        }
        public static Thing GetRepertoryThing(string name)
        {
            return null;
        }

        //public Blank[] GetRepertoryBlanks()
        //{
        //}
        public static void RepertoryCleanup()
        {
        }
        public static bool PutBagThingsToRepertory()
        {
            return true;
        }
        public static bool GetRepertoryThingsToBag()
        {
            return true;
        }
        public static bool GetRepertoryMoneyToBag()
        {
            return true;
        }
        public static bool MoveThing(Thing thing)
        {
            return true;
        }
        public static bool PickupThingToMouse()
        {
            return true;
        }
        public static bool PutdownMouseThing()
        {
            return true;
        }
        public static bool PutMouseThingToBag()
        {
            return true;
        }
        public static Thing GetMouseThing()
        {
            return null;
        }
        public static Thing GetMousePointThing()
        {
            return null;
        }

        public static void HasDialogDetail(string name, string color)
        {
            //role.HasDialogDetail("任务道具",Resources.Fsb_Color_纯红色)

        }
        public static void HasDialogButton(string name)
        {

        }
    }
}
