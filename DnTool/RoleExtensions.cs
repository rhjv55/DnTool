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
        /// 获取鼠标所指向控件的文本
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string GetMousePointContent(this IRole role)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.ReadString(role.Window.Hwnd,"[]+30",1,20);
        }

        /// <summary>
        /// 商城界面是否有某个物品
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name">物品的名字</param>
        /// <returns></returns>
        public static bool HasMallThing(this IRole role,string name)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindPicE(0, 0, role.Window.Width, role.Window.Height, name + ".bmp")==""?false:true;
        }

        public static bool FindMallButtonAndClick(this IRole role, MallThing thing)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindPicE_LeftClick(0, 0, role.Window.Width,role.Window.Height, thing.Name + ".bmp",35,85)? true : false; 

        }
        /// <summary>
        /// 获取背包中指定名称的物品,不存在返回null
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static MallThing GetBagThing(this IRole role, string name, int page = 0)
        {
            if (name == null) throw new Exception("");
            Debug.WriteLine("在背包0中查找物品1" + GetBagPageName(page), name);
            return GetBagThings(role, page).FirstOrDefault(m => m.Name == name);
        }

        public static string GetBagPageName(int page)
        {
            throw new NotImplementedException();
        }

        private static MallThing[] GetBagThings(IRole role, int page)
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

        public static MallThing GetShortcutThing(this IRole role,int num)
        {
            return null;
        }
        /// <summary>
        /// 是否有任务的关键项目,任务名，文本关键字,物品名,npc名
        /// </summary>
        /// <param name="role"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool HasTaskItem(this IRole role,params string[] args)
        {
            return true;
        }
      
        /// <summary>
        /// 是否有buff
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool HasBuffState(this IRole role)
        {
            return true;
        }
        /// <summary>
        /// 是否有按钮
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasButton(this IRole role,string name)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindStr(0, 0, 2000, 2000, name, "BEBEBE-414141", 0.9);
        }

        public static bool FindButtonAndClick(this IRole role, string name)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindStrE_LeftClick(0, 0, 2000, 2000, name, "BEBEBE-414141");
        }
        /// <summary>
        /// 是否有对话框
        /// </summary>
        /// <param name="role"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public static bool HasDialogBoard(this IRole role,string boardName)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindStr(0, 0, 2000, 2000, boardName, "BEBEBE-414141", 0.9);
        }
        /// <summary>
        /// 是否有对话框按钮
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasDialogButton(this IRole role,string name)
        {
            return true;
        }

        /// <summary>
        /// 是否有对话框详细信息  Resources.Fsb_Color_纯红色
        /// </summary>
        /// <param name="role"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static bool HasDialogDetail(this IRole role, string detail,string color)
        {
            return role.Window.Dm.FindStr(0,0,role.Window.Width,role.Window.Height,detail,color);
        }
        /// <summary>
        /// 是否有没有按钮的对话框
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasNoButtonDialog(this IRole role, string name)
        {
            return true;
        }
        /// <summary>
        /// 角色是否在坐标上
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasRoleInPoint(this IRole role, string name)
        {
            return true;
        }
        /// <summary>
        /// 是否快捷栏有物品
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasShortcurThing(this IRole role, string name)
        {
            return true;
        }

        public static void CloseDialogBoard()
        {
            
        }
        #region xxx
       
        /// <summary>
        /// 清理指定页背包
        /// </summary>
        /// <param name="role"></param>
        /// <param name="page">背包页（1-5）</param>
        /// <param name="begin">开始格数（1-30）</param>
        /// <param name="stop">结束格数（1-30）</param>
        public static void BagCleanup(this IRole role,int page,int begin,int stop)
        {
            DmPlugin dm = role.Window.Dm;
            int hwnd = role.Window.Hwnd;
            dm.MoveToClick(794 + 65 * (page - 1), 217);  //点击背包页
            dm.Delay(500);
            //5-28格
            for (int i = begin; i <= stop; i++)
            {
                dm.MoveTo(800 + 65 * ((i - 1) % 5), 274 + 65 * ((i - 1) / 5));
                int num = dm.ReadInt(hwnd, "[16cbc90]+314", 0);
                if (num == 0)
                    continue;
                Debug.WriteLine(800 + 65 * ((i - 1) % 5) + "   " + (274 + 65 * ((i - 1) / 5)));
                Delegater.WaitTrue(() => role.HasDialogDetail("破坏物品", "bebebe-414141"),
                () => 
                {
                    dm.MoveToClick(800 + 65 * ((i - 1) % 5), 274 + 65 * ((i - 1) / 5));  //点击格子
                    dm.Delay(200);
                    dm.MoveToClick(748, 687);                  //点击丢弃
                    dm.Delay(100);
                });
                dm.MoveToClick(574, 496);                   //点击确认
            }
        }
       /// <summary>
       /// 清理背包，从第几页几格到几页几格
       /// </summary>
       /// <param name="role"></param>
       /// <param name="beginPage">开始页数</param>
       /// <param name="beginItem">开始格数</param>
       /// <param name="stopPage">停止页数</param>
       /// <param name="sotpItem">停止格数</param>
        public static void BagCleanup(this IRole role,int beginPage,int beginItem,int stopPage,int sotpItem)
        {
            if (beginPage == stopPage)
                role.BagCleanup(beginPage, beginItem, sotpItem);
            if (beginPage < stopPage)
            {
                for (int i = beginPage; i <= stopPage; i++)
                {
                    if (i == stopPage)
                        role.BagCleanup(i, 1, sotpItem);
                    if (i == beginPage)
                        role.BagCleanup(i, beginItem, 30);
                    if (i < stopPage && i != beginPage)
                        role.BagCleanup(i, 1, 30);
                }
            }

        }
        /// <summary>
        /// 是否有打开面板
        /// </summary>
        /// <param name="role"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public static bool HasBoard(this IRole role,string boardName)
        {
            DmPlugin dm = role.Window.Dm;
            return dm.FindStr(0, 0, role.Window.Width, role.Window.Height, boardName, "bebebe-414141");
        }
        public static string GetBagPageName(MallThing thing)
        {
            return "";
        }

        public static string GetRepertoryPageName(MallThing thing)
        {
            return "";
        }
        public static string GetRepertoryPageName(int page)
        {
            return "";
        }

        public static MallThing[] GetRepertoryThings()
        {
            return null;
        }
        public static MallThing GetRepertoryThing(string name, int page = 0)
        {
            return null;
        }
        public static MallThing GetRepertoryThing(string name)
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
        public static bool MoveThing(MallThing thing)
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
        public static MallThing GetMouseThing()
        {
            return null;
        }
        public static MallThing GetMousePointThing()
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
