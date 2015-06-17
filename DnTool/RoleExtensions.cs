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
    /// <summary>
    /// 角色拓展类
    /// </summary>
    public static class RoleExtensions 
    {
        /// <summary>
        /// 商城界面是否有某个物品
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name">物品的名字</param>
        /// <returns></returns>
        public static bool HasMallThing(this IRole role,string name)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindPicE(0, 0, role.Window.Width, role.Window.Height, name + ".bmp")==""?true:false;
        }

        public static bool FindMallButtonAndClick(this IRole role, string name)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindPicE_LeftClick(0, 0, role.Window.Width,role.Window.Height, name + ".bmp",35,85)? true : false; 

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

        #region xxx
       
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
        #endregion
    }
}
