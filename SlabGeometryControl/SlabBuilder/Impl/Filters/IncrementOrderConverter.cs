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
                var revercedValues = new SensorValueInfoImpl[valuesCount];
                for (var i = 0; i < valuesCount; ++i) {
                    var sourceValue = aPositionValues[valuesCount - i - 1];
                    revercedValues[i] = new SensorValueInfoImpl(sourceValue.GetSensorId(), sourceValue.GetValue(), aPositionValues[i].GetTime());
                }

                aPositionValues = revercedValues;
            }
        }
    }
}
