using System;
using System.Timers;
using log4net;
using NHibernate.Criterion;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;

namespace Alvasoft.DataWriter.DataCleaner
{
    using System.Threading;
    using NHibernateImpl.Entity;
    using Timer = System.Timers.Timer;

    public class NHibernateDataCleanerImpl
        : InitializableImpl
    {
        private static readonly ILog logger = LogManager.GetLogger("NHibernateDataCleanerImpl");

        private Timer timer;
        private ISlabInfoReader dataReader;

        public void SetSlabInfoWriter(ISlabInfoReader aDataReader)
        {
            dataReader = aDataReader;
        }

        protected override void DoInitialize()
        {                     
            logger.Info("Инициализация...");            
            timer = new Timer();
            timer.Interval = 60 * 60 * 1000;
            timer.Elapsed += TryClearSensorValues;                        
            logger.Info("Инициализация завершена.");            
            ThreadPool.QueueUserWorkItem(StartDAtaCleaner);
        }

        private void StartDAtaCleaner(object state)
        {
            TryClearSensorValues(null, null);
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            timer.Stop();
            timer.Dispose();
            logger.Info("Деинициализация завершена.");
        }

        private void TryClearSensorValues(object sender, ElapsedEventArgs e)
        {
            try {
                timer.Stop();
                Console.WriteLine("Удаление устаревших показаний датчиков...");
                logger.Info("Удаление устаревших показаний датчиков...");
                var from = new DateTime(DateTime.Now.Date.AddDays(-1000).Ticks, DateTimeKind.Local).ToBinary();
                var to = new DateTime(DateTime.Now.Date.AddDays(-7).Ticks, DateTimeKind.Local).ToBinary();
                var slabInfos = dataReader.GetSlabInfoByTimeInterval(from, to);
                if (slabInfos == null || slabInfos.Length == 0) {
                    Console.WriteLine("Нет данных для удалдения.");
                    logger.Info("Нет данных для удалдения.");
                    return;
                }
                using (var session = NHibernateHelper.OpenSession()) {
                    foreach (var slabInfo in slabInfos) {
                        using (var transaction = session.BeginTransaction()) {
                            var entitys = session.CreateCriteria(typeof (SensorValueEntity))
                                .Add(Restrictions.Eq("SlabId", slabInfo.GetId()))
                                .List<SensorValueEntity>();
                            if (entitys != null && entitys.Count > 0) {
                                Console.WriteLine("Данных для удаления: " + entitys.Count);
                                logger.Info("Данных для удаления: " + entitys.Count);
                                foreach (var sensorValueEntity in entitys) {
                                    session.Delete(sensorValueEntity);
                                }
                                transaction.Commit();
                                Console.WriteLine("Данные удалены, slabid = " + slabInfo.GetId());
                            }
                            else {
                                Console.WriteLine("Данные были удалены ранее, slabid = " + slabInfo.GetId());
                            }
                        }
                    }
                }
                logger.Info("Удаление завершено.");
            }
            catch (Exception ex) {
                Console.WriteLine("Ошибка при удалении данных показаний датчиков: " + ex.Message);
                logger.Error("Ошибка при удалении данных показаний датчиков: " + ex.Message);
            }
            finally {
                timer.Start();
            }
        }
    }
}
