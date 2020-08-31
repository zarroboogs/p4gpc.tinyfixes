using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Structs;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;

namespace tinyfixes.Fixes
{
    public sealed class IntroFix : Fix
    {
        public override string Name => "tinyfixes | IntroSkip";

        public IntroFix(ILogger logger, bool enabled) : base(logger, enabled) { }

        protected override void OnApply()
        {
            using var scan = new Scanner(mProc, mProc.MainModule);
            var resIntro = scan.CompiledFindPattern(new CompiledScanPattern("5e 5b 5d c3 b9 01 00 00 00"), 0);

            if (!resIntro.Found)
            {
                mLogger.Warning("Pattern not found, maybe already patched?");
                return;
            }

            mLogger.Info("Pattern found, patching...");
            mMem.SafeWrite<ushort>(mBaseAddr + resIntro.Offset + 4, 0x30e9 );
        }
    }
}
