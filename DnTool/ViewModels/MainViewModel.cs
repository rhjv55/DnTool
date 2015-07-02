using System;
using DnTool.Utilities;
using Utilities.Dm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.CommandWpf;
namespace DnTool.ViewModels
{
    public class MainViewModel:NotifyPropertyChanged
    {

       //  private int roleBaseAddress = 0x1221740;//游戏内存基址
       // private int moneyBaseAddress = 0x16D1E50;//背包金钱基址
        private string processName = "DragonNest";//游戏进程名字
        public static int Hwnd = 0;
   
        public RelayCommand ExitLoginCommand { get; set; }
        public RelayCommand ShowLoginCommand { get; set; }
        ViewModelLocator Locator = new ViewModelLocator();
        public MainViewModel()
        {

            this.ExitLoginCommand = new RelayCommand(()=>
            {
                Locator.Login.IsOpen = true;
                SoftContext.IsLogin = false;
            }
            ,()=>SoftContext.IsLogin);

            this.OpenCommand = new RelayCommand(() =>
            {
                try
                {
                    Process[] all = Process.GetProcessesByName("DragonNest");

                    if (all != null)
                    {
                        if (all.Length == 0)
                        {
                            return;
                        }

                        foreach (Process process in all)
                        {
                            var handles = Win32Processes.GetHandles(process, "Mutant", "\\BaseNamedObjects\\MutexDragonNest");

                            if (handles.Count == 0)
                            {
                                continue;
                            }

                            foreach (var handle in handles)
                            {
                                IntPtr ipHandle = IntPtr.Zero;
                                if (!MutexCloseHelper.DuplicateHandle(Process.GetProcessById(handle.ProcessID).Handle,
                                    handle.Handle, MutexCloseHelper.GetCurrentProcess(), out ipHandle, 0, false, MutexCloseHelper.DUPLICATE_CLOSE_SOURCE))
                                {
                                    // richTextBox1.AppendText("DuplicateHandle() failed, error =" + Marshal.GetLastWin32Error() + Environment.NewLine);


                                }
                                else
                                {
                                    MutexCloseHelper.CloseHandle(ipHandle);
                                    Debug.WriteLine("进程[" + handle.ProcessID + "]的互斥体句柄关闭成功");
                                }
                            }

                        }
                    }
                    else
                    {
                        // richTextBox1.AppendText("没有找到运行的程序" + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                   Debug.WriteLine(ex.Message);

                }
            });
           
    
            this.LoadedCommand = new RelayCommand(() =>
            {
               // WinIo.Initialize();
            });

            this.ClosedCommand = new RelayCommand(() =>
            {
                TeleportViewModel.timer.Stop();
            });
          

        }
       
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand ClosedCommand { get; set; }
        public RelayCommand LoadedCommand { get; set; }

        public void SetMessage(string message)
        {
            this.Message = Message;
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
              
                base.SetProperty(ref _message, value, () => this.Message);
            }
        }
        

    }
}
