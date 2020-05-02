namespace CardEngine.Widgets
{
    public struct Font
    {
        public string Family { get; set; }

        // bold, italic, etc
        public string Style { get; set; }

        public int Size { get; set; }

        /// <summary>
        /// A scale between 0 and 200 for the boldness
        ///     100: normal.
        ///     0: ultra-light
        ///     200: ultra-bold
        /// </summary>
        public int Weight { get; set; }
    }
}
