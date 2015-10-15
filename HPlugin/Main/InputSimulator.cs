using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.Main
{
    public class InputSimulator
    {
        #region 结构
        struct HARDWAREINPUT
        {
            public UInt32 Msg;
            public UInt16 ParamL;
            public UInt16 ParamH;
        }
        struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }
        [StructLayout(LayoutKind.Explicit)]
        struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
        }
        struct MOUSEINPUT
        {
            public Int32 X;
            public Int32 Y;
            public Int32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }
        struct KEYBDINPUT
        {
            public UInt16 Vk;
            public UInt16 Scan;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }
        #endregion

        #region 枚举
        public enum KeyboardFlag : uint // UInt32
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }
        public enum MouseFlag : uint
        {
            MOVE = 0x0001,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            XDOWN = 0x0080,
            XUP = 0x0100,
            WHEEL = 0x0800,
            VIRTUALDESK = 0x4000,
            ABSOLUTE = 0x8000,
        }
        public enum InputType : uint // UInt32
        {
            MOUSE = 0,
            KEYBOARD = 1,
            HARDWARE = 2,
        }
        public enum XButton : uint
        {
            XBUTTON1 = 0x0001,
            XBUTTON2 = 0x0002,
        }
        public enum SystemMetric
        {
            SM_CXSCREEN = 0,  // 0x00 屏幕的宽度
            SM_CYSCREEN = 1,  // 0x01 屏幕的高度
            SM_CXVSCROLL = 2,  // 0x02
            SM_CYHSCROLL = 3,  // 0x03
            SM_CYCAPTION = 4,  // 0x04
            SM_CXBORDER = 5,  // 0x05
            SM_CYBORDER = 6,  // 0x06
            SM_CXDLGFRAME = 7,  // 0x07
            SM_CXFIXEDFRAME = 7,  // 0x07
            SM_CYDLGFRAME = 8,  // 0x08
            SM_CYFIXEDFRAME = 8,  // 0x08
            SM_CYVTHUMB = 9,  // 0x09
            SM_CXHTHUMB = 10, // 0x0A
            SM_CXICON = 11, // 0x0B
            SM_CYICON = 12, // 0x0C
            SM_CXCURSOR = 13, // 0x0D
            SM_CYCURSOR = 14, // 0x0E
            SM_CYMENU = 15, // 0x0F
            SM_CXFULLSCREEN = 16, // 0x10
            SM_CYFULLSCREEN = 17, // 0x11
            SM_CYKANJIWINDOW = 18, // 0x12
            SM_MOUSEPRESENT = 19, // 0x13
            SM_CYVSCROLL = 20, // 0x14
            SM_CXHSCROLL = 21, // 0x15
            SM_DEBUG = 22, // 0x16
            SM_SWAPBUTTON = 23, // 0x17
            SM_CXMIN = 28, // 0x1C
            SM_CYMIN = 29, // 0x1D
            SM_CXSIZE = 30, // 0x1E
            SM_CYSIZE = 31, // 0x1F
            SM_CXSIZEFRAME = 32, // 0x20
            SM_CXFRAME = 32, // 0x20
            SM_CYSIZEFRAME = 33, // 0x21
            SM_CYFRAME = 33, // 0x21
            SM_CXMINTRACK = 34, // 0x22
            SM_CYMINTRACK = 35, // 0x23
            SM_CXDOUBLECLK = 36, // 0x24
            SM_CYDOUBLECLK = 37, // 0x25
            SM_CXICONSPACING = 38, // 0x26
            SM_CYICONSPACING = 39, // 0x27
            SM_MENUDROPALIGNMENT = 40, // 0x28
            SM_PENWINDOWS = 41, // 0x29
            SM_DBCSENABLED = 42, // 0x2A
            SM_CMOUSEBUTTONS = 43, // 0x2B
            SM_SECURE = 44, // 0x2C
            SM_CXEDGE = 45, // 0x2D
            SM_CYEDGE = 46, // 0x2E
            SM_CXMINSPACING = 47, // 0x2F
            SM_CYMINSPACING = 48, // 0x30
            SM_CXSMICON = 49, // 0x31
            SM_CYSMICON = 50, // 0x32
            SM_CYSMCAPTION = 51, // 0x33
            SM_CXSMSIZE = 52, // 0x34
            SM_CYSMSIZE = 53, // 0x35
            SM_CXMENUSIZE = 54, // 0x36
            SM_CYMENUSIZE = 55, // 0x37
            SM_ARRANGE = 56, // 0x38
            SM_CXMINIMIZED = 57, // 0x39
            SM_CYMINIMIZED = 58, // 0x3A
            SM_CXMAXTRACK = 59, // 0x3B
            SM_CYMAXTRACK = 60, // 0x3C
            SM_CXMAXIMIZED = 61, // 0x3D
            SM_CYMAXIMIZED = 62, // 0x3E
            SM_NETWORK = 63, // 0x3F
            SM_CLEANBOOT = 67, // 0x43
            SM_CXDRAG = 68, // 0x44
            SM_CYDRAG = 69, // 0x45
            SM_SHOWSOUNDS = 70, // 0x46
            SM_CXMENUCHECK = 71, // 0x47
            SM_CYMENUCHECK = 72, // 0x48
            SM_SLOWMACHINE = 73, // 0x49
            SM_MIDEASTENABLED = 74, // 0x4A
            SM_MOUSEWHEELPRESENT = 75, // 0x4B
            SM_XVIRTUALSCREEN = 76, // 0x4C
            SM_YVIRTUALSCREEN = 77, // 0x4D
            SM_CXVIRTUALSCREEN = 78, // 0x4E
            SM_CYVIRTUALSCREEN = 79, // 0x4F
            SM_CMONITORS = 80, // 0x50
            SM_SAMEDISPLAYFORMAT = 81, // 0x51
            SM_IMMENABLED = 82, // 0x52
            SM_CXFOCUSBORDER = 83, // 0x53
            SM_CYFOCUSBORDER = 84, // 0x54
            SM_TABLETPC = 86, // 0x56
            SM_MEDIACENTER = 87, // 0x57
            SM_STARTER = 88, // 0x58
            SM_SERVERR2 = 89, // 0x59
            SM_MOUSEHORIZONTALWHEELPRESENT = 91, // 0x5B
            SM_CXPADDEDBORDER = 92, // 0x5C
            SM_DIGITIZER = 94, // 0x5E
            SM_MAXIMUMTOUCHES = 95, // 0x5F

            SM_REMOTESESSION = 0x1000, // 0x1000
            SM_SHUTTINGDOWN = 0x2000, // 0x2000
            SM_REMOTECONTROL = 0x2001, // 0x2001


            SM_CONVERTABLESLATEMODE = 0x2003,
            SM_SYSTEMDOCKED = 0x2004,
        }

        #endregion

        #region 引用
        [DllImport("user32.dll", SetLastError = true)]
         static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);

        [DllImport("user32.dll", SetLastError = true)]
         static extern Int16 GetAsyncKeyState(UInt16 virtualKeyCode);

        [DllImport("user32.dll", SetLastError = true)]
        static extern Int16 GetKeyState(UInt16 virtualKeyCode);

        [DllImport("user32.dll")]
        static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);
        #endregion

        public static bool IsKeyDownAsync(VirtualKeyCode keyCode)
        {
            Int16 result = GetAsyncKeyState((UInt16)keyCode);
            return (result < 0);
        }

       
        public static bool IsKeyDown(VirtualKeyCode keyCode)
        {
            Int16 result = GetKeyState((UInt16)keyCode);
            return (result < 0);
        }


        public static bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            Int16 result = GetKeyState((UInt16)keyCode);
            return (result & 0x01) == 0x01;
        }

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key DOWN.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to press</param>
        public static void SimulateKeyDown(VirtualKeyCode keyCode)
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.KEYBOARD;
            down.Data.Keyboard = new KEYBDINPUT();
            down.Data.Keyboard.Vk = (UInt16)keyCode;
            down.Data.Keyboard.Scan = 0;
            down.Data.Keyboard.Flags = 0;
            down.Data.Keyboard.Time = 0;
            down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = down;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
        }

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key UP.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to lift up</param>
        public static void SimulateKeyUp(VirtualKeyCode keyCode)
        {
            var up = new INPUT();
            up.Type = (UInt32)InputType.KEYBOARD;
            up.Data.Keyboard = new KEYBDINPUT();
            up.Data.Keyboard.Vk = (UInt16)keyCode;
            up.Data.Keyboard.Scan = 0;
            up.Data.Keyboard.Flags = (UInt32)KeyboardFlag.KEYUP;
            up.Data.Keyboard.Time = 0;
            up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a KeyDown and KeyUp message in the same input sequence in order to simulate a Key PRESS.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to press</param>
        public static void SimulateKeyPress(VirtualKeyCode keyCode)
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.KEYBOARD;
            down.Data.Keyboard = new KEYBDINPUT();
            down.Data.Keyboard.Vk = (UInt16)keyCode;
            down.Data.Keyboard.Scan = 0;
            down.Data.Keyboard.Flags = 0;//为0时表示按下某键
            down.Data.Keyboard.Time = 0;
            down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            var up = new INPUT();
            up.Type = (UInt32)InputType.KEYBOARD;
            up.Data.Keyboard = new KEYBDINPUT();
            up.Data.Keyboard.Vk = (UInt16)keyCode;
            up.Data.Keyboard.Scan = 0;
            up.Data.Keyboard.Flags = (UInt32)KeyboardFlag.KEYUP;
            up.Data.Keyboard.Time = 0;
            up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[2];
            inputList[0] = down;
            inputList[1] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(2, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key press simulation for {0} was not successful.", keyCode));
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
        /// </summary>
        /// <param name="text">The text to be simulated.</param>
        public static void SimulateTextEntry(string text)
        {
            if (text.Length > UInt32.MaxValue / 2) throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", UInt32.MaxValue / 2), "text");

            var chars = UTF8Encoding.ASCII.GetBytes(text);
            var len = chars.Length;
            INPUT[] inputList = new INPUT[len * 2];
            for (int x = 0; x < len; x++)
            {
                UInt16 scanCode = chars[x];

                var down = new INPUT();
                down.Type = (UInt32)InputType.KEYBOARD;
                down.Data.Keyboard = new KEYBDINPUT();
                down.Data.Keyboard.Vk = 0;
                down.Data.Keyboard.Scan = scanCode;
                down.Data.Keyboard.Flags = (UInt32)KeyboardFlag.UNICODE;
                down.Data.Keyboard.Time = 0;
                down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

                var up = new INPUT();
                up.Type = (UInt32)InputType.KEYBOARD;
                up.Data.Keyboard = new KEYBDINPUT();
                up.Data.Keyboard.Vk = 0;
                up.Data.Keyboard.Scan = scanCode;
                up.Data.Keyboard.Flags = (UInt32)(KeyboardFlag.KEYUP | KeyboardFlag.UNICODE);
                up.Data.Keyboard.Time = 0;
                up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

                // Handle extended keys:
                // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
                // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
                if ((scanCode & 0xFF00) == 0xE000)
                {
                    down.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
                    up.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
                }

                inputList[2 * x] = down;
                inputList[2 * x + 1] = up;

            }

            var numberOfSuccessfulSimulatedInputs = SendInput((UInt32)len * 2, inputList, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// Performs a simple modified keystroke like CTRL-C where CTRL is the modifierKey and C is the key.
        /// The flow is Modifier KEYDOWN, Key PRESS, Modifier KEYUP.
        /// </summary>
        /// <param name="modifierKeyCode">The modifier key</param>
        /// <param name="keyCode">The key to simulate</param>
        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            SimulateKeyDown(modifierKeyCode);
            SimulateKeyPress(keyCode);
            SimulateKeyUp(modifierKeyCode);
        }

        /// <summary>
        /// Performs a modified keystroke where there are multiple modifiers and one key like CTRL-ALT-C where CTRL and ALT are the modifierKeys and C is the key.
        /// The flow is Modifiers KEYDOWN in order, Key PRESS, Modifiers KEYUP in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCode">The key to simulate</param>
        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(x => SimulateKeyDown(x));
            SimulateKeyPress(keyCode);
            if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(x => SimulateKeyUp(x));
        }

        /// <summary>
        /// Performs a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the modifierKey and K and C are the keys.
        /// The flow is Modifier KEYDOWN, Keys PRESS in order, Modifier KEYUP.
        /// </summary>
        /// <param name="modifierKey">The modifier key</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            SimulateKeyDown(modifierKey);
            if (keyCodes != null) keyCodes.ToList().ForEach(x => SimulateKeyPress(x));
            SimulateKeyUp(modifierKey);
        }

        /// <summary>
        /// Performs a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL and ALT are the modifierKeys and K and C are the keys.
        /// The flow is Modifiers KEYDOWN in order, Keys PRESS in order, Modifiers KEYUP in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(x => SimulateKeyDown(x));
            if (keyCodes != null) keyCodes.ToList().ForEach(x => SimulateKeyPress(x));
            if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(x => SimulateKeyUp(x));
        }

        public static bool SimulateLeftClick()
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.MOUSE;
            down.Data.Mouse = new MOUSEINPUT();
            down.Data.Mouse.X = 0;
            down.Data.Mouse.Y = 0;
            down.Data.Mouse.MouseData = 0;
            down.Data.Mouse.Flags = (UInt32)MouseFlag.LEFTDOWN;
            down.Data.Mouse.Time = 0;
            down.Data.Mouse.ExtraInfo = IntPtr.Zero;

            var up = new INPUT();
            up.Type = (UInt32)InputType.MOUSE;
            up.Data.Mouse = new MOUSEINPUT();
            up.Data.Mouse.X = 0;
            up.Data.Mouse.Y = 0;
            up.Data.Mouse.MouseData = 0;
            up.Data.Mouse.Flags = (UInt32)MouseFlag.LEFTUP;
            up.Data.Mouse.Time = 0;
            up.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[2];
            inputList[0] = down;
            inputList[1] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(2, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
        public static bool SimulateRightClick()
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.MOUSE;
            down.Data.Mouse = new MOUSEINPUT();
            down.Data.Mouse.X = 0;
            down.Data.Mouse.Y = 0;
            down.Data.Mouse.MouseData = 0;
            down.Data.Mouse.Flags = (UInt32)MouseFlag.RIGHTDOWN;
            down.Data.Mouse.Time = 0;
            down.Data.Mouse.ExtraInfo = IntPtr.Zero;

            var up = new INPUT();
            up.Type = (UInt32)InputType.MOUSE;
            up.Data.Mouse = new MOUSEINPUT();
            up.Data.Mouse.X = 0;
            up.Data.Mouse.Y = 0;
            up.Data.Mouse.MouseData = 0;
            up.Data.Mouse.Flags = (UInt32)MouseFlag.RIGHTUP;
            up.Data.Mouse.Time = 0;
            up.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[2];
            inputList[0] = down;
            inputList[1] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(2, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
        public static bool SimulateMiddleClick()
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.MOUSE;
            down.Data.Mouse = new MOUSEINPUT();
            down.Data.Mouse.X = 0;
            down.Data.Mouse.Y = 0;
            down.Data.Mouse.MouseData = 0;
            down.Data.Mouse.Flags = (UInt32)MouseFlag.MIDDLEDOWN;
            down.Data.Mouse.Time = 0;
            down.Data.Mouse.ExtraInfo = IntPtr.Zero;

            var up = new INPUT();
            up.Type = (UInt32)InputType.MOUSE;
            up.Data.Mouse = new MOUSEINPUT();
            up.Data.Mouse.X = 0;
            up.Data.Mouse.Y = 0;
            up.Data.Mouse.MouseData = 0;
            up.Data.Mouse.Flags = (UInt32)MouseFlag.MIDDLEUP;
            up.Data.Mouse.Time = 0;
            up.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[2];
            inputList[0] = down;
            inputList[1] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(2, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
        public static bool SimulateLeftDown()
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.MOUSE;
            down.Data.Mouse = new MOUSEINPUT();
            down.Data.Mouse.X = 0;
            down.Data.Mouse.Y = 0;
            down.Data.Mouse.MouseData = 0;
            down.Data.Mouse.Flags = (UInt32)MouseFlag.LEFTDOWN;
            down.Data.Mouse.Time = 0;
            down.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = down;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
        public static bool SimulateLeftUp()
        {
            var up = new INPUT();
            up.Type = (UInt32)InputType.MOUSE;
            up.Data.Mouse = new MOUSEINPUT();
            up.Data.Mouse.X = 0;
            up.Data.Mouse.Y = 0;
            up.Data.Mouse.MouseData = 0;
            up.Data.Mouse.Flags = (UInt32)MouseFlag.LEFTUP;
            up.Data.Mouse.Time = 0;
            up.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }

        public static bool SimulateRightDown()
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.MOUSE;
            down.Data.Mouse = new MOUSEINPUT();
            down.Data.Mouse.X = 0;
            down.Data.Mouse.Y = 0;
            down.Data.Mouse.MouseData = 0;
            down.Data.Mouse.Flags = (UInt32)MouseFlag.RIGHTDOWN;
            down.Data.Mouse.Time = 0;
            down.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = down;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
        public static bool SimulateRightUp()
        {
            var up = new INPUT();
            up.Type = (UInt32)InputType.MOUSE;
            up.Data.Mouse = new MOUSEINPUT();
            up.Data.Mouse.X = 0;
            up.Data.Mouse.Y = 0;
            up.Data.Mouse.MouseData = 0;
            up.Data.Mouse.Flags = (UInt32)MouseFlag.RIGHTUP;
            up.Data.Mouse.Time = 0;
            up.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }

        public static bool SimulateMiddleDown()
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.MOUSE;
            down.Data.Mouse = new MOUSEINPUT();
            down.Data.Mouse.X = 0;
            down.Data.Mouse.Y = 0;
            down.Data.Mouse.MouseData = 0;
            down.Data.Mouse.Flags = (UInt32)MouseFlag.MIDDLEDOWN;
            down.Data.Mouse.Time = 0;
            down.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = down;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
        public static bool SimulateMiddleUp()
        {
            var up = new INPUT();
            up.Type = (UInt32)InputType.MOUSE;
            up.Data.Mouse = new MOUSEINPUT();
            up.Data.Mouse.X = 0;
            up.Data.Mouse.Y = 0;
            up.Data.Mouse.MouseData = 0;
            up.Data.Mouse.Flags = (UInt32)MouseFlag.MIDDLEUP;
            up.Data.Mouse.Time = 0;
            up.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }

        public static bool SimulateMoveTo(int x,int y)
        {
            int width = GetSystemMetrics(SystemMetric.SM_CXSCREEN);
            int hight=GetSystemMetrics(SystemMetric.SM_CYSCREEN);
            var m = new INPUT();
            m.Type = (UInt32)InputType.MOUSE;
            m.Data.Mouse = new MOUSEINPUT();
            m.Data.Mouse.X = (65536/(width))*x;
            m.Data.Mouse.Y = (65536/(hight))*y;
            m.Data.Mouse.MouseData = 0;
            m.Data.Mouse.Flags = (UInt32)(MouseFlag.ABSOLUTE|MouseFlag.MOVE);
            m.Data.Mouse.Time = 0;
            m.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = m;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
        /// <summary>
        /// 滚动鼠标的滚轮
        /// </summary>
        /// <param name="x">当x小于0时向下滚动，当x大于0时向上滚动，x的绝对值一般设为120</param>
        /// <returns></returns>
        public static bool SimulateWheel(int x)
        {
            var m = new INPUT();
            m.Type = (UInt32)InputType.MOUSE;
            m.Data.Mouse = new MOUSEINPUT();
            m.Data.Mouse.X = 0;
            m.Data.Mouse.Y = 0;
            m.Data.Mouse.MouseData = x;
            m.Data.Mouse.Flags = (UInt32)MouseFlag.WHEEL;
            m.Data.Mouse.Time = 0;
            m.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[1];
            inputList[0] = m;

            var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                return false;
            return true;
        }
    }
}
