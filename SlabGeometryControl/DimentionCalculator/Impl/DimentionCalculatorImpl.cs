﻿using System.Collections.Generic;
using Alvasoft.DimentionCalculator.Impl.Algorithms;
using Alvasoft.DimentionConfiguration;
using Alvasoft.DimentionValueContainer;
using Alvasoft.SlabBuilder;
using Alvasoft.Utils.Activity;
using log4net;

namespace Alvasoft.DimentionCalculator.Impl
{
    public class DimentionCalculatorImpl :
        InitializableImpl,
        IDimentionCalculator
    {
        private static readonly ILog logger = LogManager.GetLogger("DimentionCalculatorImpl");

        private List<IDimentionAlgorithm> algorithms = new List<IDimentionAlgorithm>();
        private IDimentionValueContainer container = null;
        private IDimentionConfiguration configuration = null;

        public void SetDimentionConfiguration(IDimentionConfiguration aConfiguration)
        {
            configuration = aConfiguration;
        }

        public void SetDimentionValueContainer(IDimentionValueContainer aContainer)
        {
            container = aContainer;
        }

        public void CalculateDimentions(ISlabModel aSlabModel)
        {
            logger.Info("Вычисление параметров слитка...");

            foreach (var algorithm in algorithms) {
                var result = algorithm.CalculateValue(aSlabModel);
                var dimentionId = GetDimentionId(algorithm);
                var dimentionValue = new DimentionValueImpl(dimentionId, result);
                container.AddDimentionValue(dimentionValue);                
            }

            logger.Info("Вычисление закончено.");
        }

        private int GetDimentionId(IDimentionAlgorithm algorithm)
        {
            var dimention = configuration.GetDimentionInfoByName(algorithm.GetName());
            return dimention.GetId();
        }

        protected override void DoInitialize()
        {            
            logger.Info("Инициализация...");
            algorithms.Add(new HeightAlgorithm());
            algorithms.Add(new WidthAlgorithm());
            algorithms.Add(new LengthAlgorithms());
            //algorithms.Add(new ...);
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {     
            logger.Info("Деинициализация...");
            logger.Info("Деинициализация завершена.");
        }
    }
}
