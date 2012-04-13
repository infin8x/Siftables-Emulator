using System.Collections.Generic;

namespace Sifteo.Util
{
    public class StateMachine
    {
        public Dictionary<string, IStateController> States;

        public IStateController CurrentState {  get {} }

        public StateMachine()
        {
            States = new Dictionary<string, IStateController>();
        }

        public StateMachine State(string name, IStateController scene)
        {
            if (!States.ContainsKey(name))
            {
                States.Add(name, scene);
            }
            return this;
        }

        public StateMachine State(string name, StateFunction func)
        {
            return this.State(name, new SimpleState(this, func));
        }

        public delegate string StateFunction(string transitionId);
    }


    class SimpleState : IStateController
    {
        private StateMachine _machine;
        private StateMachine.StateFunction _func;

        public SimpleState(StateMachine stateMachine, StateMachine.StateFunction func)
        {
            _machine = _machine;
            _func = func;
        }
        public void OnSetup(string transitionId)
        {
            throw new System.NotImplementedException();
        }

        public void OnTick(float dt)
        {
        }

        public void OnPaint(bool canvasDirty)
        {

        }

        public void OnDispose()
        {
        }
    }


}
