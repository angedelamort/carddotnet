using CardEngine.Widgets;

namespace CardEngineConsole.UI
{
    class ConsoleText : ConsoleWidget, IText
    {
        public string Value { get => GetProperty<string>(); set => SetProperty(value); }

        public Alignment TextAlignment { get => GetProperty<Alignment>(); set => SetProperty(value); }

        public Font Font { get => GetProperty<Font>(); set => SetProperty(value); }
    }
}
