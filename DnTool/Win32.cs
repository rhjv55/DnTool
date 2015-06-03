using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DictTool.View
{
    public class Win32
    {
        public const int ANSI_FIXED_FONT = 11;
        public const int ANSI_VAR_FONT = 12;
        public const int BLACK_BRUSH = 4;
        public const int BLACK_PEN = 7;
        public const int DEFAULT_PALETTE = 15;
        public const int DEVICE_DEFAULT_FONT = 14;
        public const int DKGRAY_BRUSH = 3;
        public const int FALSE = 0;
        public const int GCL_HICON = -14;
        public const int GCL_HICONSM = -34;
        public const int GRAY_BRUSH = 2;
        public const int GWL_EXSTYLE = -20;
        public const int GWL_HWNDPARENT = -8;
        public const int GWL_STYLE = -16;
        public const int HIDE_WINDOW = 0;
        public const int HOLLOW_BRUSH = 5;
        public const int HWND_BOTTOM = 1;
        public const int HWND_NOTOPMOST = -2;
        public const int HWND_TOP = 0;
        public const int HWND_TOPMOST = -1;
        public const int ICON_BIG = 1;
        public const int ICON_SMALL = 0;
        public const int LTGRAY_BRUSH = 1;
        public const uint MAX_PATH = 260;
        public const int NULL_BRUSH = 5;
        public const int NULL_PEN = 8;
        public const int OEM_FIXED_FONT = 10;
        public const int RDW_ALLCHILDREN = 128;
        public const int RDW_ERASE = 4;
        public const int RDW_ERASENOW = 512;
        public const int RDW_FRAME = 1024;
        public const int RDW_INTERNALPAINT = 2;
        public const int RDW_INVALIDATE = 1;
        public const int RDW_NOCHILDREN = 64;
        public const int RDW_NOERASE = 32;
        public const int RDW_NOFRAME = 2048;
        public const int RDW_NOINTERNALPAINT = 16;
        public const int RDW_UPDATENOW = 256;
        public const int RDW_VALIDATE = 8;
        public const int SHOW_FULLSCREEN = 3;
        public const int SHOW_ICONWINDOW = 2;
        public const int SHOW_OPENNOACTIVATE = 4;
        public const int SHOW_OPENWINDOW = 1;
        public const int SMTO_ABORTIFHUNG = 2;
        public const int SW_OTHERUNZOOM = 4;
        public const int SW_OTHERZOOM = 2;
        public const int SW_PARENTCLOSING = 1;
        public const int SW_PARENTOPENING = 3;
        public const int SWP_ASYNCWINDOWPOS = 16384;
        public const int SWP_DEFERERASE = 8192;
        public const int SWP_DRAWFRAME = 32;
        public const int SWP_FRAMECHANGED = 32;
        public const int SWP_HIDEWINDOW = 128;
        public const int SWP_NOACTIVATE = 16;
        public const int SWP_NOCOPYBITS = 256;
        public const int SWP_NOMOVE = 2;
        public const int SWP_NOOWNERZORDER = 512;
        public const int SWP_NOREDRAW = 8;
        public const int SWP_NOREPOSITION = 512;
        public const int SWP_NOSENDCHANGING = 1024;
        public const int SWP_NOSIZE = 1;
        public const int SWP_NOZORDER = 4;
        public const int SWP_SHOWWINDOW = 64;
        public const int SYSTEM_FIXED_FONT = 16;
        public const int SYSTEM_FONT = 13;
        public const int TRUE = 1;
        public const int WHITE_BRUSH = 0;
        public const int WHITE_PEN = 6;
        public const int WM_GETICON = 127;
        public const int WM_QUERYDRAGICON = 55;
        public const int WM_SETICON = 128;

        public Win32(){}

       // public extern static bool BitBlt(IntPtr hdcDst, int xDst, int yDst, int cx, int cy, IntPtr hdcSrc, int xSrc, int ySrc, uint ulRop);
        public extern static bool BringWindowToTop(IntPtr window);
        public extern static int CallNextHookEx(IntPtr hookHandle, int nCode, IntPtr wParam, IntPtr lParam);
        public extern static int CopyFile(string source, string destination, int failIfExists);
        public extern static IntPtr CreateDC(IntPtr lpszDriver, string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);
        public extern static IntPtr DeleteDC(IntPtr hdc);
        public extern static int DeregisterShellHookWindow(IntPtr hWnd);
        public extern static void EnumWindows(Win32.EnumWindowEventHandler callback, int lParam);
        public extern static int GetClassLong(IntPtr hWnd, int index);
        public extern static int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        public extern static bool GetClientRect(IntPtr hwnd, ref Win32.Rect rectangle);
        public extern static IntPtr GetDC(IntPtr hwnd);
        public extern static IntPtr GetDesktopWindow();
        public extern static IntPtr GetForegroundWindow();
        public extern static int GetLastError();
        public extern static IntPtr GetParent(IntPtr hWnd);
        public extern static IntPtr GetStockObject(int nObject);
        public extern static int GetWindow(IntPtr hWnd, int wCmd);
        public extern static IntPtr GetWindowDC(IntPtr hwnd);
        public extern static bool GetWindowInfo(IntPtr hwnd, ref Win32.WindowInfo info);
        public extern static Win32.WindowStyles GetWindowLong(IntPtr hWnd, int index);
        public extern static int GetWindowModuleFileName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        public extern static bool GetWindowPlacement(IntPtr window, ref Win32.WindowPlacement position);
        public extern static bool GetWindowRect(IntPtr hwnd, ref Win32.Rect rectangle);
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        public extern static int GetWindowThreadProcessId(IntPtr hWnd, out int processId);
        public extern static short GlobalAddAtom(string atomName);
        public extern static short GlobalDeleteAtom(short atom);
        public extern static byte HIBYTE(short a);
        public extern static short HIWORD(int a);
        public extern static int InvalidateRect(IntPtr hWnd, IntPtr lpRect, int bErase);
        public extern static bool IsWindowVisible(IntPtr hWnd);
        public extern static byte LOBYTE(short a);
        public extern static int LockWindowUpdate(IntPtr windowHandle);
        public extern static short LOWORD(int a);
        public extern static int MAKELONG(short a, short b);
        public extern static short MAKEWORD(byte a, byte b);
        public extern static string PathCompactPathEx(string source, uint maxChars);
        public extern static int PathCompactPathEx(StringBuilder pszOut, StringBuilder pszSrc, uint cchMax, uint dwFlags);
        public extern static string PathGetArgs(string path);
        public extern static int PostMessage(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);
        public extern static int RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);
        public extern static int RegisterHotKey(IntPtr hWnd, int id, uint modifiers, uint virtualkeyCode);
        public extern static int RegisterShellHookWindow(IntPtr hWnd);
        public extern static int RegisterWindowMessage(string message);
        public extern static int ReleaseCapture();
        public extern static int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        public extern static string SafePathGetArgs(string path);
        public extern static IntPtr SelectObject(IntPtr hDc, IntPtr hObject);
        public extern static int SendMessage(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);
        public extern static int SendMessageTimeout(IntPtr hWnd, int uMsg, int wParam, int lParam, int fuFlags, int uTimeout, out int lpdwResult);
        public extern static IntPtr SetCapture(IntPtr hWnd);
        public extern static int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public extern static IntPtr SetWindowsHookEx(Win32.HookTypes hookType, Win32.HookProc hookProc, IntPtr hInstance, int nThreadId);
        public extern static int ShowWindowAsync(IntPtr hWnd, int command);
        public extern static bool StretchBlt(IntPtr hdcDst, int xDst, int yDst, int cx, int cy, IntPtr hdcSrc, int xSrc, int ySrc, int cxSrc, int cySrc, uint ulRop);
        public extern static void SwitchToThisWindow(IntPtr hWnd, int altTabActivated);
        public extern static int UnhookWindowsHookEx(IntPtr hookHandle);
        public extern static int UnregisterHotKey(IntPtr hWnd, int id);
        public extern static int UpdateWindow(IntPtr hWnd);
        public extern static IntPtr WindowFromPoint(Point pt);

        public enum BinaryRasterOperations
        {
            R2_BLACK = 1,
            R2_NOTMERGEPEN = 2,
            R2_MASKNOTPEN = 3,
            R2_NOTCOPYPEN = 4,
            R2_MASKPENNOT = 5,
            R2_NOT = 6,
            R2_XORPEN = 7,
            R2_NOTMASKPEN = 8,
            R2_MASKPEN = 9,
            R2_NOTXORPEN = 10,
            R2_NOP = 11,
            R2_MERGENOTPEN = 12,
            R2_COPYPEN = 13,
            R2_MERGEPENNOT = 14,
            R2_MERGEPEN = 15,
            R2_WHITE = 16,
            R2_LAST = 16,
        }

        public enum HookTypes
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14,
        }

        [Flags]
        public enum HotkeyModifiers
        {
            MOD_ALT = 1,
            MOD_CONTROL = 2,
            MOD_SHIFT = 4,
            MOD_WIN = 8,
        }

        public enum PeekMessageFlags
        {
            PM_NOREMOVE = 0,
            PM_REMOVE = 1,
            PM_NOYIELD = 2,
        }

        public enum ShellHookMessages
        {
            HSHELL_WINDOWCREATED = 1,
            HSHELL_WINDOWDESTROYED = 2,
            HSHELL_ACTIVATESHELLWINDOW = 3,
            HSHELL_WINDOWACTIVATED = 4,
            HSHELL_GETMINRECT = 5,
            HSHELL_REDRAW = 6,
            HSHELL_TASKMAN = 7,
            HSHELL_LANGUAGE = 8,
            HSHELL_ACCESSIBILITYSTATE = 11,
        }

        public enum ShowWindowCmds
        {
            SW_HIDE = 0,
            SW_NORMAL = 1,
            SW_SHOWNORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_MAXIMIZE = 3,
            SW_SHOWMAXIMIZED = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11,
        }

        public enum TernaryRasterOperations
        {
            BLACKNESS = 66,
            NOTSRCERASE = 1114278,
            NOTSRCCOPY = 3342344,
            SRCERASE = 4457256,
            DSTINVERT = 5570569,
            PATINVERT = 5898313,
            SRCINVERT = 6684742,
            SRCAND = 8913094,
            MERGEPAINT = 12255782,
            MERGECOPY = 12583114,
            SRCCOPY = 13369376,
            SRCPAINT = 15597702,
            PATCOPY = 15728673,
            PATPAINT = 16452105,
            WHITENESS = 16711778,
        }

        public enum WindowMessages
        {
            WM_NULL = 0,
            WM_CREATE = 1,
            WM_DESTROY = 2,
            WM_MOVE = 3,
            WM_SIZE = 5,
            WM_ACTIVATE = 6,
            WM_SETFOCUS = 7,
            WM_KILLFOCUS = 8,
            WM_ENABLE = 10,
            WM_SETREDRAW = 11,
            WM_SETTEXT = 12,
            WM_GETTEXT = 13,
            WM_GETTEXTLENGTH = 14,
            WM_PAINT = 15,
            WM_CLOSE = 16,
            WM_QUERYENDSESSION = 17,
            WM_QUIT = 18,
            WM_QUERYOPEN = 19,
            WM_ERASEBKGND = 20,
            WM_SYSCOLORCHANGE = 21,
            WM_ENDSESSION = 22,
            WM_SHOWWINDOW = 24,
            WM_CTLCOLOR = 25,
            WM_WININICHANGE = 26,
            WM_SETTINGCHANGE = 26,
            WM_DEVMODECHANGE = 27,
            WM_ACTIVATEAPP = 28,
            WM_FONTCHANGE = 29,
            WM_TIMECHANGE = 30,
            WM_CANCELMODE = 31,
            WM_SETCURSOR = 32,
            WM_MOUSEACTIVATE = 33,
            WM_CHILDACTIVATE = 34,
            WM_QUEUESYNC = 35,
            WM_GETMINMAXINFO = 36,
            WM_PAINTICON = 38,
            WM_ICONERASEBKGND = 39,
            WM_NEXTDLGCTL = 40,
            WM_SPOOLERSTATUS = 42,
            WM_DRAWITEM = 43,
            WM_MEASUREITEM = 44,
            WM_DELETEITEM = 45,
            WM_VKEYTOITEM = 46,
            WM_CHARTOITEM = 47,
            WM_SETFONT = 48,
            WM_GETFONT = 49,
            WM_SETHOTKEY = 50,
            WM_GETHOTKEY = 51,
            WM_QUERYDRAGICON = 55,
            WM_COMPAREITEM = 57,
            WM_GETOBJECT = 61,
            WM_COMPACTING = 65,
            WM_COMMNOTIFY = 68,
            WM_WINDOWPOSCHANGING = 70,
            WM_WINDOWPOSCHANGED = 71,
            WM_POWER = 72,
            WM_COPYDATA = 74,
            WM_CANCELJOURNAL = 75,
            WM_NOTIFY = 78,
            WM_INPUTLANGCHANGEREQUEST = 80,
            WM_INPUTLANGCHANGE = 81,
            WM_TCARD = 82,
            WM_HELP = 83,
            WM_USERCHANGED = 84,
            WM_NOTIFYFORMAT = 85,
            WM_CONTEXTMENU = 123,
            WM_STYLECHANGING = 124,
            WM_STYLECHANGED = 125,
            WM_DISPLAYCHANGE = 126,
            WM_GETICON = 127,
            WM_SETICON = 128,
            WM_NCCREATE = 129,
            WM_NCDESTROY = 130,
            WM_NCCALCSIZE = 131,
            WM_NCHITTEST = 132,
            WM_NCPAINT = 133,
            WM_NCACTIVATE = 134,
            WM_GETDLGCODE = 135,
            WM_SYNCPAINT = 136,
            WM_NCMOUSEMOVE = 160,
            WM_NCLBUTTONDOWN = 161,
            WM_NCLBUTTONUP = 162,
            WM_NCLBUTTONDBLCLK = 163,
            WM_NCRBUTTONDOWN = 164,
            WM_NCRBUTTONUP = 165,
            WM_NCRBUTTONDBLCLK = 166,
            WM_NCMBUTTONDOWN = 167,
            WM_NCMBUTTONUP = 168,
            WM_NCMBUTTONDBLCLK = 169,
            WM_KEYDOWN = 256,
            WM_KEYUP = 257,
            WM_CHAR = 258,
            WM_DEADCHAR = 259,
            WM_SYSKEYDOWN = 260,
            WM_SYSKEYUP = 261,
            WM_SYSCHAR = 262,
            WM_SYSDEADCHAR = 263,
            WM_KEYLAST = 264,
            WM_IME_STARTCOMPOSITION = 269,
            WM_IME_ENDCOMPOSITION = 270,
            WM_IME_KEYLAST = 271,
            WM_IME_COMPOSITION = 271,
            WM_INITDIALOG = 272,
            WM_COMMAND = 273,
            WM_SYSCOMMAND = 274,
            WM_TIMER = 275,
            WM_HSCROLL = 276,
            WM_VSCROLL = 277,
            WM_INITMENU = 278,
            WM_INITMENUPOPUP = 279,
            WM_MENUSELECT = 287,
            WM_MENUCHAR = 288,
            WM_ENTERIDLE = 289,
            WM_MENURBUTTONUP = 290,
            WM_MENUDRAG = 291,
            WM_MENUGETOBJECT = 292,
            WM_UNINITMENUPOPUP = 293,
            WM_MENUCOMMAND = 294,
            WM_CTLCOLORMSGBOX = 306,
            WM_CTLCOLOREDIT = 307,
            WM_CTLCOLORLISTBOX = 308,
            WM_CTLCOLORBTN = 309,
            WM_CTLCOLORDLG = 310,
            WM_CTLCOLORSCROLLBAR = 311,
            WM_CTLCOLORSTATIC = 312,
            WM_MOUSEMOVE = 512,
            WM_LBUTTONDOWN = 513,
            WM_LBUTTONUP = 514,
            WM_LBUTTONDBLCLK = 515,
            WM_RBUTTONDOWN = 516,
            WM_RBUTTONUP = 517,
            WM_RBUTTONDBLCLK = 518,
            WM_MBUTTONDOWN = 519,
            WM_MBUTTONUP = 520,
            WM_MBUTTONDBLCLK = 521,
            WM_MOUSEWHEEL = 522,
            WM_PARENTNOTIFY = 528,
            WM_ENTERMENULOOP = 529,
            WM_EXITMENULOOP = 530,
            WM_NEXTMENU = 531,
            WM_SIZING = 532,
            WM_CAPTURECHANGED = 533,
            WM_MOVING = 534,
            WM_DEVICECHANGE = 537,
            WM_MDICREATE = 544,
            WM_MDIDESTROY = 545,
            WM_MDIACTIVATE = 546,
            WM_MDIRESTORE = 547,
            WM_MDINEXT = 548,
            WM_MDIMAXIMIZE = 549,
            WM_MDITILE = 550,
            WM_MDICASCADE = 551,
            WM_MDIICONARRANGE = 552,
            WM_MDIGETACTIVE = 553,
            WM_MDISETMENU = 560,
            WM_ENTERSIZEMOVE = 561,
            WM_EXITSIZEMOVE = 562,
            WM_DROPFILES = 563,
            WM_MDIREFRESHMENU = 564,
            WM_IME_SETCONTEXT = 641,
            WM_IME_NOTIFY = 642,
            WM_IME_CONTROL = 643,
            WM_IME_COMPOSITIONFULL = 644,
            WM_IME_SELECT = 645,
            WM_IME_CHAR = 646,
            WM_IME_REQUEST = 648,
            WM_IME_KEYDOWN = 656,
            WM_IME_KEYUP = 657,
            WM_MOUSEHOVER = 673,
            WM_MOUSELEAVE = 675,
            WM_CUT = 768,
            WM_COPY = 769,
            WM_PASTE = 770,
            WM_CLEAR = 771,
            WM_UNDO = 772,
            WM_RENDERFORMAT = 773,
            WM_RENDERALLFORMATS = 774,
            WM_DESTROYCLIPBOARD = 775,
            WM_DRAWCLIPBOARD = 776,
            WM_PAINTCLIPBOARD = 777,
            WM_VSCROLLCLIPBOARD = 778,
            WM_SIZECLIPBOARD = 779,
            WM_ASKCBFORMATNAME = 780,
            WM_CHANGECBCHAIN = 781,
            WM_HSCROLLCLIPBOARD = 782,
            WM_QUERYNEWPALETTE = 783,
            WM_PALETTEISCHANGING = 784,
            WM_PALETTECHANGED = 785,
            WM_HOTKEY = 786,
            WM_PRINT = 791,
            WM_PRINTCLIENT = 792,
            WM_HANDHELDFIRST = 856,
            WM_HANDHELDLAST = 863,
            WM_AFXFIRST = 864,
            WM_AFXLAST = 895,
            WM_PENWINFIRST = 896,
            WM_PENWINLAST = 911,
            WM_USER = 1024,
            WM_REFLECT = 8192,
            WM_APP = 32768,
        }

        public enum WindowStyles
        {
            WS_TILED = 0,
            WS_OVERLAPPED = 0,
            WS_EX_DLGMODALFRAME = 1,
            WS_EX_NOPARENTNOTIFY = 4,
            WS_EX_TOPMOST = 8,
            WS_EX_ACCEPTFILES = 16,
            WS_EX_TRANSPARENT = 32,
            WS_EX_TOOLWINDOW = 128,
            WS_MAXIMIZEBOX = 65536,
            WS_TABSTOP = 65536,
            WS_GROUP = 131072,
            WS_MINIMIZEBOX = 131072,
            WS_THICKFRAME = 262144,
            WS_SIZEBOX = 262144,
            WS_EX_APPWINDOW = 262144,
            WS_SYSMENU = 524288,
            WS_HSCROLL = 1048576,
            WS_VSCROLL = 2097152,
            WS_DLGFRAME = 4194304,
            WS_BORDER = 8388608,
            WS_CAPTION = 12582912,
            WS_OVERLAPPEDWINDOW = 13565952,
            WS_MAXIMIZE = 16777216,
            WS_CLIPCHILDREN = 33554432,
            WS_CLIPSIBLINGS = 67108864,
            WS_DISABLED = 134217728,
            WS_VISIBLE = 268435456,
            WS_ICONIC = 536870912,
            WS_MINIMIZE = 536870912,
            WS_CHILD = 1073741824,
            WS_CHILDWINDOW = 1073741824,
            //WS_POPUP = (int)2147483648,
            //WS_POPUPWINDOW = (int)2156396544,
        }

        public delegate bool EnumWindowEventHandler(IntPtr hWnd, int lParam);

        public delegate void HookEventHandler(object sender, Win32.HookEventArgs e);

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public class HookEventArgs : EventArgs
        {
            public HookEventArgs(int code, IntPtr wParam, IntPtr lParam)
            {
            }

            public int Code { get; set; }
            public IntPtr lParam { get; set; }
            public IntPtr wParam { get; set; }
        }

        public struct MSLLHOOKSTRUCT
        {
            public int ExtraInfo;
            public int Flags;
            public int MouseData;
            public Point Point;
            public int Time;
        }

        public struct Rect
        {
            public int bottom;
            public int left;
            public int right;
            public int top;

            public int Height { get; set; }
            public int Width { get; set; }
        }

        public struct WindowInfo
        {
            public short atomWindowtype;
            public Rectangle client;
            public short creatorVersion;
            public int exStyle;
            public int size;
            public int style;
            public Rectangle window;
            public int windowStatus;
            public uint xWindowBorders;
            public uint yWindowBorders;
        }

        public struct WindowPlacement
        {
            public int flags;
            public int length;
            public Point maxPosition;
            public Point minPosition;
            public Rectangle normalPosition;
            public int showCmd;
        }
    }
}
