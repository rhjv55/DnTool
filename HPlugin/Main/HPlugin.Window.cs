using IPlugin.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IPlugin.Main
{
    public partial class HPlugin
    {
        /// <summary>
        /// 根据进程名获得窗口句柄 
        /// </summary>
        /// <param name="ProssName">进程名,区分大小写，不需要带上进程后缀</param>
        /// <returns></returns>
        public List<IntPtr> EnumWindowByProcessName(string ProssName)
        {
            List<IntPtr> list = new List<IntPtr>();
            try
            {
                Process[] pp = Process.GetProcessesByName(ProssName);
                for (int i = 0; i < pp.Length; i++)
                {
                    if (pp[i].ProcessName == ProssName)
                    {
                        list.Add(pp[i].MainWindowHandle);
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        /// <summary>
        /// 将窗口坐标转换为屏幕坐标
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool ClientToScreen(int hwnd,out int x,out int y)
        {
            if (!Win32API.IsWindow((IntPtr)hwnd))
            {
                x = -1;
                y=-1;
                return false;
            }
            Point point=new Point();
            bool ret=Win32API.ClientToScreen((IntPtr)hwnd,ref point);
            x=point.X;
            y=point.Y;
            return ret;
        }
	 
        /// <summary>
        /// 将屏幕坐标转为窗口坐标
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool ScreenToClient(int hwnd,out int x,out int y)
        {
            if (!Win32API.IsWindow((IntPtr)hwnd))
            {
                x = -1;
                y = -1;
                return false;
            }
            Point point=new Point();
            bool ret=Win32API.ScreenToClient((IntPtr)hwnd,ref point);
            x=point.X;
            y=point.Y;
            return ret;
        }
        /// <summary>
        /// 获取窗口客户区域在屏幕上的位置
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public bool GetClientRect(int hwnd,out int x1,out int y1,out int x2,out int y2)
        {
            if (!Win32API.IsWindow((IntPtr)hwnd))
            {
                x1 = -1;
                y1 = -1;
                x2 = -1;
                y2 = -1;
                return false;
            }
            Win32API.RECT rect = new Win32API.RECT();
            bool a=Win32API.GetClientRect((IntPtr)hwnd, out rect);

            Point point=new Point(){X=rect.Left,Y=rect.Top};
            bool b=Win32API.ClientToScreen((IntPtr)hwnd,ref point);
            x1 = point.X;
            y1 = point.Y;

            point=new Point(){X=rect.Right,Y=rect.Bottom};
            bool c=Win32API.ClientToScreen((IntPtr)hwnd,ref point);
            x2 = point.X;
            y2 = point.Y;
            return a && b && c;
        }
        /// <summary>
        /// 获取窗口客户区域的宽度和高度
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool GetClientSize(int hwnd,out int width,out int height)
        {
            if (!Win32API.IsWindow((IntPtr)hwnd))
            {
                width = -1;
                height = -1;
                return false;
            }
	     	Win32API.RECT clientrect;
			bool ret=Win32API.GetClientRect((IntPtr)hwnd,out clientrect);
			width=clientrect.Right-clientrect.Left;
			height=clientrect.Bottom-clientrect.Top;
		    return ret;
        }
        /// <summary>
        /// 获取指定窗口的一些属性
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="flag">
        ///0 : 判断窗口是否存在
        ///1 : 判断窗口是否处于激活
        ///2 : 判断窗口是否可见
        ///3 : 判断窗口是否最小化
        ///4 : 判断窗口是否最大化
        ///5 : 判断窗口是否置顶
        ///6 : 判断窗口是否无响应
        ///7 : 判断窗口是否可用(灰色为不可用)
        /// </param>
        /// <returns></returns>
        public bool GetWindowState(int hwnd,int flag)
        {
            bool bret=false;
	        IntPtr wnd=(IntPtr)hwnd;
	        if(flag==0)	 //0://判断窗口是否存在
		        bret=Win32API.IsWindow(wnd);
	        else if(flag==1)//判断窗口是否处于激活
		        {
		            if(Win32API.GetActiveWindow()==wnd)
			        bret=true;
		        }
	        else if(flag==2)//2 : 判断窗口是否可见
		        bret=Win32API.IsWindowVisible(wnd);
	        else if(flag==3)//3 : 判断窗口是否最小化
		        bret=Win32API.IsIconic(wnd);
	        else if(flag==4)//4 : 判断窗口是否最大化
		        bret=Win32API.IsZoomed(wnd);
	        else if(flag==5)//5 : 判断窗口是否置顶
		        {
		           if(Win32API.GetForegroundWindow()==wnd)
			        bret=true;
		        }
	        else if(flag==6) //6 : 判断窗口是否无响应
		        bret=Win32API.IsHungAppWindow(wnd);
	        else if(flag==7) //判断窗口是否可用(灰色为不可用)
                bret = Win32API.IsWindowEnabled(wnd);

	        return bret;
        }

        public bool SetWindowSize()
        {
             bool bret=false;
       //if(type==0)//SetClientSize
       //    {
       //        RECT  rectProgram,rectClient;
       //        HWND hWnd=(HWND)hwnd;
       //        ::GetWindowRect(hWnd, &rectProgram);   //获得程序窗口位于屏幕坐标
       //        ::GetClientRect(hWnd, &rectClient);      //获得客户区坐标
       //        //非客户区宽,高
       //        int nWidth = rectProgram.right - rectProgram.left -(rectClient.right - rectClient.left); 
       //        int nHeiht = rectProgram.bottom - rectProgram.top -(rectClient.bottom - rectClient.top); 
       //        nWidth += width;
       //        nHeiht += hight;
       //        rectProgram.right =  nWidth;
       //        rectProgram.bottom =  nHeiht;
       //        int showToScreenx = GetSystemMetrics(SM_CXSCREEN)/2-nWidth/2;    //居中处理
       //        int showToScreeny = GetSystemMetrics(SM_CYSCREEN)/2-nHeiht/2;
       //        bret=::MoveWindow(hWnd, showToScreenx, showToScreeny, rectProgram.right, rectProgram.bottom, false);
       //    }
       //else	//SetWindowSize
       //    {
       //         RECT rectClient;
       //         HWND hWnd=(HWND)hwnd;
       //         ::GetWindowRect(hWnd, &rectClient);   //获得程序窗口位于屏幕坐标
       //         bret=::MoveWindow(hWnd, rectClient.left, rectClient.top, width, hight, false);
       //    }
	   return bret;
        }

        public bool SetWindowState()
        {
            bool bret=false;
        //HWND hWnd=(HWND)hwnd;
        //if(IsWindow(hWnd)==false)
        //    return bret;
        //int type=-1;
        //type=flag;
        //if(flag==0)//关闭指定窗口
        //    ::SendMessage(hWnd,WM_CLOSE,0,0);
        //else if(flag==1)//激活指定窗口
        //{
        //    ::ShowWindow(hWnd,SW_SHOW);
        //    ::SetForegroundWindow(hWnd);
        //}
        //else if(flag==2)//最小化指定窗口,但不激活
        //    ::ShowWindow(hWnd,SW_SHOWMINNOACTIVE);
        //else if(flag==3)//最小化指定窗口,并释放内存,但同时也会激活窗口	
        //    ::ShowWindow(hWnd,SW_SHOWMINIMIZED);
        //else if(flag==4)//最大化指定窗口,同时激活窗口.
        //    ::ShowWindow(hWnd,SW_SHOWMAXIMIZED);
        //else if(flag==5)//恢复指定窗口 ,但不激活
        //    ::ShowWindow(hWnd,SW_SHOWNOACTIVATE);
        //else if(flag==6)//隐藏指定窗口
        //    ::ShowWindow(hWnd,SW_HIDE);
        //else if(flag==7)//显示指定窗口
        //{
        //    ::ShowWindow(hWnd,SW_SHOW);
        //    ::SetForegroundWindow(hWnd);
        //}
        //else if(flag==8)//置顶指定窗口
        //    ::SetWindowPos(hWnd,HWND_TOPMOST,0,0,0,0,SWP_NOMOVE|SWP_NOSIZE);//窗口置顶
        //else if(flag==9)//9 : 取消置顶指定窗口
        //    ::SetWindowPos(hWnd, HWND_NOTOPMOST,0,0,0,0,SWP_NOMOVE|SWP_NOSIZE);
        //else if(flag==10)//禁止指定窗口
        //    ::EnableWindow(hWnd,false);
        //else if(flag==11)//取消禁止指定窗口
        //    ::EnableWindow(hWnd,true);
        //else if(flag==12)//12 : 恢复并激活指定窗口
        //    ::ShowWindow(hWnd,SW_RESTORE);
        //else if(flag==13)//13 : 强制结束窗口所在进程.
        //    {
        //        DWORD pid=0;
        //        ::GetWindowThreadProcessId(hWnd,&pid);
        //        TSRuntime::EnablePrivilege(L"SeDebugPrivilege",true);
        //        HANDLE hprocess=NULL;
        //        if(my_OpenProcess)
        //             hprocess=my_OpenProcess(PROCESS_ALL_ACCESS,false,pid);
        //        else
        //             hprocess=::OpenProcess(PROCESS_ALL_ACCESS,false,pid);

        //        ::TerminateProcess(hprocess,0);
        //    }
        //else if(flag==14)//14 : 闪烁指定的窗口
        //    {
        //      FLASHWINFO fInfo;
        //      fInfo.cbSize=sizeof(FLASHWINFO);
        //      fInfo.dwFlags=FLASHW_ALL | FLASHW_TIMERNOFG;//这里是闪动窗标题和任务栏按钮,直到用户激活窗体
        //      fInfo.dwTimeout= 0;
        //      fInfo.hwnd=hWnd;
        //      fInfo.uCount=0xffffff;
        //      FlashWindowEx(&fInfo);
        //    }
        //else if(flag==15)//使指定的窗口获取输入焦点
        //    {
        //    ::ShowWindow(hWnd,SW_SHOW);
        //    ::SetFocus(hWnd);
        //    }

        //if(type>=0&&type<16)
        //   bret=true;

		return bret;
        }










	}


    
}
