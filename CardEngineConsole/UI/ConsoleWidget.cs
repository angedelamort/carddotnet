using CardEngine.Widgets;

namespace CardEngineConsole.UI
{
    class ConsoleWidget : ConsoleBase, IWidget
    {
        public int Id { get => GetProperty<int>(); set => SetProperty(value); }
        public string Name { get => GetProperty<string>(); set => SetProperty(value); }
        public Position Position { get => GetProperty<Position>(); set => SetProperty(value); }
        public Size Size { get => GetProperty<Size>(); set => SetProperty(value); }
        public Padding Margin { get => GetProperty<Padding>(); set => SetProperty(value); }
        public Padding Padding { get => GetProperty<Padding>(); set => SetProperty(value); }
        public bool Visible { get => GetProperty<bool>(); set => SetProperty(value); }
        public float Opacity { get => GetProperty<float>(); set => SetProperty(value); }
    }
}
