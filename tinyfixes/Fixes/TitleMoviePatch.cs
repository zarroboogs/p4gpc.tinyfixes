using Reloaded.Memory.Sigscan;
using Reloaded.Mod.Interfaces;

namespace tinyfixes.Fixes
{
    public sealed class TitleMoviePatch : Fix
    {
        public override string Name => "tinyfixes | TitleMovie";

        public TitleMoviePatch(ILogger logger) : base(logger) { }

        protected override void Init()
        {
            using var scan = new Scanner(sProc, sProc.MainModule);
            var res = scan.CompiledFindPattern("0F 45 C1 5F 89 06 31 C0 5E 5B 5D C3");

            if (!res.Found)
            {
                InitFailed = true;
                return;
            }

            mLogger.Info("Adding patch...");
            mPatch.Add(sBaseAddr + res.Offset + 0xC, "BA 15 00 00 00");

            InitDone = true;
        }
    }
}