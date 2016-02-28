using System;
using System.Collections.Generic;
using Alvasoft.DataWriter.NHibernateImpl.Entity;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping;

namespace Alvasoft.DataWriter.NHibernateImpl
{
    public class NHibernateSlabInfoWriter : 
        InitializableImpl,
        ISlabInfoReader
    {
        private static readonly ILog logger = LogManager.GetLogger("NHibernateSlabWriter");        

        public int StoreNewSlab(long aStartScanTime, long aEndScanTime)
        {
            try {                
                var slabEntity = new SlabInfoEntity {
                    StartScanTime = aStartScanTime,
                    EndScanTime = aEndScanTime
                };
                using (var session = NHibernateHelper.OpenSession()) {
                    using (var t = session.BeginTransaction()) {
                        session.Save(slabEntity);
                        t.Commit();
                    }
                }                                
                return slabEntity.Id;
            }
            catch (Exception ex) {
                logger.Error("Ошибка при сохранении Slab: " + ex.Message);
                return -1;
            }
        }

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

        public ISlabInfo GetSlabInfo(string aSlabNumber)
        {
            try {
                using (var session = NHibernateHelper.OpenSession()) {
                    var entity = session.CreateCriteria(typeof(SlabInfoEntity))
                            .Add(Restrictions.Eq("Number", aSlabNumber))
                            .UniqueResult<SlabInfoEntity>();
                    return new SlabInfoImpl {
                        Id = entity.Id,
                        Number = entity.Number,
                        StandartSizeId = entity.StandartSizeId,
                        StartScanTime = entity.StartScanTime,
                        EndScanTime = entity.EndScanTime
                    };
                }                
            }
            catch (Exception ex) {
                logger.Error("Ошибка при чтении SlabInfo: " + ex.Message);
                return null;
            }
        }

        public ISlabInfo GetSlabInfo(int aSlabId)
        {
            try {
                using (var session = NHibernateHelper.OpenSession()) {
                    var entity = session.CreateCriteria(typeof(SlabInfoEntity))
                            .Add(Restrictions.Eq("Id", aSlabId))
                            .UniqueResult<SlabInfoEntity>();
                    return new SlabInfoImpl {
                        Id = entity.Id,
                        Number = entity.Number,
                        StandartSizeId = entity.StandartSizeId,
                        StartScanTime = entity.StartScanTime,
                        EndScanTime = entity.EndScanTime
                    };
                }                
            }
            catch (Exception ex) {
                logger.Error("Ошибка при чтении SlabInfo: " + ex.Message);
                return null;
            }
        }

        public ISlabInfo[] GetSlabInfoByTimeInterval(long aFrom, long aTo)
        {
            try {
                using (var session = NHibernateHelper.OpenSession()) {
                    var entitys = session.CreateCriteria(typeof(SlabInfoEntity))
                    .Add(Restrictions.Between("StartScanTime", aFrom, aTo))
                    .List<SlabInfoEntity>();

                    var results = new List<ISlabInfo>();
                    foreach (var entity in entitys) {
                        results.Add(new SlabInfoImpl {
                            Id = entity.Id,
                            Number = entity.Number,
                            StandartSizeId = entity.StandartSizeId,
                            StartScanTime = entity.StartScanTime,
                            EndScanTime = entity.EndScanTime
                        });
                    }

                    return results.ToArray();
                }                
            }
            catch (Exception ex) {
                logger.Error("Ошибка при чтении SlabInfo: " + ex.Message);
                return null;
            }
        }

        public void UpdateStandartSizeId(int aSlabId, int aStandartSizeId) {
            try {
                var slabInfo = GetSlabInfo(aSlabId);
                var slabEntity = new SlabInfoEntity() {
                    Id = slabInfo.GetId(),
                    Number = slabInfo.GetNumber(),
                    EndScanTime = slabInfo.GetEndScanTime(),
                    StartScanTime = slabInfo.GetStartScanTime(),
                    StandartSizeId = aStandartSizeId
                };

                using (var session = NHibernateHelper.OpenSession()) {
                    using (var transaction = session.BeginTransaction()) {
                        session.Update(slabEntity);
                        transaction.Commit();
                    }
                }
            } catch (Exception ex) {
                logger.Error("Ошибка при обновлении StandartSizeId: " + ex.Message);
            }
        }
    }
}
