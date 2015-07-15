using System;
using Alvasoft.DataProviderConfiguration;
using Alvasoft.DataProviderConfiguration.XmlImpl;
using NUnit.Framework;

namespace Alvasoft.Tests
{
    [TestFixture]
    public class XmlDataProviderConfigurationTest
    {
        private class ConfigurationListener :
            IDataProviderConfigurationListener
        {
            public IOpcSensorInfo CreatedSensor = null;

            public void OnSensorCreated(IDataProviderConfiguration aConfiguration, int aSensorId)
            {
                CreatedSensor = aConfiguration.ReadOpcSensorInfoById(aSensorId);
            }

            public void OnSensorUpdated(IDataProviderConfiguration aConfiguration, int aSensorId)
            {
                throw new System.NotImplementedException();
            }

            public void OnSensorDeleted(IDataProviderConfiguration aConfiguration, int aSensorId)
            {
                throw new System.NotImplementedException();
            }
        }

        [Test]
        public void LoadingConfigurationTest()
        {
            var configuration = new XmlDataProviderConfigurationImpl("Tests/Files/OpcConfigurationTest.xml");
            configuration.Initialize();

            Assert.AreEqual(configuration.GetOpcSensorInfoCount(), 2);
            var top = configuration.ReadOpcSensorInfoByName("Top0");
            var bottom = configuration.ReadOpcSensorInfoByName("Bottom0");
            Assert.IsNotNull(top);
            Assert.IsNotNull(bottom);
        }

        [Test]
        public void CreatindSensorTest()
        {
            var configuration = new XmlDataProviderConfigurationImpl();
            var listener = new ConfigurationListener();
            configuration.SubscribeConfigurationListener(listener);

            var sensor = new OpcSensorInfoImpl {
                Id = 1,
                Name = "TestName"
            };

            configuration.CreateOpcSensorInfo(sensor);

            Assert.AreEqual(configuration.GetOpcSensorInfoCount(), 1);
            Assert.AreSame(sensor, configuration.ReadOpcSensorInfoByName(sensor.Name));
            Assert.AreSame(sensor, listener.CreatedSensor);

            configuration.UnsubscribeConfigurationListener(listener);

            var sensor2 = new OpcSensorInfoImpl {
                Id = 1,
                Name = "sensor2"
            };

            Assert.Throws<ArgumentException>(() => configuration.CreateOpcSensorInfo(sensor2));

            sensor2.Id = 2;
            configuration.CreateOpcSensorInfo(sensor2);

            Assert.AreNotSame(listener.CreatedSensor, configuration.ReadOpcSensorInfoById(2));
        }
    }
}
