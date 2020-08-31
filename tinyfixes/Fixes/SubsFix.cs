using System;
using System.Text;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Structs;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;

namespace tinyfixes.Fixes
{
    public sealed class SubsFix : Fix
    {
        public override string Name => "tinyfixes | SubsFix";

        private readonly (int Id, string Pattern, string Replace)[] mSubs = {
            (1, "\nI'll reserve a room for", " I'll reserve\na room for"),
            (2, "we've been through", "we've been\nthrough"),
        };

        public SubsFix(ILogger logger, bool enabled) : base(logger, enabled) { }

        protected override void OnApply()
        {
            using var scan = new Scanner(mProc, mProc.MainModule);
            var start = 0;

            foreach (var (Id, Pattern, Replace) in mSubs)
            {
                var pattern = BitConverter.ToString(Encoding.ASCII.GetBytes(Pattern)).Replace("-", " ");
                var resSub = scan.CompiledFindPattern(new CompiledScanPattern(pattern), start);

                if (!resSub.Found)
                {
                    mLogger.Warning($"Pattern #{Id} not found, maybe already patched?");
                    continue;
                }

                start = resSub.Offset;

                mLogger.Info($"Pattern #{Id} found, patching...");

                var buff = Encoding.ASCII.GetBytes(Replace);
                mMem.SafeWriteRaw(mBaseAddr + resSub.Offset, buff);
            }
        }
    }
}
