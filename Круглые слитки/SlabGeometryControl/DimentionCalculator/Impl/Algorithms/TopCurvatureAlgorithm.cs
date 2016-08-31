using System;
using Alvasoft.SlabBuilder.Impl;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class TopCurvatureAlgorithm : IDimentionAlgorithm {
        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("TopCurvatureAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.CenterLine == null) {
                throw new ArgumentNullException("TopCurvatureAlgorithm, CalculateValue: Центральные точки равны null.");
            }

            if (aSlabModel.Diameters == null) {
                throw new ArgumentNullException("TopCurvatureAlgorithm, CalculateValue: Диаметры слитка равны null.");
            }

            if (aSlabModel.Diameters.Length != aSlabModel.CenterLine.Length) {
                throw new ArgumentNullException("TopCurvatureAlgorithm, CalculateValue: Количество центральных точек не равно количеству точек диаметра.");
            }

            var xpoints = new double[aSlabModel.CenterLine.Length];
            var ypoints = new double[aSlabModel.CenterLine.Length];
            var points3d = new Point3D[aSlabModel.CenterLine.Length];

            for (var i = 0; i < aSlabModel.CenterLine.Length; ++i) {
                xpoints[i] = aSlabModel.CenterLine[i].Z;
                ypoints[i] = aSlabModel.CenterLine[i].Y + aSlabModel.Diameters[i] / 2.0;
                points3d[i] = new Point3D {
                    X = xpoints[i],
                    Y = ypoints[i],
                    Z = 0
                };
            }

            // Строим выпуклую оболочку из имеющихся точек.
            var saddlePoints = ConvexHull.Build(xpoints, ypoints, 1);
            var leftSaddlePoint = 0;
            var maxCurvature = double.MinValue;
            for (var i = 1; i < points3d.Length - 1; ++i) {
                if (i == saddlePoints[leftSaddlePoint + 1]) {
                    maxCurvature = Math.Max(maxCurvature, 0);
                    leftSaddlePoint++;
                } else {
                    var distance = points3d[i].DistanceToLine(
                        points3d[saddlePoints[leftSaddlePoint]],
                        points3d[saddlePoints[leftSaddlePoint + 1]]);
                    maxCurvature = Math.Max(maxCurvature, distance);
                }
            }

            return Math.Round(maxCurvature, 4, MidpointRounding.ToEven);
        }        

        public string GetName() {
            return "top_curvature";
        }
    }
}
