namespace CardEngine.Widgets
{
    public interface IText : IWidget
    {
        string Value { get; set; }

        Alignment TextAlignment { get; set; }
        
        Font Font { get; set; }
    }
}
