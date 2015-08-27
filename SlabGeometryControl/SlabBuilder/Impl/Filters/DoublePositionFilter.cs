using System;
using System.Collections.Generic;
using Alvasoft.SensorValueContainer;

namespace Alvasoft.SlabBuilder.Impl.Filters
{
    public class DoublePositionFilter
    {
        public static void Filter(ref ISensorValueInfo[] aPositionValues, ref ISensorValueInfo[] aSensorValues)
        {
            if (aPositionValues == null || aSensorValues == null) {
                throw new ArgumentNullException("aPositionValues" + " or " + "aSensorValue");
            }

            if (aPositionValues.Length != aSensorValues.Length) {
                throw new ArgumentException("Длина массивов не совпадает.");
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

            var newPosirionValues = new List<ISensorValueInfo>();
            var newSensorValues = new List<ISensorValueInfo>();
            for (var i = 0; i < aPositionValues.Length; ++i) {
                if (!removed.Contains(i)) {
                    newPosirionValues.Add(aPositionValues[i]);
                    newSensorValues.Add(aSensorValues[i]);
                }
            }

            aPositionValues = newPosirionValues.ToArray();
            aSensorValues = newSensorValues.ToArray();
        }
    }
}
