using Reloaded.Mod.Interfaces;
using tinyfixes.Configuration;
using tinyfixes.Fixes;
using tinyfixes.Utilities;

namespace tinyfixes
{
    public class P4GPCTinyFixes
    {
        private Config mConfig;

        private readonly ILogger mLogger;

        private readonly SaveSlotFix mSaveSlotFix;
        private readonly SubsFix mSubsFix;
        private readonly TexWrapFix mTexWrapFix;

        private readonly IntroSkipPatch mIntroSkipPatch;
        private readonly TitleMoviePatch mTitleMoviePatch;

        public P4GPCTinyFixes(ILogger logger, Config config)
        {
            mConfig = config;
            mLogger = logger;

            mSaveSlotFix = new SaveSlotFix(mLogger);
            mSubsFix = new SubsFix(mLogger);
            mTexWrapFix = new TexWrapFix(mLogger);

            mIntroSkipPatch = new IntroSkipPatch(mLogger);
            mTitleMoviePatch = new TitleMoviePatch(mLogger);
        }

        public void Activate(Config config)
        {
            mConfig = config;

            LevelLogger.Verbose = mConfig.Verbose;

            Activate();
        }

        public void Activate()
        {
            mSaveSlotFix.Toggle(mConfig.SaveSlotFix);
            mSubsFix.Toggle(mConfig.SubsFix);
            mTexWrapFix.Toggle(mConfig.TexWrapFix);

            mIntroSkipPatch.Toggle(mConfig.IntroSkipPatch);
            mTitleMoviePatch.Toggle(mConfig.TitleMoviePatch);
        }

        public void Deactivate()
        {
            mSaveSlotFix.Deactivate();
            mSubsFix.Deactivate();
            mTexWrapFix.Deactivate();

            mIntroSkipPatch.Deactivate();
            mTitleMoviePatch.Deactivate();
        }
    }
}