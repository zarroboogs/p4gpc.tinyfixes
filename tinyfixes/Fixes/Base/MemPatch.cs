using System;
using System.Collections.Generic;
using Reloaded.Memory.Sources;

namespace tinyfixes.Fixes
{
    public class MemPatch
    {
        private readonly IList<(IntPtr Offset, byte[] Patch, byte[] Original)> mPatchedBytes;
        private readonly IMemory mMem;

        public MemPatch(IMemory mem)
        {
            mMem = mem;
            mPatchedBytes = new List<(IntPtr, byte[], byte[])>();
        }

        public void Add(IntPtr offset, byte[] patch)
        {
            mMem.SafeReadRaw(offset, out byte[] original, patch.Length);
            mPatchedBytes.Add((offset, patch, original));
        }

        public void Add(IntPtr offset, string patch)
            => Add(offset, Convert.FromHexString(patch.Replace(" ", "")));

        public void Apply()
        {
            foreach (var patch in mPatchedBytes)
            {
                mMem.SafeWriteRaw(patch.Offset, patch.Patch);
            }
        }

        public void Revert()
        {
            foreach (var patch in mPatchedBytes)
            {
                mMem.SafeWriteRaw(patch.Offset, patch.Original);
            }
        }
    }
}