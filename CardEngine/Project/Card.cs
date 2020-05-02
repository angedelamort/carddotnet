using CardEngine.Data;
using CardEngine.Widgets;

namespace CardEngine.Project
{
    public class Card
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public Script PageScript { get; } = new Script();

        public IPanel Panel { get; set; }
    }
}
