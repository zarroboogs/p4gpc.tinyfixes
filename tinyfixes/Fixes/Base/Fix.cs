using System;
using System.Diagnostics;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;
using tinyfixes.Utilities;

namespace tinyfixes.Fixes
{
    public abstract class Fix
    {
        public abstract string Name { get; }
        public bool Enabled { get; protected set; } = false;
        public bool InitDone { get; protected set; } = false;
        public bool InitFailed { get; protected set; } = false;

        protected readonly LevelLogger mLogger;
        protected readonly MemPatch mPatch;

        protected static readonly Process sProc;
        protected static readonly IntPtr sBaseAddr;
        protected static readonly IMemory sMem;

        static Fix()
        {
            sProc = Process.GetCurrentProcess();
            sBaseAddr = sProc.MainModule.BaseAddress;
            sMem = new Memory();
        }

        public Fix(ILogger logger)
        {
            mLogger = new LevelLogger(logger, Name);
            mPatch = new MemPatch(sMem);
        }

        protected abstract void Init();

        public void Toggle(bool on)
        {
            if (on)
            {
                Activate();
                return;
            }

            Deactivate();
        }

        public void Activate()
        {
            if (!InitDone && !InitFailed) Init();
            if (InitFailed) mLogger.Warning("Init failed, skipping...");
            if (!InitDone || Enabled) return;

            mPatch.Apply();
            mLogger.Info("Activated!");

            Enabled = true;
        }

        public void Deactivate()
        {
            if (!InitDone || !Enabled) return;

            mPatch.Revert();
            mLogger.Info("Deactivated!");

            Enabled = false;
        }
    }
}