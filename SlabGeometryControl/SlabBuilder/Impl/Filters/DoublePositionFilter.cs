using System;
using System.Collections.Generic;
using Alvasoft.SensorValueContainer;

namespace Alvasoft.SlabBuilder.Impl.Filters
{
    public class DoublePositionFilter
    {
        public static void Filter(ref ISensorValueInfo[] aPositionValues)
        {
            if (aPositionValues == null) {
                throw new ArgumentNullException("aPositionValues");
            }
            
            var removed = new List<int>();
            var inflectionPoint = 0;
            for (var i = 1; i < aPositionValues.Length; ++i) {
                if (aPositionValues[i].GetValue() > aPositionValues[inflectionPoint].GetValue()) {
                    inflectionPoint = i;                    
                }
                else {
                    removed.Add(i);
                }
            }

            var newPositionValues = new List<ISensorValueInfo>();            
            for (var i = 0; i < aPositionValues.Length; ++i) {
                if (!removed.Contains(i)) {
                    newPositionValues.Add(aPositionValues[i]);                    
                }
            }

            aPositionValues = newPositionValues.ToArray();           
        }
    }
}
