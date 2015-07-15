using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;
using Alvasoft.Utils.Mathematic3D;
using NUnit.Framework;

namespace Alvasoft.Tests
{
    [TestFixture]
    public class SlabModelImplTest
    {
        [Test]
        public void GetingTopPointsTest()
        {
            var slabModel = BuildSlabModel();

            Assert.AreEqual(slabModel.GetTopLimit(), 500);

            var t1 = slabModel.GetTopSidePoint(-10, 100);
            Assert.AreEqual(t1.X, -10);
            Assert.AreEqual(t1.Y, 500);
            Assert.AreEqual(t1.Z, 100);

            var t2 = slabModel.GetTopSidePoint(-500, 500);
            Assert.AreEqual(t2.X, -500);
            Assert.AreEqual(t2.Y, 500);
            Assert.AreEqual(t2.Z, 500);

            Assert.Throws<ArgumentException>(() => slabModel.GetTopSidePoint(1001, 100));
        }

        [Test]
        public void GetingBottomPointsTest()
        {
            var slabModel = BuildSlabModel();

            Assert.AreEqual(slabModel.GetBottomLimit(), -500);

            var t1 = slabModel.GetBottomSidePoint(-10, 100);
            Assert.AreEqual(t1.X, -10);
            Assert.AreEqual(t1.Y, -500);
            Assert.AreEqual(t1.Z, 100);

            var t2 = slabModel.GetBottomSidePoint(-500, 500);
            Assert.AreEqual(t2.X, -500);
            Assert.AreEqual(t2.Y, -500);
            Assert.AreEqual(t2.Z, 500);

            Assert.Throws<ArgumentException>(() => slabModel.GetBottomSidePoint(1001, 100));
        }

        [Test]
        public void GetingLeftPointsTest()
        {
            var slabModel = BuildSlabModel();

            Assert.AreEqual(slabModel.GetLeftLimit(), -500);

            var t1 = slabModel.GetLeftSidePoint(-10, 100);
            Assert.AreEqual(t1.X, -500);
            Assert.AreEqual(t1.Y, -10);
            Assert.AreEqual(t1.Z, 100);

            var t2 = slabModel.GetLeftSidePoint(-500, 500);
            Assert.AreEqual(t2.X, -500);
            Assert.AreEqual(t2.Y, -500);
            Assert.AreEqual(t2.Z, 500);

            Assert.Throws<ArgumentException>(() => slabModel.GetLeftSidePoint(1001, 100));
        }

        [Test]
        public void GetingRightPointsTest()
        {
            var slabModel = BuildSlabModel();

            Assert.AreEqual(slabModel.GetRightLimit(), 500);

            var t1 = slabModel.GetRightSidePoint(-10, 100);
            Assert.AreEqual(t1.X, 500);
            Assert.AreEqual(t1.Y, -10);
            Assert.AreEqual(t1.Z, 100);

            var t2 = slabModel.GetRightSidePoint(-500, 500);
            Assert.AreEqual(t2.X, 500);
            Assert.AreEqual(t2.Y, -500);
            Assert.AreEqual(t2.Z, 500);

            Assert.Throws<ArgumentException>(() => slabModel.GetRightSidePoint(1001, 100));
        }

        private ISlabModel BuildSlabModel()
        {
            var slab = new SlabModelImpl();

            slab.TopLimit = 50*10;
            slab.BottomLimit = -50*10;
            slab.LeftLimit = -50*10;
            slab.RightLimit = 50*10;
            slab.LengthLimit = 1000;

            slab.TopLines = new Point3D[3][];
            for (var i = 0; i < 3; ++i) {
                slab.TopLines[i] = new Point3D[1000];
                for (var j = 0; j < 1000; ++j) {
                    slab.TopLines[i][j] = new Point3D {
                        X = -20*10 + i * (20 * 10),
                        Y = 50 * 10,
                        Z = j
                    };
                }
            }

            slab.BottomLines = new Point3D[3][];
            for (var i = 0; i < 3; ++i) {
                slab.BottomLines[i] = new Point3D[1000];
                for (var j = 0; j < 1000; ++j) {
                    slab.BottomLines[i][j] = new Point3D {
                        X = -20 * 10 + i * (20 * 10),
                        Y = -50 * 10,
                        Z = j
                    };
                }
            }

            slab.LeftLines = new Point3D[3][];
            for (var i = 0; i < 3; ++i) {
                slab.LeftLines[i] = new Point3D[1000];
                for (var j = 0; j < 1000; ++j) {
                    slab.LeftLines[i][j] = new Point3D {
                        X = -50 * 10,
                        Y = -20 * 10 + i * (20 * 10),
                        Z = j
                    };
                }
            }

            slab.RightLines = new Point3D[3][];
            for (var i = 0; i < 3; ++i) {
                slab.RightLines[i] = new Point3D[1000];
                for (var j = 0; j < 1000; ++j) {
                    slab.RightLines[i][j] = new Point3D {
                        X = 50 * 10,
                        Y = -20 * 10 + i * (20 * 10),
                        Z = j
                    };
                }
            }

            return slab;
        }
    }
}
