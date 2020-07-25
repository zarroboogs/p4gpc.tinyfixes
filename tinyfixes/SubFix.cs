namespace tinyfixes
{
    internal class SubFix
    {
        public int Id { get; set; }
        public string Pattern { get; set; }
        public string Replace { get; set; }

        public SubFix(int id, string pattern, string replace)
        {
            Id = id;
            Pattern = pattern;
            Replace = replace;
        }
    }
}
