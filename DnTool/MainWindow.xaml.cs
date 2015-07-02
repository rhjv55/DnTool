using DnTool.Views;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls.Dialogs;

namespace DnTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow :MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
           // this.ShowTitleBar = false;
            this.ShowIconOnTitleBar = true;
            this.ShowMinButton = true;
            this.ShowMaxRestoreButton = false;
            this.ShowCloseButton = true;
            this.ResizeMode = ResizeMode.CanMinimize;
           // this.SaveWindowPosition = true;
            this.LeftWindowCommandsOverlayBehavior = WindowCommandsOverlayBehavior.Never;
            this.RightWindowCommandsOverlayBehavior = WindowCommandsOverlayBehavior.Never;
            this.image1.Source = EyeHelper.ChangeBitmapToImageSource(softwatcher.Properties.Resources.drag);
          
            this.Flyouts1.Items.Add(new LoginFlyout());
            new SoftContext(this);
        }


       

       

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            this.ToggleFlyout(0);
        }
        private void ToggleFlyout(int index)
        {
            var flyout = this.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }
        private bool IsDragging = false;
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.IsDragging = true;
                this.CaptureMouse(true);
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsDragging)
            {
                this.IsDragging = false;
                this.CaptureMouse(false);
            }
        }
        /// <summary>
        /// 当前鼠标下的指向的句柄
        /// </summary>
        private IntPtr _hWndCurrent;

        /// <summary>
        /// 捕获或释放鼠标
        /// </summary>
        /// <param name="captured">true捕获,false释放</param>
        private void CaptureMouse(bool captured)
        {
            if (captured)
            {
                var hwnd = EyeHelper.SetCapture(new WindowInteropHelper(this).Handle);
                this.Cursor = new Cursor(new MemoryStream(softwatcher.Properties.Resources.eye));
                this.image1.Source = EyeHelper.ChangeBitmapToImageSource(softwatcher.Properties.Resources.drag2);
            }
            else
            {
                this.image1.Source = EyeHelper.ChangeBitmapToImageSource(softwatcher.Properties.Resources.drag);
                this.Cursor = Cursors.Arrow;

                var ret = EyeHelper.ReleaseCapture();
                if (this._hWndCurrent != IntPtr.Zero)
                {
                   // EyeHelper.DrawRevFrame(this._hWndCurrent); //还原边框
                    //this._hWndCurrent = IntPtr.Zero;
                }

            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
        }
        
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEMOVE = 512;
        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_LBUTTONUP://鼠标左键弹起
                    {
                        if (this.IsDragging)
                        {
                            this.IsDragging = false;
                            this.CaptureMouse(false);
                            handled = true;

                            uint pid;
                            EyeHelper.GetWindowThreadProcessId(this._hWndCurrent,out pid);
                            if (pid > 0)
                            {
                                Process p = Process.GetProcessById((int)pid);
                                if (p.ProcessName == "notepad")
                                {
                                    //鼠标最后指向的句柄
                                    SoftContext.Role = new Role((int)this._hWndCurrent);
                                    if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\resources"))
                                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\resources");
                                    SoftContext.Role.Window.Dm.SetPath(AppDomain.CurrentDomain.BaseDirectory+"\\resources");
                                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\dnDict.txt"))
                                        File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\dnDict.txt");
                                    SoftContext.Role.Window.Dm.SetDict(0,"dnDict.txt");
                                    
                                }
                                else
                                {
                                    this.ShowMessageAsync("绑定失败", "请选择游戏窗口！");
                                }
                            }
                          
                          
                           
                        }
                    }
                    break;
                case WM_MOUSEMOVE://鼠标移动
                    {
                        if (this.IsDragging)
                        {
                            IntPtr hWnd = EyeHelper.WindowFromPoint(System.Windows.Forms.Control.MousePosition);
                            if (this._hWndCurrent != hWnd)
                            {
                               // EyeHelper.DrawRevFrame(this._hWndCurrent);
                               // EyeHelper.DrawRevFrame(hWnd);
                                this._hWndCurrent = hWnd;
                                // 想显示当前句柄的位置如this.tbHwnd.Text = hWnd.ToInt32().ToString();
                            }
                            handled = true;
                        }
                    }
                    break;
            }
            return IntPtr.Zero;
        }

 
    }
    /// <summary>
    /// WPF实现SPY++Eye效果帮助类
    /// </summary>
    public class EyeHelper
    {
        [Serializable]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #region WIN32API
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public extern static int RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        public extern static bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(System.Drawing.Point Point);

        [DllImport("user32.dll")]
        public static extern bool OffsetRect(ref Rect lprc, int dx, int dy);

        [DllImport("gdi32.dll")]
        public static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, PatBltTypes dwRop);

        [DllImport("gdi32.dll")]
        public static extern bool PlgBlt(IntPtr hdcDest, ref System.Drawing.Point lpPoint, IntPtr hdcSrc, int nXSrc, int nYSrc, int nWidth, int nHeight, IntPtr hbmMask, int xMask, int yMask);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        [DllImport("kernel32.dll")]
        public extern static IntPtr LoadLibrary(String path);

        [DllImport("kernel32.dll")]
        public extern static IntPtr GetProcAddress(IntPtr lib, String funcName);

        [DllImport("kernel32.dll")]
        public extern static bool FreeLibrary(IntPtr lib);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
        #endregion

      
        public enum PatBltTypes  
        {
            SRCCOPY = 0x00CC0020,     //将源矩形区域直接拷贝到目标矩形区域
            SRCPAINT = 0x00EE0086,    //通过使用布尔型的OR（或）操作符将源和目标矩形区域的颜色合并
            SRCAND = 0x008800C6,      //通过使用AND（与）操作符来将源和目标矩形区域内的颜色合并
            SRCINVERT = 0x00660046,   //通过使用布尔型的XOR（异或）操作符将源和目标矩形区域的颜色合并
            SRCERASE = 0x00440328,    //通过使用AND（与）操作符将目标矩形区域颜色取反后与源矩形区域的颜色值合并
            NOTSRCCOPY = 0x00330008,  //将源矩形区域颜色取反，于拷贝到目标矩形区域
            NOTSRCERASE = 0x001100A6, //使用布尔类型的OR（或）操作符组合源和目标矩形区域的颜色值，然后将合成的颜色取反
            MERGECOPY = 0x00C000CA,   //表示使用布尔型的AND（与）操作符将源矩形区域的颜色与特定模式组合一起
            MERGEPAINT = 0x00BB0226,  //通过使用布尔型的OR（或）操作符将反向的源矩形区域的颜色与目标矩形区域的颜色合并
            PATCOPY = 0x00F00021,     //将特定的模式拷贝到目标位图上
            PATPAINT = 0x00FB0A09,    //通过使用布尔OR（或）操作符将源矩形区域取反后的颜色值与特定模式的颜色合并。然后使用OR（或）操作符将该操作的结果与目标矩形区域内的颜色合并
            PATINVERT = 0x005A0049,   //通过使用XOR（异或）操作符将源和目标矩形区域内的颜色合并
            DSTINVERT = 0x00550009,   //表示使目标矩形区域颜色取反
            BLACKNESS = 0x00000042,   //表示使用与物理调色板的索引0相关的色彩来填充目标矩形区域，（对缺省的物理调色板而言，该颜色为黑色）。
            WHITENESS = 0x00FF0062    //使用与物理调色板中索引1有关的颜色填充目标矩形区域。（对于缺省物理调色板来说，这个颜色就是白色）。
        }  

        public static void DrawRevFrame(IntPtr hWnd)
        {
            if (!(hWnd == IntPtr.Zero))
            {
                IntPtr windowDC = GetWindowDC(hWnd);
                Rect rECT = default(Rect);
                GetWindowRect(hWnd, ref rECT);
                OffsetRect(ref rECT, -rECT.left, -rECT.top);
                PatBlt(windowDC, rECT.left, rECT.top, rECT.right - rECT.left, 3, PatBltTypes.DSTINVERT);
                PatBlt(windowDC, rECT.left, rECT.bottom - 3, 3, -(rECT.bottom - rECT.top - 6), PatBltTypes.DSTINVERT);
                PatBlt(windowDC, rECT.right - 3, rECT.top + 3, 3, rECT.bottom - rECT.top - 6, PatBltTypes.DSTINVERT);
                PatBlt(windowDC, rECT.right, rECT.bottom - 3, -(rECT.right - rECT.left), 3, PatBltTypes.DSTINVERT);
            }
        }

        /// <summary>
        /// 从bitmap转换成ImageSource
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            //Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }
      
    }
}
