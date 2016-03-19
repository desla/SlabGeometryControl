using System;
using System.Collections.Generic;
using Alvasoft.SensorValueContainer;

namespace Alvasoft.SlabBuilder.Impl.Filters {

    /// <summary>
    /// Фильтр пиков датчика положения.    
    /// </summary>
    public class PickPositionFilter {

        /// <summary>
        /// Предел изменения показаний датчика за тик.
        /// </summary>
        private static double maxDistance = 30;

        public static void Filter(ref ISensorValueInfo[] aPositionValues) {
            //return;
            if (aPositionValues == null) {
                throw new ArgumentNullException("aPositionValues");
            }

            var removed = new HashSet<int>();
            var lastPoint = 0;
            for (var i = 1; i < aPositionValues.Length; ++i) {
                if (Math.Abs(aPositionValues[i].GetValue() - aPositionValues[lastPoint].GetValue()) > maxDistance * (i - lastPoint)) {
                    removed.Add(i);
                } else {
                    lastPoint = i;
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
