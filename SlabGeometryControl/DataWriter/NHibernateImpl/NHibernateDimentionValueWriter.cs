using System;
using Alvasoft.DataWriter.NHibernateImpl.Entity;
using Alvasoft.DimentionCalculator.Impl;
using Alvasoft.DimentionValueContainer;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;
using log4net;
using NHibernate;
using NHibernate.Criterion;

namespace Alvasoft.DataWriter.NHibernateImpl
{
    public class NHibernateDimentionValueWriter :
        InitializableImpl,
        IDimentionValueReaderWriter
    {
        private static readonly ILog logger = LogManager.GetLogger("NHibernateDimentionValueWriter");
        
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

        public void WriteDimentionValues(int aSlabId, IDimentionValue[] aValues)
        {
            try {
                using (var session = NHibernateHelper.OpenSession()) {
                    foreach (var dimentionValue in aValues) {
                        var dimentionValueEntity = new DimentionValueEntity(dimentionValue);
                        dimentionValueEntity.SlabId = aSlabId;
                        using (var t = session.BeginTransaction()) {
                            session.Save(dimentionValueEntity);
                            t.Commit();
                        }
                    }
                }                
            }
            catch (Exception ex) {
                logger.Info("Ошибка при сохранении параметров слитка: " + ex.Message);
            }
        }        

        /// <summary>
        /// Для тестов сохранения значений.
        /// </summary>
        /// <param name="aDimentionId"></param>
        /// <param name="aSlabId"></param>
        /// <returns></returns>
        public IDimentionValue ReadDimentionValue(int aSlabId, int aDimentionId)
        {
            try {
                using (var session = NHibernateHelper.OpenSession()) {
                    var entity = session.CreateCriteria(typeof(DimentionValueEntity))
                    .Add(Restrictions.Eq("DimentionId", aDimentionId))
                    .Add(Restrictions.Eq("SlabId", aSlabId))
                    .UniqueResult<DimentionValueEntity>();

                    if (entity == null) {
                        return null;
                    }

                    return new DimentionValueImpl(entity.DimentionId, entity.Value);                
                }                
            }
            catch (Exception ex) {
                logger.Error("Ошибка при чтении значения: " + ex.Message);
                return null;
            }
        }
    }
}
