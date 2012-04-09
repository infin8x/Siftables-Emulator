using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void TestState()
        {
            _sm.State("State1", state1Controller);
        }

        [TestMethod]
        public void TestStateFunction()
        {
            
        }

        [TestMethod]
        public void TestCurrentStateGet()
        {
            Assert.Inconclusive();
        }
    }
}