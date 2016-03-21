using System;
using Alvasoft.SensorValueContainer;
using Alvasoft.SensorValueContainer.Impl;

namespace Alvasoft.SlabBuilder.Impl.Filters {
    public class IncrementOrderConverter {
        public static void Convert(ref ISensorValueInfo[] aPositionValues) {
            if (aPositionValues == null || aPositionValues.Length == 0) {
                return;
            }

            var valuesCount = aPositionValues.Length;
            if (aPositionValues[0].GetValue() > aPositionValues[valuesCount - 1].GetValue()) {
                var maxMinSummed = GetMaxMinSummed(aPositionValues);
                var revercedValues = new SensorValueInfoImpl[valuesCount];
                for (var i = 0; i < valuesCount; ++i) {
                    var sourceValue = aPositionValues[i];
                    var reversedValue = -1.0 * aPositionValues[i].GetValue() + maxMinSummed;
                    revercedValues[i] = new SensorValueInfoImpl(aPositionValues[i].GetSensorId(), reversedValue, aPositionValues[i].GetTime());                  
                }

                aPositionValues = revercedValues;
            }            
        }

        private static double GetMaxMinSummed(ISensorValueInfo[] aSensorValues) {
            var maximum = double.MinValue;
            var minimum = double.MaxValue;
            for (var i = 0; i < aSensorValues.Length; ++i) {
                maximum = Math.Max(maximum, aSensorValues[i].GetValue());
                minimum = Math.Min(minimum, aSensorValues[i].GetValue());
            }

            return maximum + minimum;
        }
    }
}
