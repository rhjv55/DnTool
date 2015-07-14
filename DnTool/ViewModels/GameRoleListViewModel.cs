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
            
        }

        private void UpdateGameRoleList()
        {
            if (SoftContext.Role == null)
                return;
            DmPlugin dm = SoftContext.Role.Window.Dm;

            this._gameRoleList.Clear();

            string hwnds = dm.EnumWindowByProcess("DragonNest.exe", "", "DRAGONNEST", 2);
            List<int> hList = dm.GetHwnds(hwnds);
            foreach (var h in hList)
            {
                RoleInfo roleInfo = new RoleInfo();
                roleInfo.ID = 1;
                roleInfo.Name = dm.ReadString(h,"[170112]+111",1,10);
                roleInfo.Occupation = dm.ReadString(h,"[1170172]+222",1,10);
                roleInfo.IsTogether = false;
                roleInfo.IsMove = false;
                roleInfo.Hwnd = h;
                roleInfo.Delay = 0;
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
        public string Name { get; set; }
        public string Occupation { get; set; }
        public bool IsTogether { get; set; }
        public bool IsMove { get; set; }
        public int Hwnd { get; set; }
        public int Delay { get; set; }
    }
}
