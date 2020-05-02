namespace CardEngine.Widgets
{
    public struct Padding
    {
        public Padding(int horizontal, int vertical)
        {
            Left = Right = horizontal;
            Top = Down = vertical;
        }

        public Padding(int all)
        {
            Left = Right = Top = Down = all;
        }

        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Down { get; set; }

        public int All
        {
            get => Left == Right && Right == Top && Top == Down ? Left : -1;
            set => Left = Right = Top = Down = value;
        }

        public int Horizontal
        {
            get => Left == Right ? Left : -1;
            set => Left = Right = value;
        }

        public int Vertical
        {
            get => Top == Down ? Top : -1;
            set => Top = Down = value;
        }
    }
}
