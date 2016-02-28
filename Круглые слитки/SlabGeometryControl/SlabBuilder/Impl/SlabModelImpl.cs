using System;
using System.Collections.Generic;
using Alvasoft.SlabGeometryControl;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder.Impl
{
    public class SlabModelImpl : ISlabModel
    {        
        public double LengthLimit { get; set; }

        /// <summary>
        /// Точки должны быть отсортированы по возрастанию координаты Z.        
        /// </summary>
        public Point3D[] CenterLine { get; set; }

        public double[] Diameters { get; set; }               

        public double GetLengthLimit() {
            return LengthLimit;
        }

        public Point3D GetCenterPoint(double aZ) {
            if (aZ < 0 || aZ > LengthLimit) {
                throw new ArgumentException("GetCenterPoint: точка выходит за пределы слитка.");
            }

            for (var i = 1; i < CenterLine.Length; ++i) {
                if (aZ >= CenterLine[i - 1].Z && aZ <= CenterLine[i].Z) {
                    var leftPoint = CenterLine[i - 1];
                    var rightPoint = CenterLine[i];
                    var difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                    return new Point3D {
                        X = leftPoint.X + difference * (rightPoint.X - leftPoint.X),
                        Y = leftPoint.Y + difference * (rightPoint.Y - leftPoint.Y),
                        Z = aZ
                    };
                }
            }

            throw new ArgumentException("GetCenterPoint: не удалось найти положение точки на слитке.");
        }

        public double GetDiameter(double aZ) {
            if (aZ < 0 || aZ > LengthLimit) {
                throw new ArgumentException("GetDiameter: точка выходит за пределы слитка.");
            }

            for (var i = 1; i < CenterLine.Length; ++i) {
                if (aZ >= CenterLine[i - 1].Z && aZ <= CenterLine[i].Z) {
                    var leftPoint = CenterLine[i - 1];
                    var rightPoint = CenterLine[i];
                    var difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                    return Diameters[i - 1] + difference * (Diameters[i] - Diameters[i - 1]);
                }
            }

            throw new ArgumentException("GetDiameter: не удалось найти положение точки на слитке.");
        }

        /// <summary>
        /// Для передачи по сети.
        /// </summary>
        /// <returns></returns>
        public SlabModel3D ToSlabModel() {
            var model = new SlabModel3D();
            model.CenterLine = new SlabPoint[CenterLine.Length];
            model.Diameters = new double[Diameters.Length];
            for (var i = 0; i < CenterLine.Length; ++i) {
                model.CenterLine[i] = new SlabPoint {
                    X = CenterLine[i].X,
                    Y = CenterLine[i].Y,
                    Z = CenterLine[i].Z
                };
            }
            for (var i = 0; i < Diameters.Length; ++i) {
                model.Diameters[i] = Diameters[i];
            }

            return model;
        }
    }
}
