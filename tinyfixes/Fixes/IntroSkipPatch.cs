using Reloaded.Memory.Sigscan;
using Reloaded.Mod.Interfaces;

namespace tinyfixes.Fixes
{
    public sealed class IntroSkipPatch : Fix
    {
        public override string Name => "tinyfixes | IntroSkip";

        public IntroSkipPatch(ILogger logger) : base(logger) { }

        protected override void Init()
        {
            using var scan = new Scanner(sProc, sProc.MainModule);
            var res = scan.CompiledFindPattern("5E 5B 5D C3 B9 01 00 00 00");

            if (!res.Found)
            {
                InitFailed = true;
                return;
            }

            mLogger.Info("Adding patch...");
            mPatch.Add(sBaseAddr + res.Offset + 4, "E9 30");

            InitDone = true;
        }
    }
}