using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;
using tinyfixes.Utilities;

namespace tinyfixes.Fixes
{
    public sealed class SaveSlotFix : Fix
    {
        public override string Name => "tinyfixes | SlotFix";

        private readonly string[] mLangs = { "jpn", "eng", "kor", "zho" };

        public SaveSlotFix(ILogger logger) : base(logger) { }

        protected override void Init()
        {
            using var scan = new Scanner(sProc, sProc.MainModule);
            var resPush = scan.CompiledFindPattern("8D 95 F8 FD FF FF 8B CE 6A 06");
            var resTable = scan.CompiledFindPattern("8B 04 B5 ?? ?? ?? ?? FF 30 57 FF D3");

            if (!resPush.Found || !resTable.Found)
            {
                InitFailed = true;
                return;
            }

            mLogger.Info("Adding patch - parser...");
            mPatch.Add(sBaseAddr + resPush.Offset + 0x09, "07");
            mPatch.Add(sBaseAddr + resPush.Offset + 0x09 + 0x2C, "06");

            for (int i = 0; i < 4; i++)
            {
                mLogger.Info($"Adding patch - {mLangs[i]}...");
                var pTable = sMem.Dereference(sBaseAddr, resTable.Offset + 3, i * 4);

                sMem.SafeReadRaw(pTable + 0x18, out byte[] pTimesCleared, 4);
                sMem.SafeReadRaw(pTable + 0x1C, out byte[] pLang, 4);

                mPatch.Add(pTable + 0x18, pLang);
                mPatch.Add(pTable + 0x1C, pTimesCleared);
            }

            InitDone = true;
        }
    }
}