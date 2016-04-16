using System;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder.Impl.Filters {
    public class AverageFilter
    {
        private const int WINDOW_SIZE = 50; // размер окна для вычисления среднего.        

        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }

            if (aSlab.Diameters != null) {
                FilterDiameters(aSlab.Diameters);
            }

            if (aSlab.CenterLine != null) {
                FilterCenters(aSlab.CenterLine);
            }
        }

        private static void FilterCenters(Point3D[] aCenterLine) {
            var newCenters = new Point3D[aCenterLine.Length];
            for (var i = 0; i < aCenterLine.Length; ++i) {
                var sumX = 0.0;
                var sumY = 0.0;
                var pointCount = 0;
                for (var j = i - WINDOW_SIZE; j <= i + WINDOW_SIZE; ++j) {
                    if (j >= 0 && j < aCenterLine.Length) {
                        sumX += aCenterLine[j].X;
                        sumY += aCenterLine[j].Y;
                        pointCount++;
                    }
                }

                newCenters[i] = new Point3D {
                    X = sumX / pointCount,
                    Y = sumY / pointCount
                };
            }

            for (var i = 0; i < aCenterLine.Length; ++i) {
                aCenterLine[i].X = newCenters[i].X;
                aCenterLine[i].Y = newCenters[i].Y;
            }
        }

        private static void FilterDiameters(double[] aDiameters) {
            var newDiameters = new double[aDiameters.Length];
            for (var i = 0; i < aDiameters.Length; ++i) {
                var sumDiameter = 0.0;
                var pointCount = 0;
                for (var j = i - WINDOW_SIZE; j <= i + WINDOW_SIZE; ++j) {
                    if (j >= 0 && j < aDiameters.Length) {
                        sumDiameter += aDiameters[j];
                        pointCount++;
                    }
                }

                newDiameters[i] = sumDiameter / pointCount;
            }

            for (var i = 0; i < aDiameters.Length; ++i) {
                aDiameters[i] = newDiameters[i];
            }
        }
    }
}
