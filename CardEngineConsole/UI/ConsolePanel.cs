using System.Collections.Generic;
using CardEngine.Widgets;

namespace CardEngineConsole.UI
{
    class ConsolePanel : ConsoleWidget, IPanel
    {
        public ConsolePanel()
        {
            Children = new DirtyableList<IWidget>();
        }

        public IList<IWidget> Children { get => GetProperty<DirtyableList<IWidget>>(); set => SetProperty(value); }
    }
}
