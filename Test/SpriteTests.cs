using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiftDomain.Util;
using Siftables;
using Siftables.Sifteo;
using Siftables.ViewModel;
using Sifteo.MathExt;

namespace Test
{
    [TestClass]
    public class SpriteTests
    {
        private Sprite _emptySprite;
        private Sprite _unavailableSprite;
        private Sprite _cubeSprite;
        private Sprite _cubeRotatedSprite;

        private CubeViewModel _cubeViewModel;

        [TestInitialize] 
        public void SetUp()
        {
            _emptySprite = new Sprite();

            var unavailableSpriteData = new SpriteData("boogityWoogity.png", 0, 0, 50, 50);
            _unavailableSprite = new Sprite(unavailableSpriteData);

            var cubeSpriteData = new SpriteData("cube.png", 0, 0, 50, 50);
            _cubeSprite = new Sprite(cubeSpriteData);

            var cubeRotatedSpriteData = new SpriteData("flip.png", 0, 0, 25, 50);
            _cubeRotatedSprite = new Sprite(cubeRotatedSpriteData);

            _cubeViewModel = new CubeViewModel();
            _cubeViewModel.ImageSources = new ImageSources(@"C:/Users/zimmerka/Siftables-Emulator/Applications/ChangingColorsApp/bin/Release/assets/images");
        }

        private static void SpritePaintAndCubePaint(Sprite sprite, CubeViewModel cubeViewModel)
        {
            sprite.Paint(cubeViewModel.CubeModel);
            cubeViewModel.CubeModel.Paint();
        }

        [TestMethod]
        public void TestNullSpriteDoesNothing()
        {
            SpritePaintAndCubePaint(_emptySprite, _cubeViewModel);
            Assert.AreEqual(_cubeViewModel.ScreenItems.Count, 0);
        }

        [TestMethod]
        public void TestDrawUnavailableSpriteDoesNothing()
        {
            SpritePaintAndCubePaint(_unavailableSprite, _cubeViewModel);
            Assert.AreEqual(_cubeViewModel.ScreenItems.Count, 0);
        }

        [TestMethod]
        public void TestDrawInvisibleSpriteDoesNothing()
        {
            _cubeSprite.visible = false;
            SpritePaintAndCubePaint(_cubeSprite, _cubeViewModel);
            Assert.AreEqual(_cubeViewModel.ScreenItems.Count, 0);
        }

        [TestMethod]
        public void TestDrawSpriteAtOriginNoRotationNoPivot()
        {
            SpritePaintAndCubePaint(_cubeSprite, _cubeViewModel);
            Assert.AreEqual(_cubeViewModel.ScreenItems.Count, 1);
        }

        [TestMethod]
        public void TestDrawSpriteAtMiddlePointNoRotationNoPivot()
        {
            _cubeSprite.position = new Int2(64, 64);
            SpritePaintAndCubePaint(_cubeSprite, _cubeViewModel);

            var image = _cubeViewModel.ScreenItems[0];
            Assert.AreEqual(Canvas.GetLeft(image), 64);
            Assert.AreEqual(Canvas.GetTop(image), 64);
        }

        [TestMethod]
        public void TestDrawSpriteAtMiddlePointSizedNoRotationNoPivot()
        {
            _cubeSprite.position = new Int2(64, 64);
            SpritePaintAndCubePaint(_cubeSprite, _cubeViewModel);

            var image = _cubeViewModel.ScreenItems[0];
            Assert.AreEqual(image.Width, 50);
            Assert.AreEqual(image.ActualHeight, 50);
        }

        [TestMethod]
        public void TestDrawSpriteOffCubeDoesNothing()
        {
            _cubeSprite.position = new Int2(500, 500);
            SpritePaintAndCubePaint(_cubeSprite, _cubeViewModel);

            Assert.AreEqual(_cubeViewModel.ScreenItems.Count, 0);
        }

        [TestMethod]
        public void TestBackgroundAfterSpriteRemovesSprite()
        {
            SpritePaintAndCubePaint(_cubeSprite, _cubeViewModel);
            _cubeViewModel.CubeModel.FillScreen(Color.Black);

            Assert.AreEqual(_cubeViewModel.ScreenItems.Count, 0);
        }

        [TestMethod]
        public void TestSpriteOrientationRightRotatesImage()
        {
            _cubeRotatedSprite.Orientation = Cube.Side.RIGHT;
            SpritePaintAndCubePaint(_cubeRotatedSprite, _cubeViewModel);

            // check image's rotation
            var image = _cubeViewModel.ScreenItems[0];
            Assert.AreEqual(50, image.Width);
            Assert.AreEqual(25, image.Height);
        }

        [TestMethod]
        public void TestSpriteOrientationBottomKeepsWidthAndHeight()
        {
            _cubeRotatedSprite.Orientation = Cube.Side.BOTTOM;
            SpritePaintAndCubePaint(_cubeRotatedSprite, _cubeViewModel);

            var image = _cubeViewModel.ScreenItems[0];
            Assert.AreEqual(25, image.Width);
            Assert.AreEqual(50, image.Height);
        }

        [TestMethod]
        public void TestSpritePaintMask()
        {
            var mask = new AABB(25, 25, 50, 50);
            _cubeSprite.PaintMasked(_cubeViewModel.CubeModel, mask);
            _cubeViewModel.CubeModel.Paint();

            var image = _cubeViewModel.ScreenItems[0];
            Assert.AreEqual(25, Canvas.GetLeft(image));
            Assert.AreEqual(25, Canvas.GetTop(image));
            Assert.AreEqual(25, image.Width);
            Assert.AreEqual(25, image.Height);
        }


    }
}