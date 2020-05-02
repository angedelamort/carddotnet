namespace CardEngine.Widgets
{
    public struct Alignment
    {
        public Alignment(AlignmentX horizontal, AlignmentY vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public AlignmentX Horizontal { get; set; }
        public AlignmentY Vertical { get; set; }
    }
}
