using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sifteo;
using Sifteo.Util;

namespace Test
{
    [TestClass]
    public class StateMachineTests
    {
        private StateMachine _sm;
        [TestInitialize]
        public void Initialize()
        {
            _sm = new StateMachine();
        }
        [TestMethod]
        public void TestNewStateMachine()
        {
            Assert.IsInstanceOfType(_sm, typeof(StateMachine));
        }

        [TestMethod]
        public void TestStateWithIStateController()
        {
            var state1Controller = new State1Controller();
            var returned = _sm.State("State1", state1Controller);
            Assert.IsTrue(_sm.States.ContainsKey("State1"));
            Assert.AreEqual(returned, _sm);
        }

        [TestMethod]
        public void TestStateWithIStateControllerDuplicateKeyInsertion()
        {
            var state1Controller = new State1Controller();
            var returned = _sm.State("State1", state1Controller);
            _sm.State("State1", state1Controller);
            Assert.IsTrue(_sm.States.ContainsKey("State1"));
            Assert.AreEqual(1, _sm.States.Count);
            Assert.AreEqual(returned, _sm);
        }

        [TestMethod]
        public void TestStateWithStateFunction()
        {
            var state2Function = new StateMachine.StateFunction(delegate(string transitionId) { return transitionId; });
            _sm.State("State2", state2Function);
        }

        [TestMethod]
        public void TestCurrentStateGet()
        {
            //Assert.IsNotNull(_sm.CurrentState);
        }
    }

    public class State1Controller : IStateController
    {
        public void OnSetup(string transitionId)
        {
            throw new System.NotImplementedException();
        }

        public void OnTick(float dt)
        {
            throw new System.NotImplementedException();
        }

        public void OnPaint(bool canvasDirty)
        {
            throw new System.NotImplementedException();
        }

        public void OnDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}