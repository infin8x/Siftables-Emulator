namespace Sifteo.Util
{
    public class StateMachine
    {
        
        public void State(string state, StateFunction func)
        {
            throw new System.NotImplementedException();
        }

        public delegate string StateFunction(string transitionId);
    }
}
