using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardEngine.Controls;
using CardEngine.Project;
using CardEngine.State;
using CardEngine.State.Events;
using ScriptEngine;

namespace CardEngine
{
    // TODO: add debugger capabilities.
    public class Engine : IFunctionsApi, IGlobalsApi
    {
        private readonly Solution solution;
        private readonly Dictionary<string, object> globalVariableInstances = new Dictionary<string, object>();
        private CardState cardState;
        private volatile bool isRunning;
        private State engineState = State.Initializing;
        private readonly IRenderer renderer;
        private readonly ILogger logger;
        private readonly EventCollection events = new EventCollection();

        private enum State
        {
            Initializing,
            CardChange,
            CardChanging,
            ProcessEvents,
            Error
        }

        public Engine(Solution solution, EngineOptions options)
        {
            this.solution = solution ?? throw new ArgumentNullException(nameof(solution));

            renderer = options.Renderer;
            logger = options.Logger;
        }

        public Exception EngineError { get; private set; }

        public void Start()
        {
            if (cardState != null)
                throw new InvalidOperationException("The engine is already started. Stop it before starting it again.'");
            if (isRunning)
                throw new InvalidOperationException("The engine is already running.");

            InitState();

            Loop();
        }

        public void Resume()
        {
            if (cardState == null)
                throw new InvalidOperationException("The engine wasn't started. Use Engine.start().'");
            if (isRunning)
                throw new InvalidOperationException("The engine is already running.");

            Loop();
        }

        public void Stop()
        {
            if (cardState == null)
                throw new InvalidOperationException("The engine wasn't started.'");

            ClearState();
        }

        public bool IsRunning => isRunning;

        public event EventHandler<Exception> OnError;

        private void Loop()
        {
            Task.Run(() =>
            {
                try
                {
                    CardState newCardState = null;
                    Transition transition = null; // TODO: Not sure it's a good idea to put it as local variables.
                    var generator = new ScriptGenerator(
                        this,
                        this,
                        solution.GlobalVariables.Values.ToList(),
                        solution.GlobalScripts.Values.ToList());

                    isRunning = true;

                    while (isRunning)
                    {
                        switch (engineState)
                        {
                            case State.Initializing:
                                var startingCard = solution.Cards[solution.StartingCardId];
                                cardState = new CardState(startingCard, generator);

                                cardState.CreateEvent();
                                cardState.ShowEvent();
                                engineState = State.ProcessEvents;
                                break;

                            case State.ProcessEvents:
                                events.Swap();
                                foreach (var @event in events)
                                {
                                    switch (@event)
                                    {
                                        case ChangeCardEvent changeCardEvent:
                                            transition = changeCardEvent.Transition;
                                            newCardState = new CardState(changeCardEvent.NewCard, generator);
                                            break;
                                    }
                                }

                                events.Clear();
                                break;

                            case State.CardChange:
                                cardState.HideEvent();
                                newCardState?.CreateEvent();
                                newCardState?.ShowEvent();
                                engineState = State.CardChanging;
                                break;

                            case State.CardChanging:
                                if (transition == null || transition.Execute())
                                {
                                    newCardState?.ShownEvent();
                                    cardState.HiddenEvent();
                                    cardState.DestroyEvent(); // TODO: not sure if we should destroy them or just re-use them.

                                    cardState = newCardState;
                                    newCardState = null;
                                    engineState = State.ProcessEvents;
                                    transition = null;
                                }

                                break;
                        }

                        Render(cardState);
                        Render(newCardState);

                        Task.Delay(1);
                    }
                }
                catch (Exception ex)
                {
                    engineState = State.Error;
                    EngineError = ex;
                    OnError?.Invoke(this, ex);
                }
            });
        }

        private void Render(CardState state)
        {
            if (state != null && renderer != null && state.Panel != null)
            {
                state.PreRender();
                renderer.Render(state.Panel);
                state.PostRender();
            }
        }

        private void ClearState()
        {
            globalVariableInstances.Clear();

            cardState?.DestroyEvent();
            cardState = null;
            isRunning = false;
        }

        private void InitState()
        {
            engineState = State.Initializing;

            foreach (var variable in solution.GlobalVariables.Values)
                globalVariableInstances.Add(variable.Name, variable.Value);
        }

        #region FunctionAPI
        public int StartTimeout(string functionName, int delay)
        {
            throw new NotImplementedException();
        }

        public void StopTimeout()
        {
            throw new NotImplementedException();
        }

        public void GoToNextPage()
        {
            var index = solution.Cards.FindIndex(x => x.Id == cardState.Card.Id);
            if (index < solution.Cards.Count - 1)
                GoToCard(index + 1, new Transition());
        }

        public void GoToPreviousPage()
        {
            var index = solution.Cards.FindIndex(x => x.Id == cardState.Card.Id);
            if (index > 0)
                GoToCard(index -1, new Transition());
        }

        public void GoToLastPage()
        {
            var index = solution.Cards.FindIndex(x => x.Id == cardState.Card.Id);
            if (index != solution.Cards.Count - 1)
                GoToCard(solution.Cards.Count - 1, new Transition());
        }

        public void GoToFirstPage()
        {
            var index = solution.Cards.FindIndex(x => x.Id == cardState.Card.Id);
            if (index != 0)
                GoToCard(0, new Transition());
        }

        public void GoToPage(int id)
        {
            var index = solution.Cards.FindIndex(x => x.Id == cardState.Card.Id);
            if (index != id)
                GoToCard(index, new Transition());
        }

        public void GoToPage(string name)
        {
            var card = solution.Cards.Find(x => x.Name == name);
            GoToCard(card, new Transition());
        }

        private void GoToCard(int cardIndex, Transition transition)
        {
            if (cardIndex != -1)
                GoToCard(solution.Cards[cardIndex], transition);
        }

        private void GoToCard(Card card, Transition transition)
        {
            if (card != null)
                events.Push(new ChangeCardEvent(cardState, card, transition));
        }

        public void Write(string msg)
        {
            logger?.Write(msg);
        }

        public void WriteLine(string msg)
        {
            logger?.WriteLine(msg);
        }

        public void Exit()
        {
            Stop();
        }

        #endregion

        #region GlobalVariables
        public object GetVariable(string propertyName)
        {
            return globalVariableInstances[propertyName];
        }

        public void SetVariable(string propertyName, object value)
        {
            globalVariableInstances[propertyName] = value;
        }
        #endregion

        public object InvokeScriptAction(string actionName, params object[] parameters)
        {
            return cardState.InvokeScriptMethod(actionName, parameters);
        }
    }
}
