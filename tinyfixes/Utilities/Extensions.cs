using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Structs;
using Reloaded.Memory.Sources;

namespace tinyfixes.Utilities
{
    public static class Extensions
    {
        public static IntPtr Dereference(this IMemory mem, IntPtr baseAddr, params int[] offsets)
        {
            foreach (int off in offsets)
                mem.SafeRead(IntPtr.Add(baseAddr, off), out baseAddr);

            return baseAddr;
        }

        public static List<PatternScanResult> FindAllPatterns(this Scanner scanner, string pattern, int max = -1)
        {
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

        public static PatternScanResult CompiledFindPattern(this Scanner scan, string pattern, int startingIndex = 0)
            => scan.CompiledFindPattern(new CompiledScanPattern(pattern), startingIndex);

        public static void SafeWriteRaw(this IMemory m, IntPtr memoryAddress, string data)
            => m.SafeWriteRaw(memoryAddress, Convert.FromHexString(data.Replace(" ", "")));

        public static string ToHexString(this string s, Encoding enc = null)
        {
            var buffer = (enc ?? Encoding.ASCII).GetBytes(s);
            return BitConverter.ToString(buffer).Replace("-", " ");
        }
    }
}