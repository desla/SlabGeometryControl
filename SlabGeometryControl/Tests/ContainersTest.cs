using System.Collections.Generic;
using Alvasoft.DimentionCalculator.Impl;
using Alvasoft.DimentionValueContainer;
using Alvasoft.DimentionValueContainer.Impl;
using Alvasoft.SensorValueContainer;
using Alvasoft.SensorValueContainer.Impl;
using NUnit.Framework;

namespace Alvasoft.Tests
{
    [TestFixture]
    public class ContainersTest
    {
        /// <summary>
        /// Определяет количество раз вызовов данного слушателя.
        /// </summary>
        private class SensorValueContainerListener : 
            ISensorValueContainerListener            
        {
            public int CallCount { get; set; }

            public void OnDataReceived(ISensorValueContainer aContainer)
            {
                CallCount++;
            }
        }

        //[Test]
        //public void SensorValueContainerTest()
        //{
        //    var container = new SensorValueContainerImpl();
        //    var listener = new SensorValueContainerListener();
        //    container.SunbscribeContainerListener(listener);

        //    var collection = new List<ISensorValueInfo>();
        //    for (var i = 0; i < 100; ++i) {
        //        collection.Add(new SensorValueInfoImpl(i, i, i));
        //    }

        //    foreach (var item in collection) {
        //        container.AddSensorValue(item.GetSensorId(), item.GetValue(), item.GetTime());
        //    }

        //    Assert.AreEqual(listener.CallCount, collection.Count);

        //    Assert.AreEqual(container.GetAllValues(49).Length, 50);
        //    Assert.AreEqual(container.GetAllValues(99).Length, 0);
        //    Assert.AreEqual(container.GetAllValues(-1).Length, 100);
        //    Assert.AreEqual(container.GetAllValues(0).Length, 99);

        //    container.UnsunbscribeContainerListener(listener);
        //    container.AddSensorValue(100, 100, 100);
        //    Assert.AreEqual(listener.CallCount, 100);

        //    Assert.IsTrue(!container.IsEmpty());
        //    container.Clear();
        //    Assert.IsTrue(container.IsEmpty());
        //    Assert.AreEqual(container.GetAllValues(-1).Length, 0);
        //}

        [Test]
        public void DimentionValueContainerTest()
        {
            var container = new DimentionValueContainerImpl();
            var collection = new List<IDimentionValue>();
            for (var i = 0; i < 100; ++i) {
                collection.Add(new DimentionValueImpl(i, i));
            }

            foreach (var item in collection) {
                container.AddDimentionValue(item);
            }

            Assert.AreEqual(container.GetDimentionValues().Length, 100);
            Assert.AreSame(container.GetDimentionValues()[5], collection[5]);

            container.Clear();
            Assert.IsTrue(container.IsEmpty());
        }
    }
}
