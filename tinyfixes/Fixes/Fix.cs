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
        public bool Enabled { get; } = false;

        protected readonly LevelLogger mLogger;

        protected static readonly IMemory mMem;
        protected static readonly Process mProc;
        protected static readonly IntPtr mBaseAddr;

        static Fix()
        {
            mProc = Process.GetCurrentProcess();
            mBaseAddr = mProc.MainModule.BaseAddress;
            mMem = new Memory();
        }

        public Fix(ILogger logger, bool enabled)
        {
            Enabled = enabled;
            mLogger = new LevelLogger(logger, Name);
        }

        public void Apply()
        {
            if (!Enabled)
            {
                mLogger.Info("Disabled, skipping...");
                return;
            }

            mLogger.Info("Applying...");
            OnApply();
        }

        protected abstract void OnApply();
    }
}
