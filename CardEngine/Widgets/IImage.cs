namespace CardEngine.Widgets
{
    public interface IImage : IWidget
    {
        ResizeMode ResizeMode { get; set; }
        // TODO: probably store the image binary format in memory.
    }
}
