﻿using System.Collections.Generic;
using Alvasoft.DataWriter.NHibernateImpl.Entity;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;
using log4net;
using NHibernate;

namespace Alvasoft.DataWriter.NHibernateImpl
{
    public class NHibernateStandartSizeReaderWriter : 
        InitializableImpl,
        IStandartSizeReaderWriter
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

        public IStandartSize[] GetStandartSizes()
        {
            using (var session = NHibernateHelper.OpenSession()) {
                var entitys = session.CreateCriteria(typeof(StandartSizeEntity))
                .List<StandartSizeEntity>();

                var results = new List<IStandartSize>();
                foreach (var entity in entitys) {
                    results.Add(new StandartSizeImpl {
                        Id = entity.Id,
                        Diameter = entity.Diameter
                    });
                }

                return results.ToArray();
            }            
        }

        public int AddStandartSize(IStandartSize aStandartSize)
        {
            var entity = new StandartSizeEntity {
                Diameter = aStandartSize.GetDiameter()
            };

            using (var session = NHibernateHelper.OpenSession()) {
                using (var transaction = session.BeginTransaction()) {
                    session.Save(entity);
                    transaction.Commit();
                }
            }            

            return entity.Id;
        }

        public void RemoveStandartSize(int aId)
        {
            using (var session = NHibernateHelper.OpenSession()) {
                using (var transaction = session.BeginTransaction()) {
                    var entity = session.Get<StandartSizeEntity>(aId);
                    session.Delete(entity);
                    transaction.Commit();
                }
            }            
        }

        public void EditStandartSize(IStandartSize aStandartSize)
        {
            using (var session = NHibernateHelper.OpenSession()) {
                using (var transaction = session.BeginTransaction()) {
                    var entity = session.Get<StandartSizeEntity>(aStandartSize.GetId());
                    entity.Diameter = aStandartSize.GetDiameter();
                    session.Update(entity);
                    transaction.Commit();
                }
            }            
        }
    }
}
