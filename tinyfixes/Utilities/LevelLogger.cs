using System.Drawing;
using System.Text;
using Reloaded.Mod.Interfaces;

namespace tinyfixes.Utilities
{
    public class LevelLogger
    {
        public static bool Verbose { get; set; } = true;

        private readonly StringBuilder mMessage;
        private readonly ILogger mLogger;

        public string Category { get; }

        public LevelLogger(ILogger logger, string category = "")
        {
            mLogger = logger;
            Category = category;
            mMessage = new StringBuilder();
        }

        private void WriteLine(string prefix, string msg, Color c)
        {
            if (!Verbose) return;

            mMessage.Clear();
            if (!string.IsNullOrWhiteSpace(Category))
                mMessage.Append($"[{Category}] ");
            if (!string.IsNullOrWhiteSpace(prefix))
                mMessage.Append($"{prefix} ");
            mLogger.WriteLineAsync(mMessage.Append(msg).ToString(), c);
        }

        public void Info(string msg)
            => WriteLine("I", msg, mLogger.TextColor);

        public void Warning(string msg)
            => WriteLine("W", msg, mLogger.ColorYellow);

        public void Error(string msg)
            => WriteLine("E", msg, mLogger.ColorRed);

        public void Success(string msg)
            => WriteLine("S", msg, mLogger.ColorGreen);
    }
}