using System;
using Alvasoft.DataEnums;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorConfiguration.XmlImpl;
using NUnit.Framework;

namespace Alvasoft.Tests
{
    [TestFixture]
    public class XmlSensorConfigurationTest
    {
        private class SensorConfigurationListener : ISensorConfigurationListener
        {
            public ISensorInfo CretedSensor = null;

            public void OnSensorCreated(ISensorConfiguration aConfiguration, int aSensorId)
            {
                CretedSensor = aConfiguration.ReadSensorInfoById(aSensorId);
            }

            public void OnSensorUpdated(ISensorConfiguration aConfiguration, int aSensorId)
            {
                throw new System.NotImplementedException();
            }

            public void OnSensorDeleted(ISensorConfiguration aConfiguration, int aSensorId)
            {
                throw new System.NotImplementedException();
            }
        }

        [Test]
        public void LoadingConfigurationTest()
        {
            var configuration = new XmlSensorConfigurationImpl("Tests/Files/SensorConfigurationTest.xml");
            configuration.Initialize();

            var sensor = configuration.ReadSensorInfoById(0);
            Assert.IsNotNull(sensor);
            Assert.AreEqual(sensor.GetName(), "Top0");
            Assert.AreEqual(sensor.GetSensorType(), SensorType.PROXIMITY);
            Assert.AreEqual(sensor.GetSensorSide(), SensorSide.TOP);
            Assert.AreEqual(sensor.GetShift(), 123.4);

            var psensor = configuration.ReadSensorInfoByName("Position");
            Assert.IsNotNull(psensor);
            Assert.AreEqual(psensor.GetName(), "Position");
            Assert.AreEqual(psensor.GetSensorType(), SensorType.POSITION);
            Assert.AreEqual(psensor.GetSensorSide(), SensorSide.TOP);
            Assert.AreEqual(psensor.GetShift(), 0);
        }

        [Test]
        public void CreatingSensorInfoTest()
        {            
            var configuration = new XmlSensorConfigurationImpl();
            var listener = new SensorConfigurationListener();
            configuration.SubscribeConfigurationListener(listener);
            configuration.CreateSensorInfo(
                new SensorInfoImpl(1, "1", SensorType.PROXIMITY, SensorSide.LEFT, 0));
            var fromListener = listener.CretedSensor;
            Assert.IsNotNull(fromListener);
            Assert.AreSame(fromListener, configuration.ReadSensorInfoById(1));
            Assert.Throws<ArgumentException>(() => configuration.CreateSensorInfo(
                new SensorInfoImpl(1, "1", SensorType.PROXIMITY, SensorSide.LEFT, 0)));
        }
    }
}
