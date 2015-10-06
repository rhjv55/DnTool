using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


using System.Runtime.InteropServices;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ComponentModel;
using DnTool.Utilities.API;

namespace DnTool
{
    /// <summary>
    /// 遍历句柄模块
    /// 作者：清风抚断云
    /// QQ：274838061
    /// </summary>
    public class MutexCloseHelper
    {

        [DllImport("ntdll.dll")]
        public static extern int NtQueryObject(IntPtr ObjectHandle, int
            ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength,
            ref int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        [DllImport("ntdll.dll")]
        public static extern uint NtQuerySystemInformation(int
            SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength,
            ref int returnLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr OpenMutex(UInt32 desiredAccess, bool inheritHandle, string name);


        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(Win32API.ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle,
           ushort hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
           uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        public enum ObjectInformationClass : int
        {
            ObjectBasicInformation = 0,
            ObjectNameInformation = 1,
            ObjectTypeInformation = 2,
            ObjectAllTypesInformation = 3,
            ObjectHandleInformation = 4
        }

       

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_BASIC_INFORMATION
        { // Information Class 0
            public int Attributes;
            public int GrantedAccess;
            public int HandleCount;
            public int PointerCount;
            public int PagedPoolUsage;
            public int NonPagedPoolUsage;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int NameInformationLength;
            public int TypeInformationLength;
            public int SecurityDescriptorLength;
            public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_TYPE_INFORMATION
        { // Information Class 2
            public UNICODE_STRING Name;
            public int ObjectCount;
            public int HandleCount;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int Reserved4;
            public int PeakObjectCount;
            public int PeakHandleCount;
            public int Reserved5;
            public int Reserved6;
            public int Reserved7;
            public int Reserved8;
            public int InvalidAttributes;
            public GENERIC_MAPPING GenericMapping;
            public int ValidAccess;
            public byte Unknown;
            public byte MaintainHandleDatabase;
            public int PoolType;
            public int PagedPoolUsage;
            public int NonPagedPoolUsage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_NAME_INFORMATION
        { // Information Class 1
            public UNICODE_STRING Name;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UNICODE_STRING
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GENERIC_MAPPING
        {
            public int GenericRead;
            public int GenericWrite;
            public int GenericExecute;
            public int GenericAll;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SYSTEM_HANDLE_INFORMATION
        { // Information Class 16
            public int ProcessID;
            public byte ObjectTypeNumber;
            public byte Flags; // 0x01 = PROTECT_FROM_CLOSE, 0x02 = INHERIT
            public ushort Handle;
            public int Object_Pointer;
            public UInt32 GrantedAccess;
        }

        public const int MAX_PATH = 260;
        public const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
        public const int DUPLICATE_SAME_ACCESS = 0x2;
        public const int DUPLICATE_CLOSE_SOURCE = 0x1;
    }
    public class Win32Processes
    {

        const int CNST_SYSTEM_HANDLE_INFORMATION = 16;
        const uint STATUS_INFO_LENGTH_MISMATCH = 0xc0000004;

        public static string getObjectTypeName(MutexCloseHelper.SYSTEM_HANDLE_INFORMATION shHandle, Process process, out IntPtr ipHandle)
        {

            IntPtr m_ipProcessHwnd = MutexCloseHelper.OpenProcess(Win32API.ProcessAccessFlags.All, false, process.Id);
            ipHandle = IntPtr.Zero;
            var objBasic = new MutexCloseHelper.OBJECT_BASIC_INFORMATION();
            IntPtr ipBasic = IntPtr.Zero;
            var objObjectType = new MutexCloseHelper.OBJECT_TYPE_INFORMATION();
            IntPtr ipObjectType = IntPtr.Zero;
            IntPtr ipObjectName = IntPtr.Zero;
            string strObjectTypeName = "";
            int nLength = 0;
            int nReturn = 0;
            IntPtr ipTemp = IntPtr.Zero;

            if (!MutexCloseHelper.DuplicateHandle(m_ipProcessHwnd, shHandle.Handle, MutexCloseHelper.GetCurrentProcess(), out ipHandle, 0, false, MutexCloseHelper.DUPLICATE_SAME_ACCESS))
                return null;

            ipBasic = Marshal.AllocHGlobal(Marshal.SizeOf(objBasic));
            MutexCloseHelper.NtQueryObject(ipHandle, (int)MutexCloseHelper.ObjectInformationClass.ObjectBasicInformation, ipBasic, Marshal.SizeOf(objBasic), ref nLength);
            objBasic = (MutexCloseHelper.OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(ipBasic, objBasic.GetType());
            Marshal.FreeHGlobal(ipBasic);


            ipObjectType = Marshal.AllocHGlobal(objBasic.TypeInformationLength);
            nLength = objBasic.TypeInformationLength;
            while ((uint)(nReturn = MutexCloseHelper.NtQueryObject(ipHandle, (int)MutexCloseHelper.ObjectInformationClass.ObjectTypeInformation, ipObjectType, nLength, ref nLength)) == MutexCloseHelper.STATUS_INFO_LENGTH_MISMATCH)
            {
                Marshal.FreeHGlobal(ipObjectType);
                ipObjectType = Marshal.AllocHGlobal(nLength);
            }
            //Win32API.CloseHandle(ipHandle);
            objObjectType = (MutexCloseHelper.OBJECT_TYPE_INFORMATION)Marshal.PtrToStructure(ipObjectType, objObjectType.GetType());
            if (Is64Bits())
            {
                ipTemp = new IntPtr(Convert.ToInt64(objObjectType.Name.Buffer.ToString(), 10) >> 32);
            }
            else
            {
                ipTemp = objObjectType.Name.Buffer;
            }

            strObjectTypeName = Marshal.PtrToStringUni(ipTemp, objObjectType.Name.Length >> 1);
            Marshal.FreeHGlobal(ipObjectType);
            return strObjectTypeName;
        }


        public static string getObjectName(MutexCloseHelper.SYSTEM_HANDLE_INFORMATION shHandle, Process process, out IntPtr ipHandle)
        {

            IntPtr m_ipProcessHwnd = MutexCloseHelper.OpenProcess(Win32API.ProcessAccessFlags.All, false, process.Id);
            ipHandle = IntPtr.Zero;
            var objBasic = new MutexCloseHelper.OBJECT_BASIC_INFORMATION();
            IntPtr ipBasic = IntPtr.Zero;
            IntPtr ipObjectType = IntPtr.Zero;
            var objObjectName = new MutexCloseHelper.OBJECT_NAME_INFORMATION();
            IntPtr ipObjectName = IntPtr.Zero;
            string strObjectName = "";
            int nLength = 0;
            int nReturn = 0;
            IntPtr ipTemp = IntPtr.Zero;

            if (!MutexCloseHelper.DuplicateHandle(m_ipProcessHwnd, shHandle.Handle, MutexCloseHelper.GetCurrentProcess(), out ipHandle, 0, false, MutexCloseHelper.DUPLICATE_SAME_ACCESS))
                return null;

            ipBasic = Marshal.AllocHGlobal(Marshal.SizeOf(objBasic));
            MutexCloseHelper.NtQueryObject(ipHandle, (int)MutexCloseHelper.ObjectInformationClass.ObjectBasicInformation, ipBasic, Marshal.SizeOf(objBasic), ref nLength);
            objBasic = (MutexCloseHelper.OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(ipBasic, objBasic.GetType());
            Marshal.FreeHGlobal(ipBasic);


            nLength = objBasic.NameInformationLength;

            ipObjectName = Marshal.AllocHGlobal(nLength);
            while ((uint)(nReturn = MutexCloseHelper.NtQueryObject(ipHandle, (int)MutexCloseHelper.ObjectInformationClass.ObjectNameInformation, ipObjectName, nLength, ref nLength)) == MutexCloseHelper.STATUS_INFO_LENGTH_MISMATCH)
            {
                Marshal.FreeHGlobal(ipObjectName);
                ipObjectName = Marshal.AllocHGlobal(nLength);
            }
            
            //Win32API.CloseHandle(ipHandle);
            objObjectName = (MutexCloseHelper.OBJECT_NAME_INFORMATION)Marshal.PtrToStructure(ipObjectName, objObjectName.GetType());

            if (Is64Bits())
            {
                ipTemp = new IntPtr(Convert.ToInt64(objObjectName.Name.Buffer.ToString(), 10) >> 32);
            }
            else
            {
                ipTemp = objObjectName.Name.Buffer;
            }

            if (ipTemp != IntPtr.Zero)
            {

                byte[] baTemp2 = new byte[nLength];
                try
                {
                    Marshal.Copy(ipTemp, baTemp2, 0, nLength);

                    strObjectName = Marshal.PtrToStringUni(Is64Bits() ? new IntPtr(ipTemp.ToInt64()) : new IntPtr(ipTemp.ToInt32()));
                    return strObjectName;
                }
                catch (AccessViolationException)
                {
                    return null;
                }
                finally
                {
                    Marshal.FreeHGlobal(ipObjectName);
                    MutexCloseHelper.CloseHandle(ipHandle);
                }
            }
            return null;

        }



        public static List<MutexCloseHelper.SYSTEM_HANDLE_INFORMATION> GetHandles(Process process = null, string IN_strObjectTypeName = null, string IN_strObjectName = null)
        {
            uint nStatus;
            int nHandleInfoSize = 0x10000;
            IntPtr ipHandlePointer = Marshal.AllocHGlobal(nHandleInfoSize);
            int nLength = 0;
            IntPtr ipHandle = IntPtr.Zero;

            while ((nStatus = MutexCloseHelper.NtQuerySystemInformation(CNST_SYSTEM_HANDLE_INFORMATION, ipHandlePointer, nHandleInfoSize, ref nLength)) == STATUS_INFO_LENGTH_MISMATCH)
            {
                nHandleInfoSize = nLength;
                Marshal.FreeHGlobal(ipHandlePointer);
                ipHandlePointer = Marshal.AllocHGlobal(nLength);
            }

            byte[] baTemp = new byte[nLength];
            Marshal.Copy(ipHandlePointer, baTemp, 0, nLength);

            long lHandleCount = 0;
            if (Is64Bits())
            {
                lHandleCount = Marshal.ReadInt64(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt64() + 8);
            }
            else
            {
                lHandleCount = Marshal.ReadInt32(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt32() + 4);
            }

            MutexCloseHelper.SYSTEM_HANDLE_INFORMATION shHandle;
            List<MutexCloseHelper.SYSTEM_HANDLE_INFORMATION> lstHandles = new List<MutexCloseHelper.SYSTEM_HANDLE_INFORMATION>();

            IntPtr newHandle = IntPtr.Zero;
            for (long lIndex = 0; lIndex < lHandleCount; lIndex++)
            {
                shHandle = new MutexCloseHelper.SYSTEM_HANDLE_INFORMATION();
                if (Is64Bits())
                {
                    shHandle = (MutexCloseHelper.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle) + 8);
                }
                else
                {
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle));
                    shHandle = (MutexCloseHelper.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                }



                if (process != null)
                {
                    if (shHandle.ProcessID != process.Id) continue;
                }


                string strObjectTypeName = "";
                if (IN_strObjectTypeName != null)
                {
                    strObjectTypeName = getObjectTypeName(shHandle, Process.GetProcessById(shHandle.ProcessID), out newHandle);
                    try
                    {
                        if (strObjectTypeName != IN_strObjectTypeName)
                        {

                            continue;
                        }
                        else
                        {
                            MutexCloseHelper.CloseHandle(newHandle);
                        }
                    }
                    catch
                    {

                    }
                   
                }

                string strObjectName = "";
                if (IN_strObjectName != null)
                {
                    strObjectName = getObjectName(shHandle, Process.GetProcessById(shHandle.ProcessID), out newHandle);
                    try
                    {
                        MutexCloseHelper.CloseHandle(newHandle);
                    }
                    catch(Exception ex)
                    {
                       Debug.WriteLine(ex.Message);
                       
                    }
                    if (strObjectName != IN_strObjectName)
                    {
                        continue;
                    }
                }

                //string strObjectTypeName2 = getObjectTypeName(shHandle, Process.GetProcessById(shHandle.ProcessID), out newHandle);
                //try
                //{
                //    Win32API.CloseHandle(newHandle);
                //}
                //catch
                //{

                //}
                //string strObjectName2 = getObjectName(shHandle, Process.GetProcessById(shHandle.ProcessID), out newHandle);
                //try
                //{
                //    Win32API.CloseHandle(newHandle);
                //}
                //catch
                //{

                //}
                //Console.WriteLine("{0}   {1}   {2}", shHandle.ProcessID, strObjectTypeName2, strObjectName2);


                lstHandles.Add(shHandle);

            }
            return lstHandles;

        }

        public static bool Is64Bits()
        {
            return Marshal.SizeOf(typeof(IntPtr)) == 8 ? true : false;
        }


    }
}
