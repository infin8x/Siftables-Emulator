using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sifteo;

namespace Test
{
    public class SoundTests
    {
        private BaseApp _app;


        [TestInitialize]
        public void SetUp()
        {
            _app = new BaseApp();
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestBadSoundThrowsException()
        {

        }
    }
}
