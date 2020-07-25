using System;
using System.Runtime.InteropServices;

namespace tinyfixes
{
    internal static class Util
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            Int32 nSize,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualProtectEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            UIntPtr dwSize,
            uint flNewProtect,
            out uint lpflOldProtect);

        internal static IntPtr DeRef(IntPtr hProc, IntPtr ptr, int[] offsets)
        {
            var buff = new byte[IntPtr.Size];

            foreach (int off in offsets)
            {
                ReadProcessMemory(hProc, ptr, buff, buff.Length, out var read);

                ptr = (IntPtr.Size == 4)
                    ? IntPtr.Add(new IntPtr(BitConverter.ToInt32(buff, 0)), off)
                    : IntPtr.Add(new IntPtr(BitConverter.ToInt64(buff, 0)), off);
            }

            return ptr;
        }
    }
}
