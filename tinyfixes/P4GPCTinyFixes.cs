using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Structs;
using Reloaded.Mod.Interfaces;
using tinyfixes.Configuration;

namespace tinyfixes
{
    public unsafe class P4GPCTinyFixes
    {
        private readonly ILogger mLogger;
        private readonly Config mConfig;

        private readonly Process mProc;
        private readonly IntPtr mBaseAddr;
        private readonly IntPtr mHnd;

        private readonly SubFix[] mSubs = new SubFix[]
        {
            new SubFix(1, "\nI'll reserve a room for", " I'll reserve\na room for"),
            new SubFix(2, "we've been through", "we've been\nthrough"),
        };

        private readonly IDictionary<int, string> mLangs = new Dictionary<int, string>()
        {
            [0] = "Japanese",
            [1] = "English",
            [2] = "Korean",
            [3] = "Chinese",
        };

        public P4GPCTinyFixes(ILogger logger, Config config)
        {
            mLogger = logger;
            mConfig = config;

            mProc = Process.GetCurrentProcess();
            mBaseAddr = mProc.MainModule.BaseAddress;
            mHnd = mProc.Handle;
        }

        public void Apply()
        {
            if (mConfig.IntroSkip)
                PatchIntro();

            if (mConfig.SlotFix)
                PatchSlot();

            if (mConfig.SubsFix)
                PatchSubs();

            if (mConfig.TexWrapFix)
                PatchTexWrap();
        }

        private void PatchSubs()
        {
            mLogger.WriteLine("[tinyfixes] <SubsFix> Patching subtitles...");

            using var scan = new Scanner(mProc, mProc.MainModule);

            var start = 0;

            foreach (var s in mSubs)
            {
                var pattern = BitConverter.ToString(Encoding.ASCII.GetBytes(s.Pattern)).Replace("-", " ");
                var compSub = new CompiledScanPattern(pattern);
                var resSub = scan.CompiledFindPattern(compSub, start);

                if (!resSub.Found)
                {
                    mLogger.WriteLine($"[tinyfixes] <SubsFix> Pattern #{s.Id} not found, maybe already patched?");
                    continue;
                }

                start = resSub.Offset;

                mLogger.WriteLine($"[tinyfixes] <SubsFix> Pattern #{s.Id} found, patching...");
                byte[] buff = Encoding.ASCII.GetBytes(s.Replace);

                Util.VirtualProtectEx(mHnd, mBaseAddr + resSub.Offset, (UIntPtr)buff.Length, 0x04, out _);
                Util.WriteProcessMemory(mHnd, mBaseAddr + resSub.Offset, buff, buff.Length, out _);
            }
        }

        private void PatchSlot()
        {
            mLogger.WriteLine("[tinyfixes] <SlotFix> Patching save slots...");

            using var scan = new Scanner(mProc, mProc.MainModule);

            var compPush = new CompiledScanPattern("8d 95 f8 fd ff ff 8b ce 6a 06");
            var compTable = new CompiledScanPattern("8b 04 b5 ?? ?? ?? ?? ff 30 57 ff d3");
            var resPush = scan.CompiledFindPattern(compPush, 0);
            var resTable = scan.CompiledFindPattern(compTable, 0);
            
            if (!resPush.Found || !resTable.Found)
            {
                mLogger.WriteLine("[tinyfixes] <SlotFix> Pattern not found, maybe already patched?");
                return;
            }

            mLogger.WriteLine("[tinyfixes] <SlotFix> Pattern found, patching parser...");

            Util.WriteProcessMemory(mHnd, mBaseAddr + resPush.Offset + 8, new byte[] { 0x6a, 0x07 }, 2, out _);
            Util.WriteProcessMemory(mHnd, mBaseAddr + resPush.Offset + 8 + 0x2C, new byte[] { 0x6a, 0x06 }, 2, out _);

            mLogger.WriteLine("[tinyfixes] <SlotFix> Patching parser table...");

            for (int i = 0; i < 4; i++)
            {
                var ppTClr = Util.DeRef(mHnd, mBaseAddr + resTable.Offset + 3, new int[] { i * 4, 0x18 });
                var ppLang = ppTClr + 4;

                var pTClr = new Span<byte>(ppTClr.ToPointer(), 4).ToArray();
                var pLang = new Span<byte>(ppLang.ToPointer(), 4).ToArray();

                mLogger.WriteLine($"[tinyfixes] <SlotFix> Patching {mLangs[i]}...");

                Util.WriteProcessMemory(mHnd, ppTClr, pLang, pLang.Length, out _);
                Util.WriteProcessMemory(mHnd, ppLang, pTClr, pTClr.Length, out _);
            }
        }

        private void PatchTexWrap()
        {
            mLogger.WriteLine("[tinyfixes] <TexWrapFix> Patching TexWrap...");

            // 81 4f 24 00 c0 00 00 -- parse gmo TexWrap -- 1
            // f7 40 24 00 c0 00 00 -- apply gmo TexWrap (draw) -- 2

            var resDraw = Util.FindAllPatterns(mProc, "f7 40 24 00 c0 00 00", 2);

            if (resDraw.Count == 0)
            {
                mLogger.WriteLine("[tinyfixes] <TexWrapFix> Pattern not found, maybe already patched?");
                return;
            }
            else if (resDraw.Count != 2)
            {
                mLogger.WriteLine("[tinyfixes] <TexWrapFix> Too many results, skipping...");
                return;
            }

            foreach (var res in resDraw)
            {
                mLogger.WriteLine("[tinyfixes] <TexWrapFix> Pattern found, patching...");
                Util.WriteProcessMemory(mHnd, mBaseAddr + res.Offset + 20, new byte[] { 0x75 }, 1, out _);
                Util.WriteProcessMemory(mHnd, mBaseAddr + res.Offset + 25, new byte[] { 0x90, 0x90 }, 2, out _);
                Util.WriteProcessMemory(mHnd, mBaseAddr + res.Offset + 32, new byte[] { 0x75 }, 1, out _);
            }
        }

        private void PatchIntro()
        {
            mLogger.WriteLine("[tinyfixes] <IntroSkip> Patching intro...");

            using var scan = new Scanner(mProc, mProc.MainModule);

            var compIntro = new CompiledScanPattern("5e 5b 5d c3 b9 01 00 00 00");
            var resIntro = scan.CompiledFindPattern(compIntro, 0);

            if (!resIntro.Found)
            {
                mLogger.WriteLine("[tinyfixes] <IntroSkip> Pattern not found, maybe already patched?");
                return;
            }

            mLogger.WriteLine("[tinyfixes] <IntroSkip> Pattern found, patching...");

            Util.WriteProcessMemory(mHnd, mBaseAddr + resIntro.Offset + 4, new byte[] { 0xe9, 0x30 }, 2, out _);
        }
    }
}
