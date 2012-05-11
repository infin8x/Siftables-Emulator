using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sifteo.MathExt;

namespace Test
{
    [TestClass]
    public class AABBTests
    {
        private AABB _aabb;
        private int _top;
        private int _bottom;
        private int _left;
        private int _right;
        private Int2 _topLeft;
        private Int2 _topRight;
        private Int2 _bottomLeft;
        private Int2 _bottomRight;

        [TestInitialize]
        public void SetUp()
        {
            var x = 50;
            var y = 75;
            var width = 100;
            var height = 125;
            _aabb = new AABB(x, y, width, height);
            _top = y;
            _left = x;
            _right = x + width;
            _bottom = y + height;
            _topLeft = new Int2(_left, _top);
            _topRight = new Int2(_right, _top);
            _bottomLeft = new Int2(_left, _bottom);
            _bottomRight = new Int2(_right, _bottom);
        }

        [TestMethod]
        public void TestTopLeft()
        {
            Assert.AreEqual(_topLeft, _aabb.TopLeft);
        }

        [TestMethod]
        public void TestTopRight()
        {
            Assert.AreEqual(_topRight, _aabb.TopRight);
        }

        [TestMethod]
        public void TestBottomLeft()
        {
            Assert.AreEqual(_bottomLeft, _aabb.BottomLeft);
        }

        [TestMethod]
        public void TestBottomRight()
        {
            Assert.AreEqual(_bottomRight, _aabb.BottomRight);
        }

        [TestMethod]
        public void TestChangeRight()
        {
            _aabb.Right = 500;
            Assert.AreEqual(new Int2(500, _top), _aabb.TopRight);
            Assert.AreEqual(new Int2(500, _bottom), _aabb.BottomRight);
        }

        [TestMethod]
        public void TestChangeLeft()
        {
            _aabb.Left = 0;
            Assert.AreEqual(new Int2(0, _top), _aabb.TopLeft);
            Assert.AreEqual(new Int2(0, _bottom), _aabb.BottomLeft);
        }

        [TestMethod]
        public void TestChangeBottom()
        {
            _aabb.Bottom = 500;
            Assert.AreEqual(new Int2(_left, 500), _aabb.BottomLeft);
            Assert.AreEqual(new Int2(_right, 500), _aabb.BottomRight);
        }

        [TestMethod]
        public void TestChangeTop()
        {
            _aabb.Top = 0;
            Assert.AreEqual(new Int2(_left, 0), _aabb.TopLeft);
            Assert.AreEqual(new Int2(_right, 0), _aabb.TopRight);
        }

        [TestMethod]
        public void TestChangeTopLeft()
        {
            _aabb.TopLeft = new Int2(0, 0);
            Assert.AreEqual(0, _aabb.Top);
            Assert.AreEqual(0, _aabb.Left);
            Assert.AreEqual(_right - 0, _aabb.size.x);
            Assert.AreEqual(_bottom - 0, _aabb.size.y);
        }

        [TestMethod]
        public void TestChangeTopRight()
        {
            _aabb.TopRight = new Int2(500, 0);
            Assert.AreEqual(0, _aabb.Top);
            Assert.AreEqual(500, _aabb.Right);
            Assert.AreEqual(500 - _left, _aabb.size.x);
            Assert.AreEqual(_bottom - 0, _aabb.size.y);
        }

        [TestMethod]
        public void TestChangeBottomLeft()
        {
            _aabb.BottomLeft = new Int2(0, 500);
            Assert.AreEqual(500, _aabb.Bottom);
            Assert.AreEqual(0, _aabb.Left);
            Assert.AreEqual(_right - 0, _aabb.size.x);
            Assert.AreEqual(500 - _top, _aabb.size.y);
        }

        [TestMethod]
        public void TestChangeBottomRight()
        {
            _aabb.BottomRight = new Int2(500, 500);
            Assert.AreEqual(500, _aabb.Bottom);
            Assert.AreEqual(500, _aabb.Right);
            Assert.AreEqual(500 - _left, _aabb.size.x);
            Assert.AreEqual(500 - _top, _aabb.size.y);
        }

        [TestMethod]
        public void TestIntersection()
        {
            AABB result;
            var otherAabb = new AABB(0, 0, 100, 100);
            _aabb.Intersection(otherAabb, out result);
            Assert.AreEqual(50, result.Left);
            Assert.AreEqual(100, result.Right);
            Assert.AreEqual(75, result.Top);
            Assert.AreEqual(100, result.Bottom);
        }
    }
}