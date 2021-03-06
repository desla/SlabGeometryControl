﻿using System;
using System.Collections.Generic;
using Alvasoft.DimentionCalculator.Impl.Algorithms;
using Alvasoft.DimentionConfiguration;
using Alvasoft.DimentionValueContainer;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;
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
                try {
                    var result = algorithm.CalculateValue(aSlabModel as SlabModelImpl);
                    var dimentionId = GetDimentionId(algorithm);
                    var dimentionValue = new DimentionValueImpl(dimentionId, result);
                    container.AddDimentionValue(dimentionValue);
                }
                catch (Exception ex) {
                    logger.Info(string.Format("Не удалось вычислить {0} слитка: {1}", algorithm.GetName(), ex.Message));
                }
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
            algorithms.Add(new LengthAlgorithm()); // длина.
            algorithms.Add(new BackDiameterAlgorithm()); // Диаметр на заднем торце слитка.
            algorithms.Add(new FrontDiameterAlgorithm()); // Диаметр на переднем торце слитка.
            algorithms.Add(new MiddleDiameterAlgorithm()); // Диаметр посередине слитка.
            algorithms.Add(new FrontMiddleDiameterAlgorithm()); // Диаметр между серединой и передним торцом с учетом отступа.
            algorithms.Add(new BackMiddleDiameterAlgorithm()); // Диаметр между серединой и задним торцом с учетом отступа.
            algorithms.Add(new TopCurvatureAlgorithm()); // Максимальная кривизна сверху на всей длине слитка.
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {     
            logger.Info("Деинициализация...");
            logger.Info("Деинициализация завершена.");
        }
    }
}
