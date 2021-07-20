using Reloaded.Memory.Sigscan;
using Reloaded.Mod.Interfaces;
using tinyfixes.Utilities;

namespace tinyfixes.Fixes
{
    public sealed class SubsFix : Fix
    {
        public override string Name => "tinyfixes | SubsFix";

        private readonly (int Id, string Pattern, string Replace)[] mSubs = {
            (1, "\nI'll reserve a room for", " I'll reserve\na room for"),
            (2, "we've been through", "we've been\nthrough"),
        };

        public SubsFix(ILogger logger) : base(logger) { }

        protected override void Init()
        {
            using var scan = new Scanner(sProc, sProc.MainModule);
            var start = 0;

            foreach (var (Id, Pattern, Replace) in mSubs)
            {
                var res = scan.CompiledFindPattern(Pattern.ToHexString(), start);

                if (!res.Found)
                {
                    InitFailed = true;
                    return;
                }

                start = res.Offset;
                mLogger.Info($"Adding patch - pattern #{Id}...");
                mPatch.Add(sBaseAddr + res.Offset, Replace.ToHexString());
            }

            InitDone = true;
        }
    }
}