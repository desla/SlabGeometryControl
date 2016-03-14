using System;
using System.Collections.Generic;
using Alvasoft.DataEnums;
using Alvasoft.DataProvider;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;
using Alvasoft.SlabBuilder.Impl.Filters;
using Alvasoft.Utils.Activity;
using Alvasoft.Utils.Mathematic3D;
using log4net;

namespace Alvasoft.SlabBuilder.Impl
{
    public class SlabBuilderImpl :
        InitializableImpl,
        ISlabBuilder,
        ISensorConfigurationListener
    {
        private static readonly ILog logger = LogManager.GetLogger("SlabBuilderImpl");

        private ISensorConfiguration configuration;
        private ISensorValueContainer container;
        private ICalibrator calibrator;

        public void OnSensorCreated(ISensorConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void OnSensorUpdated(ISensorConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void OnSensorDeleted(ISensorConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void SetSensorConfiguration(ISensorConfiguration aConfiguration)
        {
            if (aConfiguration == null) {
                throw new ArgumentNullException("aConfiguration");
            }

            configuration = aConfiguration;            
        }

        public void SetSensorValueContainer(ISensorValueContainer aContainer)
        {
            if (aContainer == null) {
                throw new ArgumentNullException("aContainer");
            }

            container = aContainer;
        }

        public void SetCalibrator(ICalibrator aCalibrator)
        {
            if (aCalibrator == null) {
                throw new ArgumentNullException("aCalibrator");
            }

            calibrator = aCalibrator;
        }

        public ISlabModel BuildSlabModel()
        {
            if (container == null || container.IsEmpty()) {
                throw new ArgumentException("Контейнер не содержит данных для построения модели слитка.");
            }

            if (configuration == null) {
                throw new ArgumentException("Конфигурация не установлена.");
            }

            var slab = new SlabModelImpl();

            BuildCenters(slab);

            //SplashFilter.Filter(slab);
            //PickFilter.Filter(slab);
            //BumpFilter.Filter(slab);
            //AverageFilter.Filter(slab);

            BuildLimits(slab);

            return slab;
        }

        /// <summary>
        /// Строит линию центров слитка.
        /// </summary>
        /// <param name="aModel"></param>
        private void BuildCenters(SlabModelImpl aModel) {
            // построим точки на поверхности слитка.
            aModel.SensorsLines = BuildSurfacePoints();

            if (aModel.SensorsLines.Length < 3) {
                throw new ArgumentException("BuildCenters: точек не достаточно для построения модели слитка.");
            }

            var lines = aModel.SensorsLines;
            // теперь по 3-м точкам будем строить описанную окружность и вычислять ее центр и диаметр.
            var pointsCount = lines[0].Length;
            var centers = new Point3D[pointsCount];
            var diameters = new double[pointsCount];
            for (var i = 0; i < pointsCount; ++i) {
                centers[i] = CalcCircleCenter(lines[0][i], lines[1][i], lines[2][i]);
                diameters[i] = CalcCircleDiameter(lines[0][i], lines[1][i], lines[2][i]);
            }

            aModel.CenterLine = centers;
            aModel.Diameters = diameters;
        }

        /// <summary>
        /// Вычисляет диаметр описанной окружности.
        /// </summary>
        /// <param name="aA"></param>
        /// <param name="aB"></param>
        /// <param name="aC"></param>
        /// <returns>Диаметр.</returns>
        private double CalcCircleDiameter(Point3D aA, Point3D aB, Point3D aC) {
            // Формула:
            // D = a*b*c / 2 * sqrt(p*(p-a)*(p-b)*(p-c)), 
            // где a,b,c - длины сторон треугольника, p - полупериметр треугольника.
            var a = aA.DistanceToPoint(aB);
            var b = aB.DistanceToPoint(aC);
            var c = aC.DistanceToPoint(aA);
            var p = (a + b + c) / 2.0;

            return (2 * a * b * c) / (4 * Math.Sqrt( p * (p - a) * (p - b) * (p - c)));
        }

        /// <summary>
        /// Вычисляет центр описанной окружности.
        /// </summary>        
        /// <returns></returns>
        private Point3D CalcCircleCenter(Point3D aA, Point3D aB, Point3D aC) {
            var epsilon = 0.0001;
            if (Math.Abs(aA.Z - aB.Z) > epsilon || Math.Abs(aA.Z - aC.Z) > epsilon) {
                throw new ArgumentException("CalcCircleCenter: точки находятся не в одной плоскости.");
            }

            // формула http://www.cyberforum.ru/geometry/thread1190053.html

            double x1 = aA.X, x2 = aB.X, x3 = aC.X,  
                   y1 = aA.Y, y2 = aB.Y, y3 = aC.Y;
            double x12 = x1 - x2,
                x23 = x2 - x3,
                x31 = x3 - x1,
                y12 = y1 - y2,
                y23 = y2 - y3,
                y31 = y3 - y1;
            double z1 = x1 * x1 + y1 * y1,
                z2 = x2 * x2 + y2 * y2,
                z3 = x3 * x3 + y3 * y3;
            double zx = y12 * z3 + y23 * z1 + y31 * z2,
                zy = x12 * z3 + x23 * z1 + x31 * z2,
                z = x12 * y31 - y12 * x31;

            return new Point3D {
                X = -zx / (2.0 * z),
                Y = zy / (2.0 * z),
                Z = aA.Z
            };
        }

        private Point3D[][] BuildSurfacePoints() {
            // найдем датчик положения, а остальные датчики поместим в массив sensors.
            ISensorInfo positionSensor = null;
            var sensors = new List<ISensorInfo>();
            var sensorCount = configuration.GetSensorInfoCount();
            for (var i = 0; i < sensorCount; ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);
                if (sensorInfo.GetSensorType() == SensorType.PROXIMITY) {
                    sensors.Add(sensorInfo);
                } else if (sensorInfo.GetSensorType() == SensorType.POSITION) {
                    positionSensor = sensorInfo;
                }
            }
            if (positionSensor == null) {
                throw new ArgumentException("BuildSurfacePoints: Датчик положения не найден.");
            }

            var positionValues = container.GetSensorValuesBySensorId(positionSensor.GetId());
            // сначала создадим массивы точек на поверхности слитка.
            var lines = new Point3D[sensors.Count][];

            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                var sensorValues = container.GetSensorValuesBySensorId(sensor.GetId());
                DoublePositionFilter.Filter(ref positionValues, ref sensorValues);
                lines[i] = BuildLineValues(
                    sensor,
                    positionValues,
                    sensorValues,
                    sensor.GetShift(),
                    calibrator.GetCalibratedValue(sensor.GetId()) / 2.0);

                MoveToZeroOnZ(lines[i]);
            }

            return lines;
        }

        private Point3D[] BuildLineValues(
            ISensorInfo aSensor,
            ISensorValueInfo[] aPositions,
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance) {
            switch (aSensor.GetSensorSide()) {
                case SensorSide.TOP:
                    return BuildTopSideValues(aPositions, aSensorValues, aShift, aCenterDistance);
                case SensorSide.BOTTOM:
                    return BuildBottomSideValues(aPositions, aSensorValues, aShift, aCenterDistance);
                case SensorSide.LEFT:
                    return BuildLeftSideValues(aPositions, aSensorValues, aShift, aCenterDistance);
                case SensorSide.RIGHT:
                    return BuildRightSideValues(aPositions, aSensorValues, aShift, aCenterDistance);
                default:
                    throw new ArgumentException("BuildLineValues: не указана сторона датчика.");
            }
        }

        private Point3D[] BuildTopSideValues(
            ISensorValueInfo[] aPositions,
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance) {
            var result = new Point3D[aSensorValues.Length];
            for (var i = 0; i < aSensorValues.Length; ++i) {
                result[i] = new Point3D {
                    X = aShift,
                    Y = aCenterDistance - aSensorValues[i].GetValue(),
                    Z = GetPositionByTime(aPositions, aSensorValues[i].GetTime())
                };
            }

            return result;
        }

        private Point3D[] BuildBottomSideValues(
            ISensorValueInfo[] aPositions,
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance) {
            var result = new Point3D[aSensorValues.Length];
            for (var i = 0; i < aSensorValues.Length; ++i) {
                result[i] = new Point3D {
                    X = aShift,
                    Y = -aCenterDistance + aSensorValues[i].GetValue(),
                    Z = GetPositionByTime(aPositions, aSensorValues[i].GetTime())
                };
            }

            return result;
        }

        private Point3D[] BuildLeftSideValues(
            ISensorValueInfo[] aPositions,
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance) {
            var result = new Point3D[aSensorValues.Length];
            for (var i = 0; i < aSensorValues.Length; ++i) {
                result[i] = new Point3D {
                    X = -aCenterDistance + aSensorValues[i].GetValue(),
                    Y = aShift,
                    Z = GetPositionByTime(aPositions, aSensorValues[i].GetTime())
                };
            }

            return result;
        }

        private Point3D[] BuildRightSideValues(
            ISensorValueInfo[] aPositions,
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance) {
            var result = new Point3D[aSensorValues.Length];
            for (var i = 0; i < aSensorValues.Length; ++i) {
                result[i] = new Point3D {
                    X = aCenterDistance - aSensorValues[i].GetValue(),
                    Y = aShift,
                    Z = GetPositionByTime(aPositions, aSensorValues[i].GetTime())
                };
            }

            return result;
        }


        private void BuildLimits(SlabModelImpl aModel) {
            aModel.LengthLimit = aModel.CenterLine[aModel.CenterLine.Length - 1].Z;
        }

        private double GetPositionByTime(ISensorValueInfo[] aPositions, long aTime)
        {        
            for (var i = 0; i < aPositions.Length; ++i) {
                if (aPositions[i].GetTime() == aTime) {
                    return aPositions[i].GetValue();
                }
            }

            throw new ArgumentException("Не найдена точка для указанног времени.");
        }

        private void MoveToZeroOnZ(Point3D[] aValues)
        {
            var difference = aValues[0].Z;
            
            for (var i = 0; i < aValues.Length; ++i) {
                aValues[i].Z -= difference;
            }
        }        
    }
}
