using Reloaded.Mod.Interfaces;
using tinyfixes.Configuration;
using tinyfixes.Fixes;

namespace tinyfixes
{
    public class P4GPCTinyFixes
    {
        private readonly Config mConfig;
        private readonly ILogger mLogger;

        private readonly Fix[] mFixes;

        public P4GPCTinyFixes(ILogger logger, Config config)
        {
            mConfig = config;
            mLogger = logger;

            mFixes = new Fix[]
            {
                new IntroFix( mLogger, mConfig.IntroSkip ),
                new SlotFix( mLogger, mConfig.SlotFix ),
                new SubsFix( mLogger, mConfig.SubsFix ),
                new TexWrapFix( mLogger, mConfig.TexWrapFix ),
            };
        }

        public void Apply()
        {
            foreach (var fix in mFixes)
                fix.Apply();
        }
    }
}