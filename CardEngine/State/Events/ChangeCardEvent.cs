using CardEngine.Controls;
using CardEngine.Project;

namespace CardEngine.State.Events
{
    internal class ChangeCardEvent : BaseEvent
    {
        public Card NewCard { get; }
        public Transition Transition { get; }

        public ChangeCardEvent(CardState cardState, Card newCard, Transition transition) 
            : base(cardState)
        {
            NewCard = newCard;
            Transition = transition;
        }
    }
}
