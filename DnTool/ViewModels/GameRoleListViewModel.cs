using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using DnTool.Utilities.HYX;
using Utilities.Dm;
namespace DnTool.ViewModels
{
    public class GameRoleListViewModel : FlyoutBaseViewModel
    {
        public GameRoleListViewModel()
        {
            this.Header = "角色列表";
            this.Position = Position.Right;
            this.OpendEvent +=(s,e)=>UpdateGameRoleList();
        }

        private void UpdateGameRoleList()
        {
            if (SoftContext.Role == null)
                return;
            DmPlugin dm = SoftContext.Role.Window.Dm;
            var tempList = new ObservableCollection<RoleInfo>();
            foreach (var item in this._gameRoleList)
            {
                tempList.Add(item);
            }
            this._gameRoleList.Clear();

            string hwnds = dm.EnumWindowByProcess("DragonNest.exe", "", "DRAGONNEST", 2);
            List<int> hList = dm.GetHwnds(hwnds);
            foreach (var h in hList)
            {
                RoleInfo roleInfo = new RoleInfo();
                roleInfo.ID = this._gameRoleList.Count+1;
                roleInfo.PID = dm.GetWindowProcessId(h);
                roleInfo.Occupation = dm.ReadString(h, "[1221740]+e50", 1, 10);
                roleInfo.Hwnd = h;

                var info=tempList.FirstOrDefault(x=>x.Hwnd==h);
                if (info == null)
                {
                    roleInfo.IsTogether = false;
                    roleInfo.IsMove = false;
                    roleInfo.Delay = 0;
                }
                else
                {
                    roleInfo.IsTogether = info.IsTogether;
                     roleInfo.IsMove = info.IsMove;
                     roleInfo.Delay = info.Delay;
                }
                this._gameRoleList.Add(roleInfo);
            }
        }

        private ObservableCollection<RoleInfo> _gameRoleList = new ObservableCollection<RoleInfo>();

        public ObservableCollection<RoleInfo> GameRoleList
        {
            get { return _gameRoleList; }
            set { _gameRoleList = value; }
        }
       
    }

    public class RoleInfo
    {
        public int ID { get; set; }
        public int  PID { get; set; }
        public string Occupation { get; set; }
        public bool IsTogether { get; set; }
        public bool IsMove { get; set; }
        public int Hwnd { get; set; }
        public int Delay { get; set; }
    }
}
