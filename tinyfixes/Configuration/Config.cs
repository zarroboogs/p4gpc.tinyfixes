using System.ComponentModel;
using tinyfixes.Configuration.Implementation;

namespace tinyfixes.Configuration
{
    public class Config : Configurable<Config>
    {
        /*
            User Properties:
                - Please put all of your configurable properties here.
                - Tip: Consider using the various available attributes https://stackoverflow.com/a/15051390/11106111

            By default, configuration saves as "Config.json" in mod folder.
            Need more config files/classes? See Configuration.cs
        */

        [DisplayName("Verbose")]
        [Description("Enable verbose logging.")]
        public bool Verbose { get; set; } = false;

        [DisplayName("NG+ Save Slot Fix")]
        [Description("Apply NG+ save slot fix.")]
        [Category("Fix")]
        public bool SaveSlotFix { get; set; } = true;

        [DisplayName("Subtitle Fixes")]
        [Description("Apply subtitle fixes.")]
        [Category("Fix")]
        public bool SubsFix { get; set; } = true;

        [DisplayName("TexWrap Fix")]
        [Description("Apply TexWrap fix.")]
        [Category("Fix")]
        public bool TexWrapFix { get; set; } = true;

        [DisplayName("Intro Skip")]
        [Description("Skip boot logos and title movie.")]
        [Category("Patch")]
        public bool IntroSkipPatch { get; set; } = false;

        [DisplayName("P4 Title Movie")]
        [Description("Use vanilla P4 title movie.")]
        [Category("Patch")]
        public bool TitleMoviePatch { get; set; } = false;
    }
}