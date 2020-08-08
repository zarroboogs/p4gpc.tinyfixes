using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Structs;

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

        internal static List<PatternScanResult> FindAllPatterns(Process proc, string pattern, int max = -1)
        {
            using var scanner = new Scanner(proc, proc.MainModule);
            var results = new List<PatternScanResult>();
            var result = new PatternScanResult(-1);
            var scanPattern = new CompiledScanPattern(pattern);

            do
            {
                result = scanner.CompiledFindPattern(scanPattern, result.Offset + 1);
                if (result.Found)
                    results.Add(result);
            }
            while (result.Found && (max < 0 || results.Count < max));

            return results;
        }
    }
}
