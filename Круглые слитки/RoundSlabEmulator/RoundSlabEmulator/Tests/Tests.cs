using System.Drawing;
using NUnit.Framework;
using RoundSlabEmulator.Configuration;

namespace RoundSlabEmulator {

    [TestFixture]
    public class Tests {

        [Test]
        public void TestStraightContactCricle() {
            var slabEmulator = new Emulator.RoundSlabEmulator();
            double a, b, c;
            slabEmulator.BuildLine(0, 0, 0, -1, out a, out b, out c);
            PointF A, B;
            slabEmulator.StraightContactCricle(0, 0, 1, a, b, c, out A, out B);

            Assert.AreEqual(0, A.X);
            Assert.AreEqual(-1, A.Y);

            Assert.AreEqual(0, B.X);
            Assert.AreEqual(1, B.Y);
        }

        [Test]
        public void TestStraightContactCricle1() {
            var slabEmulator = new Emulator.RoundSlabEmulator();
            double a, b, c;
            slabEmulator.BuildLine(0, 0, 10, 0, out a, out b, out c);
            PointF A, B;
            slabEmulator.StraightContactCricle(0, 0, 1, a, b, c, out A, out B);

            Assert.AreEqual(1, A.X);
            Assert.AreEqual(0, A.Y);

            Assert.AreEqual(-1, B.X);
            Assert.AreEqual(0, B.Y);
        }

        [Test]
        public void TestIntersectionLength() {
            var sensor = new SensorConfiguration();
            sensor.Position = new PointF(2, 0);
            sensor.ScanVertor = new PointF(-1, 0);

            var configuration = new EmulatorConfiguration();
            configuration.Diameter = 1;

            var slabEmulator = new Emulator.RoundSlabEmulator();
            slabEmulator.setConfiguration(configuration);

            var distance = slabEmulator.IntersectionLength(sensor, new PointF(0, 0));

            Assert.AreEqual(1.5, distance);
        }
    }
}
