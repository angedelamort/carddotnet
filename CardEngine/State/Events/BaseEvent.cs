namespace CardEngine.State.Events
{
    internal abstract class BaseEvent
    {
        public CardState Card { get; }

        /// <summary>
        /// All events should have a card state.
        /// </summary>
        /// <param name="cardState"></param>
        protected BaseEvent(CardState cardState)
        {
            Card = cardState;
        }
    }
}
