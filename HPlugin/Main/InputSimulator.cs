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
            public UInt32 MouseData;
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

        public static void SimulateLeftClick()
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
            if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key press simulation for leftclick was not successful."));
        }

    }
}
