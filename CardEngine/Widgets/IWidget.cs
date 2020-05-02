namespace CardEngine.Widgets
{
    /// <summary>
    /// All widgets must be implemented via the one who uses the engine.
    /// The goal is to provide an interface for the scripting engine.
    /// </summary>
    public interface IWidget
    {
        int Id { get; set; }
        string Name { get; set; }

        Position Position { get; set; }
        Size Size { get; set; }
        Padding Margin { get; set; }
        Padding Padding { get; set; }

        bool Visible { get; set; }
        float Opacity { get; set; }
    }
}
