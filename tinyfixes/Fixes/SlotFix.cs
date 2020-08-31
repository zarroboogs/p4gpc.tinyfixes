using System;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Structs;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;
using tinyfixes.Utilities;

namespace tinyfixes.Fixes
{
    public sealed class SlotFix : Fix
    {
        public override string Name => "tinyfixes | SlotFix";

        private readonly string[] mLangs = { "jpn", "eng", "kor", "zho" };

        public SlotFix(ILogger logger, bool enabled) : base(logger, enabled) { }

        protected override void OnApply()
        {
            using var scan = new Scanner(mProc, mProc.MainModule);
            var resPush = scan.CompiledFindPattern(new CompiledScanPattern("8d 95 f8 fd ff ff 8b ce 6a 06"), 0);
            var resTable = scan.CompiledFindPattern(new CompiledScanPattern("8b 04 b5 ?? ?? ?? ?? ff 30 57 ff d3"), 0);

            if (!resPush.Found || !resTable.Found)
            {
                mLogger.Warning("Pattern not found, maybe already patched?");
                return;
            }

            mLogger.Info("Pattern found, patching parser...");
            mMem.SafeWrite<byte>(mBaseAddr + resPush.Offset + 0x09, 0x07 );
            mMem.SafeWrite<byte>(mBaseAddr + resPush.Offset + 0x09 + 0x2C, 0x06 );

            mLogger.Info("Patching parser table...");

            for (int i = 0; i < 4; i++)
            {
                mLogger.Info($"Patching {mLangs[i]}...");
                var pTable = mMem.DeRef(mBaseAddr, resTable.Offset + 3, i * 4);

                mMem.SafeRead(pTable + 0x18, out IntPtr pTimesCleared);
                mMem.SafeRead(pTable + 0x1C, out IntPtr pLang);
                mMem.SafeWrite(pTable + 0x18, pLang);
                mMem.SafeWrite(pTable + 0x1C, pTimesCleared);
            }
        }
    }
}
