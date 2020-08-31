using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;
using tinyfixes.Utilities;

namespace tinyfixes.Fixes
{
    public sealed class TexWrapFix : Fix
    {
        public override string Name => "tinyfixes | TexWrapFix";

        public TexWrapFix(ILogger logger, bool enabled) : base(logger, enabled) { }

        protected override void OnApply()
        {
            // 81 4f 24 00 c0 00 00 -- parse gmo TexWrap -- 1
            // f7 40 24 00 c0 00 00 -- apply gmo TexWrap (draw) -- 2

            var scanner = new Scanner(mProc, mProc.MainModule);
            var resDraw = scanner.FindAllPatterns("f7 40 24 00 c0 00 00", 2);

            if (resDraw.Count == 0)
            {
                mLogger.Warning("Pattern not found, maybe already patched?");
                return;
            }
            else if (resDraw.Count != 2)
            {
                mLogger.Warning("Pattern count mismatch, skipping...");
                return;
            }

            foreach (var res in resDraw)
            {
                mLogger.Info("Pattern found, patching...");
                mMem.SafeWrite<byte>(mBaseAddr + res.Offset + 20, 0x75 );
                mMem.SafeWrite<ushort>(mBaseAddr + res.Offset + 25, 0x9090 );
                mMem.SafeWrite<byte>(mBaseAddr + res.Offset + 32, 0x75 );
            }
        }
    }

}
