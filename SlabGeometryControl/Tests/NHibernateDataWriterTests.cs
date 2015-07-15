using System;
using Alvasoft.DataWriter;
using Alvasoft.DataWriter.NHibernateImpl;
using Alvasoft.DataWriter.NHibernateImpl.Entity;
using Alvasoft.DimentionCalculator.Impl;
using Alvasoft.DimentionValueContainer;
using Alvasoft.SensorValueContainer.Impl;
using NUnit.Framework;

namespace Alvasoft.Tests
{
    [TestFixture]
    public class NHibernateDataWriterTests
    {        
        /// <summary>
        /// Проверяем что инициализация проходит успешно.
        /// </summary>
        [Test]        
        public void SensorValueDataWriterInitialize()
        {
            var dataWriter = new NHibernateSensorValueWriter();
            dataWriter.Initialize();
            dataWriter.Uninitialize();
        }

        /// <summary>
        /// Проверяем, что данные успешно сохраняются.
        /// </summary>
        [Test]
        public void SensorValueDataWriterSaveData()
        {
            var dataWriter = new NHibernateSensorValueWriter();
            dataWriter.Initialize();

            var rnd = new Random();
            var value = new SensorValueInfoImpl(rnd.Next(), rnd.Next(), rnd.Next());
            dataWriter.WriteSensorValueInfo(value);

            var fromDb = dataWriter.ReadSensorValueEntity(value.GetSensorId(), value.GetTime());

            Assert.IsNotNull(fromDb);
            Assert.AreEqual(value.GetValue(), fromDb.Value);
            Assert.AreEqual(value.GetTime(), fromDb.Time);
            Assert.AreEqual(value.GetSensorId(), fromDb.SensorId);
            Assert.Greater(fromDb.Id, 0);

            dataWriter.Uninitialize();
        }

        [Test]
        public void DimentionValueWriterInitializeTest()
        {
            var writer = new NHibernateDimentionValueWriter();            
            writer.Initialize();
            writer.Uninitialize();
        }

        [Test]
        public void SlabWriterTest()
        {
            var slabWriter = new NHibernateSlabInfoWriter();
            slabWriter.Initialize();

            var dimentionWriter = new NHibernateDimentionValueWriter();
            var realSlabWriter = new RealSlabWriter {
                Writer = slabWriter
            };            
            dimentionWriter.Initialize();

            var rnd = new Random();
            var dimentions = new IDimentionValue[] {
                new DimentionValueImpl(rnd.Next(), rnd.Next()),
                new DimentionValueImpl(rnd.Next(), rnd.Next()),
            };

            dimentionWriter.WriteDimentionValues(realSlabWriter.GetNewSlabId(), dimentions);

            var fromDb = dimentionWriter
                .ReadDimentionValue
                (dimentions[1].GetDimentionId(), realSlabWriter.LastSlabId);

            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.GetValue(), dimentions[1].GetValue());
        }

        [Test]
        public void DimentionValueWriterTest()
        {
            var writer = new NHibernateDimentionValueWriter();
            var slabWriter = new ConstSlabWriter();            
            writer.Initialize();

            var dimentions = new IDimentionValue[] {
                new DimentionValueImpl (1, 1),
                new DimentionValueImpl (2, 2),
                new DimentionValueImpl (3, 3),
                new DimentionValueImpl (4, 4),
                new DimentionValueImpl (5, 5),
            };

            var slabId = slabWriter.GetNewSlabId();
            writer.WriteDimentionValues(slabId, dimentions);

            var value1 = writer.ReadDimentionValue(1, slabId);
            Assert.IsNotNull(value1);
            Assert.AreEqual(value1.GetDimentionId(), 1);
            Assert.AreEqual(value1.GetValue(), 1);
            Assert.AreEqual(value1.GetSlabId(), slabWriter.GetNewSlabId());

            var value3 = writer.ReadDimentionValue(3, slabWriter.GetNewSlabId());
            Assert.IsNotNull(value3);
            Assert.AreEqual(value3.GetDimentionId(), 3);
            Assert.AreEqual(value3.GetValue(), 3);
            Assert.AreEqual(value3.GetSlabId(), slabWriter.GetNewSlabId());
        }

        private class RealSlabWriter : ISlabInfoWriter
        {
            public NHibernateSlabInfoWriter Writer { get; set; }
            public int LastSlabId { get; private set; }

            public int GetNewSlabId()
            {
                LastSlabId = Writer.StoreNewSlab(0, 1);
                return LastSlabId;
            }           
        }

        private class ConstSlabWriter : ISlabInfoWriter
        {
            private int slabId = -1;
            private static Random rnd = new Random();

            public int GetNewSlabId()
            {
                if (slabId == -1) {
                    slabId = rnd.Next();
                }

                return slabId;
            }
        }
    }
}
