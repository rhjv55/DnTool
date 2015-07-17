using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using DnTool.Utilities;
using Utilities.Dm;
using Utilities.Log;
using MahApps.Metro.Controls.Dialogs;
using System.Net.Http;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;


namespace DnTool.ViewModels
{
    public class LoginViewModel:FlyoutBaseViewModel
    {
      
        public RelayCommand LoginCommand { get; set; }
        public LoginViewModel()
        {
            this.Header = "登录";
            this.Position = Position.Right;
            this.IsOpen = true;
            this.LoginCommand = new RelayCommand(()=>this.Login());
            this._username = "admin";
            this._password = "admin";
        }

        private async void Login()
        {
            await Vlogin();
        }

        public async Task Vlogin()
        {
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
                return;
            //DmSystem system = SoftContext.DmSystem;
            //string url = "http://127.0.0.1/accounts/login";
            //var param = new { _username, _password, ClientSystem = system };
            Logger.Debug("连接服务器开始登录");
            ProgressDialogController progress = await
                SoftContext.MainWindow.ShowProgressAsync("请稍候", "正在登录,请稍候......");
            //HttpResponseMessage response = await SoftContext.HttpClient.PostAsJsonAsync(url, param);
            //if (response.IsSuccessStatusCode)
            //{
               await progress.CloseAsync();
            //    string message = response.ReasonPhrase;
            //    await SoftContext.MainWindow.ShowMessageAsync("登录失败", message);
            //    message = "登录验证失败：{0}".FormatWith(message);
            //    Logger.Error(message);
            //    return;
            //}
            //await progress.CloseAsync();
            Logger.Debug("登录成功");
            this.IsOpen = false;
            SoftContext.IsLogin = true;
        }
       

 
        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                base.SetProperty(ref _username, value, () => this.Username);
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                base.SetProperty(ref _password, value, () => this.Password);
            }
        }
        
    }
}
