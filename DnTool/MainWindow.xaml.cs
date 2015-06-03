using DictTool.View;
using DnTool.ViewModels;
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
using Utilities.Log;

namespace DnTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.image1.Source = ChangeBitmapToImageSource(softwatcher.Properties.Resources.drag);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
        bool IsDragging = false;
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
                // dm.GetMousePointWindow();
                this.CaptureMouse(false);
            }
        }

        private const int WM_NCHITTEST = 0x0084;
        private const int HTTOPLEFT = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEMOVE = 512;
        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_LBUTTONUP:
                    {
                        if (this.IsDragging)
                        {
                            this.IsDragging = false;
                            // dm.GetMousePointWindow();
                            this.CaptureMouse(false);
                            handled = true;
                        }
                    }
                    break;
                case WM_MOUSEMOVE:
                    {
                        if (this.IsDragging)
                        {
                            this.HandleMouseMovements();
                            handled = true;
                        }
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
        }

        private IntPtr _hWndCurrent;
        /// <summary>
        /// 捕获或释放鼠标
        /// </summary>
        /// <param name="captured"></param>
        private void CaptureMouse(bool captured)
        {
            if (captured)
            {
                var hwnd = Win32API.SetCapture(new WindowInteropHelper(this).Handle);
                Logger.Info("设置捕获鼠标,返回" + hwnd.ToString());
                this.Cursor = new Cursor(new MemoryStream(softwatcher.Properties.Resources.eye));
                this.image1.Source = ChangeBitmapToImageSource(softwatcher.Properties.Resources.drag2);
            }
            else
            {
                this.image1.Source = ChangeBitmapToImageSource(softwatcher.Properties.Resources.drag);
                this.Cursor = Cursors.Arrow;

                var ret = Win32API.ReleaseCapture();
                Logger.Info("释放鼠标,返回" + ret.ToString());
                if (this._hWndCurrent != IntPtr.Zero)
                {
                    this.DrawRevFrame(this._hWndCurrent);
                    this._hWndCurrent = IntPtr.Zero;
                }

            }
        }

        public void DrawRevFrame(IntPtr hWnd)
        {
            if (!(hWnd == IntPtr.Zero))
            {
                IntPtr windowDC = Win32API.GetWindowDC(hWnd);
                Win32API.Rect rECT = default(Win32API.Rect);
                Win32API.GetWindowRect(hWnd, ref rECT);
                Win32API.OffsetRect(ref rECT, -rECT.left, -rECT.top);
                Win32API.PatBlt(windowDC, rECT.left, rECT.top, rECT.right - rECT.left, 3, 5570569);
                Win32API.PatBlt(windowDC, rECT.left, rECT.bottom - 3, 3, -(rECT.bottom - rECT.top - 6), 5570569);
                Win32API.PatBlt(windowDC, rECT.right - 3, rECT.top + 3, 3, rECT.bottom - rECT.top - 6, 5570569);
                Win32API.PatBlt(windowDC, rECT.right, rECT.bottom - 3, -(rECT.right - rECT.left), 3, 5570569);
            }
        }
        /// <summary>
        /// Forces a window to refresh, to eliminate our funky highlighted border
        /// </summary>
        /// <param name="hWnd"></param>
        public void Refresh(IntPtr hWnd)
        {
            Win32API.InvalidateRect(hWnd, IntPtr.Zero, true);
            Win32API.UpdateWindow(hWnd);
            Win32API.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, Win32.RDW_FRAME | Win32.RDW_INVALIDATE | Win32.RDW_UPDATENOW | Win32.RDW_ALLCHILDREN);
        }
        /// <summary>
        /// Highlights the specified window just like Spy++
        /// </summary>
        /// <param name="hWnd"></param>
        public void Highlight(IntPtr hWnd)
        {
            const float penWidth = 5;
            Win32API.Rect rc = new Win32API.Rect();
            Win32API.GetWindowRect(hWnd, ref rc);

            IntPtr hDC = Win32API.GetWindowDC(hWnd);
            if (hDC != IntPtr.Zero)
            {
                using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, penWidth))
                {
                    using (Graphics g = Graphics.FromHdc(hDC))
                    {
                        Font font = new Font("Courer New", 9, System.Drawing.FontStyle.Bold);
                        g.DrawRectangle(pen, 0, 0, rc.right - rc.left - (int)penWidth, rc.bottom - rc.top - (int)penWidth);
                        g.DrawString("BIC Tech <SPY>", font, System.Drawing.Brushes.Red, 5, 5);
                    }
                }
            }
            Win32API.ReleaseDC(hWnd, hDC);
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

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
        /// <summary>
        /// Handles all mouse move messages sent to the Spy Window
        /// </summary>
        private void HandleMouseMovements()
        {
            try
            {
                // capture the window under the cursor's position
                IntPtr hWnd = Win32API.WindowFromPoint(System.Windows.Forms.Control.MousePosition);

                // if the window we're over, is not the same as the one before, and we had one before, refresh it
                if (this._hWndCurrent != hWnd)
                {
                    //   Refresh(_hWndCurrent); //erase old window
                    this.DrawRevFrame(this._hWndCurrent);
                    this.DrawRevFrame(hWnd);
                    this._hWndCurrent = hWnd;
                    // TrackerWindow(hWnd);
                   // this.tbHwnd.Text = hWnd.ToInt32().ToString();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
    public class Win32API
    {
        [Serializable]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

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
        public static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, int dwRop);

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
    }
}
