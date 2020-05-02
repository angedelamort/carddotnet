namespace CardEngine.Widgets
{
    public enum ResizeMode
    {
        None,
        Stretch,
        Cover,      // Resize uniformly. Some clipping might occur in order to fit the whole area
        Contain     // Resize uniformly. Some background might be seen since both width and height will be seen.
    }
}
