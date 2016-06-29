using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class HeightAlgorithm : IDimentionAlgorithm
    {        
        public string GetName()
        {
            return "height";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            // вычисляем середину слитка.
            var positionX = 0.5 * (aSlabModel.GetRightLimit() + aSlabModel.GetLeftLimit());
            //var positionX = 0;
            // отступаем 10 см от торца слитка.
            var positionZ = 100;

            var topPoint = aSlabModel.GetTopSidePoint(positionX, positionZ);
            var bottomPoint = aSlabModel.GetBottomSidePoint(positionX, positionZ);            

            var height = Math.Round(topPoint.Y - bottomPoint.Y, 4, MidpointRounding.ToEven);
            
            return height;
        }
    }
}
