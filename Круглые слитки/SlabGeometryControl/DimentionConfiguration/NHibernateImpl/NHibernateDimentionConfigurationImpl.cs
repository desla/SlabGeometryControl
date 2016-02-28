using System;
using System.Linq;
using System.Collections.Generic;
using Alvasoft.DimentionConfiguration.NHibernateImpl.Entity;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;
using log4net;

namespace Alvasoft.DimentionConfiguration.NHibernateImpl
{
    public class NHibernateDimentionConfigurationImpl : 
        InitializableImpl,
        IDimentionConfiguration
    {
        private static readonly ILog logger = LogManager.GetLogger("NHibernateDimentionConfigurationImpl");

        private List<IDimentionInfo> dimentions = new List<IDimentionInfo>();

        public IDimentionInfo GetDimentionInfoById(int aId)
        {
            return dimentions.FirstOrDefault(d => d.GetId() == aId);
        }

        public IDimentionInfo GetDimentionInfoByName(string aName)
        {
            return dimentions.FirstOrDefault(d => d.GetName().Equals(aName));
        }

        public IDimentionInfo GetDimentionInfoByIndex(int aIndex)
        {
            if (aIndex < 0 || aIndex >= dimentions.Count) {
                throw new IndexOutOfRangeException("Нет измерения с таким индексом.");
            }

            return dimentions[aIndex];
        }

        public int GetDimentionInfosCount()
        {
            return dimentions.Count;
        }

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");
            using (var session = NHibernateHelper.OpenSession()) {
                var entities = session
                    .CreateCriteria(typeof (DimentionInfoEntity))
                    .List<DimentionInfoEntity>();
                if (entities != null) {
                    foreach (var entity in entities) {
                        dimentions.Add(entity.ToDimentionInfo());
                    }
                }
            }
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            dimentions.Clear();
            logger.Info("Деиинциализация завершена.");
        }
    }
}
