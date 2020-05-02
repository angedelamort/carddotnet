using System.Collections.Generic;

namespace CardEngine.Widgets
{
    public interface IPanel : IWidget
    {
        IList<IWidget> Children { get; }
    }
}
