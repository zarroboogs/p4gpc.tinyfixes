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
        protected readonly IMemory mMem;

        protected readonly Process mProc;
        protected readonly IntPtr mBaseAddr;

        public Fix(ILogger logger, bool enabled)
        {
            Enabled = enabled;
            mLogger = new LevelLogger(logger, Name);

            mProc = Process.GetCurrentProcess();
            mBaseAddr = mProc.MainModule.BaseAddress;
            
            mMem = new ExternalMemory(mProc.Handle);
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
