using Reloaded.Memory.Sigscan;
using Reloaded.Mod.Interfaces;
using tinyfixes.Utilities;

namespace tinyfixes.Fixes
{
    public sealed class TexWrapFix : Fix
    {
        public override string Name => "tinyfixes | TexWrapFix";

        public TexWrapFix(ILogger logger) : base(logger) { }

        protected override void Init()
        {
            // 81 4F 24 00 C0 00 00 -- parse gmo TexWrap -- 1
            // F7 40 24 00 C0 00 00 -- apply gmo TexWrap (draw) -- 2

            using var scan = new Scanner(sProc, sProc.MainModule);
            var res = scan.FindAllPatterns("F7 40 24 00 C0 00 00", 2);

            if (res.Count != 2)
            {
                InitFailed = true;
                return;
            }

            foreach (var r in res)
            {
                mLogger.Info("Adding patch...");
                mPatch.Add(sBaseAddr + r.Offset + 20, "75");
                mPatch.Add(sBaseAddr + r.Offset + 25, "90 90");
                mPatch.Add(sBaseAddr + r.Offset + 32, "75");
            }

            InitDone = true;
        }
    }
}