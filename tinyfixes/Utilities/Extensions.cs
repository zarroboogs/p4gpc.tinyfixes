using System;
using System.Collections.Generic;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Structs;
using Reloaded.Memory.Sources;

namespace tinyfixes.Utilities
{
    public static class Extensions
    {
        public static IntPtr DeRef(this IMemory mem, IntPtr baseAddr, params int[] offsets)
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
    }
}
