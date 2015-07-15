using System.Collections.Generic;
using System.Runtime.InteropServices;
using Alvasoft.DataWriter.NHibernateImpl.Entity;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;
using log4net;
using NHibernate;

namespace Alvasoft.DataWriter.NHibernateImpl
{
    public class NHibernateRegulationReaderWriter : 
        InitializableImpl,
        IRegulationsReaderWriter
    {
        private static readonly ILog logger = LogManager.GetLogger("NHibernateStandartSizeReaderWriter");        

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

        public IRegulation[] GetRegulations()
        {
            using (var session = NHibernateHelper.OpenSession()) {
                var entitys = session.CreateCriteria(typeof(RegulationEntity))
                .List<RegulationEntity>();

                var results = new List<IRegulation>();
                foreach (var entity in entitys) {
                    results.Add(new RegulationImpl {
                        Id = entity.Id,
                        DimentionId = entity.DimentionId,
                        StandartSizeId = entity.StandartSizeId,
                        MaxValue = entity.MaxValue,
                        MinValue = entity.MinValue
                    });
                }

                return results.ToArray();
            }            
        }

        public int CreateRegulation(IRegulation aRegulation)
        {
            using (var session = NHibernateHelper.OpenSession()) {
                var entity = new RegulationEntity {
                    DimentionId = aRegulation.GetDimentionId(),
                    StandartSizeId = aRegulation.GetStandartSizeId(),
                    MaxValue = aRegulation.GetMaxValue(),
                    MinValue = aRegulation.GetMinValue()
                };

                using (var transaction = session.BeginTransaction()) {
                    session.Save(entity);
                    transaction.Commit();
                }

                return entity.Id;
            }            
        }

        public void RemoveRegulation(int aId)
        {
            using (var session = NHibernateHelper.OpenSession()) {
                using (var transaction = session.BeginTransaction()) {
                    var entity = session.Get<RegulationEntity>(aId);
                    session.Delete(entity);
                    transaction.Commit();
                }
            }            
        }

        public void EditRegulation(IRegulation aRegulation)
        {
            using (var session = NHibernateHelper.OpenSession()) {
                using (var transaction = session.BeginTransaction()) {
                    var entity = session.Get<RegulationEntity>(aRegulation.GetId());
                    entity.DimentionId = aRegulation.GetDimentionId();
                    entity.StandartSizeId = aRegulation.GetStandartSizeId();
                    entity.MaxValue = aRegulation.GetMaxValue();
                    entity.MinValue = aRegulation.GetMinValue();
                    session.Update(entity);
                    transaction.Commit();
                }
            }            
        }
    }
}
