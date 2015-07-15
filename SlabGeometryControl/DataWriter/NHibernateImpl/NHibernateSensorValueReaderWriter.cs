using System;
using System.Collections.Generic;
using Alvasoft.DataWriter.NHibernateImpl.Entity;
using Alvasoft.SensorValueContainer;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;
using log4net;
using NHibernate;
using NHibernate.Criterion;

namespace Alvasoft.DataWriter.NHibernateImpl
{
    public class NHibernateSensorValueWriter : 
        InitializableImpl,
        ISensorValueWriter,
        ISensorValueReader
    {
        private static readonly ILog logger = LogManager.GetLogger("NHibernateSensorValueWriter");        

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");                        
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            logger.Info("Деинициализация завершена.");
        }

        public void WriteSensorValueInfo(ISensorValueInfo aValue)
        {
            try {
                if (aValue != null) {
                    var sensorValueEntity = new SensorValueEntity(aValue);
                    using (var session = NHibernateHelper.OpenSession()) {
                        using (var t = session.BeginTransaction()) {
                            session.Save(sensorValueEntity);
                            t.Commit();
                        }
                    }                    
                }                               
            }
            catch (Exception ex) {
                logger.Error("Ошибка при сохранении SensorValue: " + ex.Message);
            }
        }

        /// <summary>
        /// Для тестов сохранения значений.
        /// </summary>
        /// <param name="aSensorId"></param>
        /// <param name="aTime"></param>
        /// <returns></returns>
        public SensorValueEntity ReadSensorValueEntity(int aSensorId, long aTime)
        {
            try {
                using (var session = NHibernateHelper.OpenSession()) {
                    return session.CreateCriteria(typeof(SensorValueEntity))
                    .Add(Restrictions.Eq("SensorId", aSensorId))
                    .Add(Restrictions.Eq("Time", aTime))
                    .UniqueResult<SensorValueEntity>();
                }                
            }
            catch (Exception ex) {
                logger.Error("Ошибка при чтении значения: " + ex.Message);
                return null;
            }
        }

        public ISensorValueInfo[] ReadSensorValueInfo(int aSensorId, long aFromTime, long aToTime)
        {
            try {
                using (var session = NHibernateHelper.OpenSession()) {
                    var entitys = session.CreateCriteria(typeof(SensorValueEntity))
                        .Add(Restrictions.Eq("SensorId", aSensorId))
                        .Add(Restrictions.Between("Time", aFromTime, aToTime))
                        .List<SensorValueEntity>();
                    var results = new List<ISensorValueInfo>();

                    foreach (var entity in entitys) {
                        results.Add(new SensorValueImpl {
                            Id = entity.Id,
                            Value = entity.Value,
                            Time = entity.Time
                        });
                    }

                    return results.ToArray();
                }                
            }
            catch (Exception ex) {
                logger.Error("Ошибка при чтении значения: " + ex.Message);
                return null;
            }
        }
    }
}
