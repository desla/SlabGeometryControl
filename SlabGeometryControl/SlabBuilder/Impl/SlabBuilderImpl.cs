using System;
using System.Collections.Generic;
using Alvasoft.DataEnums;
using Alvasoft.DataProvider;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;
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

        private bool isCalibrated = false;
        private Dictionary<int, double> calibratedValue;


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

            BuildTopLines(slab);
            BuildBottomLines(slab);
            BuildLeftLines(slab);
            BuildRightLines(slab);            

            BuildLimits(slab);

            //RattleFilter(slab);
            //BumpingFilter(slab);

            return slab;
        }

        private void BuildLimits(SlabModelImpl aSlab)
        {
            aSlab.TopLimit = double.MinValue;
            aSlab.BottomLimit = double.MaxValue;
            aSlab.LeftLimit = double.MaxValue;
            aSlab.RightLimit = double.MinValue;
            aSlab.LengthLimit = double.MinValue;
            var sensorCount = aSlab.TopLines.Length;
            for (var i = 0; i < sensorCount; ++i) {
                var values = aSlab.TopLines[i];
                for (var j = 0; j < values.Length; ++j) {
                    aSlab.TopLimit = Math.Max(aSlab.TopLimit, values[j].Y);
                    aSlab.LengthLimit = Math.Max(aSlab.LengthLimit, values[j].Z);
                }
            }

            sensorCount = aSlab.BottomLines.Length;
            for (var i = 0; i < sensorCount; ++i) {
                var values = aSlab.BottomLines[i];
                for (var j = 0; j < values.Length; ++j) {
                    aSlab.BottomLimit = Math.Min(aSlab.BottomLimit, values[j].Y);
                    aSlab.LengthLimit = Math.Max(aSlab.LengthLimit, values[j].Z);
                }
            }

            sensorCount = aSlab.LeftLines.Length;
            for (var i = 0; i < sensorCount; ++i) {
                var values = aSlab.LeftLines[i];
                for (var j = 0; j < values.Length; ++j) {
                    aSlab.LeftLimit = Math.Min(aSlab.LeftLimit, values[j].X);
                    aSlab.LengthLimit = Math.Max(aSlab.LengthLimit, values[j].Z);
                }
            }

            sensorCount = aSlab.RightLines.Length;
            for (var i = 0; i < sensorCount; ++i) {
                var values = aSlab.RightLines[i];
                for (var j = 0; j < values.Length; ++j) {
                    aSlab.RightLimit = Math.Max(aSlab.RightLimit, values[j].X);
                    aSlab.LengthLimit = Math.Max(aSlab.LengthLimit, values[j].Z);
                }
            }            
        }

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");
            calibratedValue = new Dictionary<int, double>();
            for (var i = 0; i < configuration.GetSensorInfoCount(); ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);
                calibratedValue[sensorInfo.GetId()] = 
                    calibrator.GetCalibratedValueBySensorId(sensorInfo.GetId());
            }
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            calibratedValue.Clear();
            logger.Info("Деинициализация завершена.");
        }

        public void OnSensorCreated(ISensorConfiguration aConfiguration, int aSensorId)
        {
            isCalibrated = false;
            if (calibrator == null) {
                throw new ArgumentException("Нет возможности получить значения калибратора.");
            }
            calibratedValue[aSensorId] = calibrator.GetCalibratedValueBySensorId(aSensorId);
            isCalibrated = true;
        }

        public void OnSensorUpdated(ISensorConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorDeleted(ISensorConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }        

        private void BuildTopLines(SlabModelImpl aSlab)
        {
            ISensorInfo positionSensor = null;
            var sensors = new List<ISensorInfo>();
            var sensorCount = configuration.GetSensorInfoCount();
            for (var i = 0; i < sensorCount; ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);                
                if (sensorInfo.GetSensorSide() == SensorSide.TOP && 
                    sensorInfo.GetSensorType() == SensorType.PROXIMITY) {
                    sensors.Add(sensorInfo);
                } 
                else if (sensorInfo.GetSensorType() == SensorType.POSITION) {
                    positionSensor = sensorInfo;
                }
            }

            if (positionSensor == null) {
                throw new ArgumentException("Датчик положения не найден.");
            }

            // Сортируем по увеличению отступа - слева на право.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);

            var positionValues = container.GetSensorValuesBySensorId(positionSensor.GetId());
            aSlab.TopLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                aSlab.TopLines[i] = BuildTopValues(
                    positionValues, 
                    container.GetSensorValuesBySensorId(sensor.GetId()),
                    sensor.GetShift(),
                    calibratedValue[sensor.GetId()] / 2.0);

                MoveToZeroOnZ(aSlab.TopLines[i]);
            }
        }

        private void BuildBottomLines(SlabModelImpl aSlab)
        {
            ISensorInfo positionSensor = null;
            var sensors = new List<ISensorInfo>();
            var sensorCount = configuration.GetSensorInfoCount();
            for (var i = 0; i < sensorCount; ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);
                if (sensorInfo.GetSensorSide() == SensorSide.BOTTOM) {
                    sensors.Add(sensorInfo);
                }

                if (sensorInfo.GetSensorType() == SensorType.POSITION) {
                    positionSensor = sensorInfo;
                }
            }

            if (positionSensor == null) {
                throw new ArgumentException("Датчик положения не найден.");
            }

            // Сортируем по увеличению отступа - слева на право.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);

            var positionValues = container.GetSensorValuesBySensorId(positionSensor.GetId());
            aSlab.BottomLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                aSlab.BottomLines[i] = BuildBottomValues(
                    positionValues,
                    container.GetSensorValuesBySensorId(sensor.GetId()),
                    sensor.GetShift(),
                    calibratedValue[sensor.GetId()] / 2.0);

                MoveToZeroOnZ(aSlab.BottomLines[i]);
            }
        }

        private void BuildLeftLines(SlabModelImpl aSlab)
        {
            ISensorInfo positionSensor = null;
            var sensors = new List<ISensorInfo>();
            var sensorCount = configuration.GetSensorInfoCount();
            for (var i = 0; i < sensorCount; ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);
                if (sensorInfo.GetSensorSide() == SensorSide.LEFT) {
                    sensors.Add(sensorInfo);
                }

                if (sensorInfo.GetSensorType() == SensorType.POSITION) {
                    positionSensor = sensorInfo;
                }
            }

            if (positionSensor == null) {
                throw new ArgumentException("Датчик положения не найден.");
            }

            // Сортируем по увеличению отступа - снизу вверх.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);

            var positionValues = container.GetSensorValuesBySensorId(positionSensor.GetId());
            aSlab.LeftLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                aSlab.LeftLines[i] = BuildLeftValues(
                    positionValues,
                    container.GetSensorValuesBySensorId(sensor.GetId()),
                    sensor.GetShift(),
                    calibratedValue[sensor.GetId()] / 2.0);

                MoveToZeroOnZ(aSlab.LeftLines[i]);
            }
        }

        private void BuildRightLines(SlabModelImpl aSlab)
        {
            ISensorInfo positionSensor = null;
            var sensors = new List<ISensorInfo>();
            var sensorCount = configuration.GetSensorInfoCount();
            for (var i = 0; i < sensorCount; ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);
                if (sensorInfo.GetSensorSide() == SensorSide.RIGHT) {
                    sensors.Add(sensorInfo);
                }

                if (sensorInfo.GetSensorType() == SensorType.POSITION) {
                    positionSensor = sensorInfo;
                }
            }

            if (positionSensor == null) {
                throw new ArgumentException("Датчик положения не найден.");
            }

            // Сортируем по увеличению отступа - снизу вверх.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);

            var positionValues = container.GetSensorValuesBySensorId(positionSensor.GetId());
            aSlab.RightLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                aSlab.RightLines[i] = BuildRightValues(
                    positionValues,
                    container.GetSensorValuesBySensorId(sensor.GetId()),
                    sensor.GetShift(),
                    calibratedValue[sensor.GetId()] / 2.0);

                MoveToZeroOnZ(aSlab.RightLines[i]);
            }
        }

        private Point3D[] BuildRightValues(
            ISensorValueInfo[] aPositions,
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance)
        {
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

        private Point3D[] BuildLeftValues(
            ISensorValueInfo[] aPositions,
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance)
        {
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

        private Point3D[] BuildBottomValues(
            ISensorValueInfo[] aPositions, 
            ISensorValueInfo[] aSensorValues, 
            double aShift, 
            double aCenterDistance)
        {
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

        private Point3D[] BuildTopValues(
            ISensorValueInfo[] aPositions, 
            ISensorValueInfo[] aSensorValues,
            double aShift,
            double aCenterDistance)
        {
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

        private double GetPositionByTime(ISensorValueInfo[] aPositions, long aTime)
        {
            if (aPositions[0].GetTime() >= aTime) {
                return aPositions[0].GetValue();
            }

            if (aPositions[aPositions.Length - 1].GetTime() <= aTime) {
                return aPositions[aPositions.Length - 1].GetValue();
            }

            // TODO: Сделать бинарнй поиск.
            for (var i = 0; i < aPositions.Length - 1; ++i) {
                if (aPositions[i].GetTime() <= aTime && aPositions[i + 1].GetTime() > aTime) {
                    var difference = (aTime - aPositions[i].GetTime()) /
                                     (double) (aPositions[i + 1].GetTime() - aPositions[i].GetTime());
                    return aPositions[i].GetValue() +
                           difference * (aPositions[i + 1].GetValue() - aPositions[i].GetValue());
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
