using System;
using System.Collections.Generic;
using Alvasoft.DataEnums;
using Alvasoft.DataProvider;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;
using Alvasoft.SensorValueContainer.Impl;
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

        private ISensorValueInfo[] positionValues;

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

        public ISlabModel BuildSlabModel(bool aIsUseFilters)
        {
            if (container == null || container.IsEmpty()) {
                throw new ArgumentException("Контейнер не содержит данных для построения модели слитка.");
            }

            if (configuration == null) {
                throw new ArgumentException("Конфигурация не установлена.");
            }

            var slab = new SlabModelImpl();

            MakePositionValues();

            BuildTopLines(slab);
            BuildBottomLines(slab);
            BuildLeftLines(slab);
            BuildRightLines(slab);

            if (aIsUseFilters) {
                SplashFilter.Filter(slab);
                PickFilter.Filter(slab);
                BumpFilter.Filter(slab);
                AverageFilter.Filter(slab);
                //RotateFilter.Filter(slab);
            }            

            BuildLimits(slab);

            return slab;
        }

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");            
            for (var i = 0; i < configuration.GetSensorInfoCount(); ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);                
            }
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");            
            logger.Info("Деинициализация завершена.");
        }

        public void OnSensorCreated(ISensorConfiguration aConfiguration, int aSensorId)
        {            
            throw new NotImplementedException();
        }

        public void OnSensorUpdated(ISensorConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorDeleted(ISensorConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        private void MakePositionValues() {
            var positionSensor = GetPositionSensor();
            positionValues = container.GetSensorValuesBySensorId(positionSensor.GetId());
            IncrementOrderConverter.Convert(ref positionValues);
            PickPositionFilter.Filter(ref positionValues);
            DoublePositionFilter.Filter(ref positionValues);
        }

        private void BuildLimits(SlabModelImpl aSlab) {
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
                }
                aSlab.LengthLimit = Math.Max(aSlab.LengthLimit, values[values.Length - 1].Z - values[0].Z);
            }

            sensorCount = aSlab.BottomLines.Length;
            for (var i = 0; i < sensorCount; ++i) {
                var values = aSlab.BottomLines[i];
                for (var j = 0; j < values.Length; ++j) {
                    aSlab.BottomLimit = Math.Min(aSlab.BottomLimit, values[j].Y);
                }
            }

            sensorCount = aSlab.LeftLines.Length;
            for (var i = 0; i < sensorCount; ++i) {
                var values = aSlab.LeftLines[i];
                for (var j = 0; j < values.Length; ++j) {
                    aSlab.LeftLimit = Math.Min(aSlab.LeftLimit, values[j].X);
                }
            }

            sensorCount = aSlab.RightLines.Length;
            for (var i = 0; i < sensorCount; ++i) {
                var values = aSlab.RightLines[i];
                for (var j = 0; j < values.Length; ++j) {
                    aSlab.RightLimit = Math.Max(aSlab.RightLimit, values[j].X);
                }
            }
        }

        private void BuildTopLines(SlabModelImpl aSlab)
        {            
            var sensors = GetProximitySensors(SensorSide.TOP);

            // Сортируем по увеличению отступа - слева на право.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);
            
            aSlab.TopLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                var sensorValues = container.GetSensorValuesBySensorId(sensor.GetId());
                RemoveFilteredValues(ref sensorValues);
                aSlab.TopLines[i] = BuildTopValues(
                    positionValues, 
                    sensorValues,
                    sensor.GetShift(),
                    calibrator.GetCalibratedValue(sensor.GetId()) / 2.0);

                MoveToZeroOnZ(aSlab.TopLines[i]);
            }
        }        

        private void BuildBottomLines(SlabModelImpl aSlab)
        {
            var sensors = GetProximitySensors(SensorSide.BOTTOM);

            // Сортируем по увеличению отступа - слева на право.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);

            aSlab.BottomLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                var sensorValues = container.GetSensorValuesBySensorId(sensor.GetId());
                RemoveFilteredValues(ref sensorValues);
                aSlab.BottomLines[i] = BuildBottomValues(
                    positionValues,
                    sensorValues,
                    sensor.GetShift(),
                    calibrator.GetCalibratedValue(sensor.GetId()) / 2.0);

                MoveToZeroOnZ(aSlab.BottomLines[i]);
            }
        }

        private void BuildLeftLines(SlabModelImpl aSlab)
        {
            var sensors = GetProximitySensors(SensorSide.LEFT);

            // Сортируем по увеличению отступа - снизу вверх.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);
            
            aSlab.LeftLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                var sensorValues = container.GetSensorValuesBySensorId(sensor.GetId());
                RemoveFilteredValues(ref sensorValues);
                aSlab.LeftLines[i] = BuildLeftValues(
                    positionValues,
                    sensorValues,
                    sensor.GetShift(),
                    calibrator.GetCalibratedValue(sensor.GetId()) / 2.0);

                MoveToZeroOnZ(aSlab.LeftLines[i]);
            }
        }

        private void BuildRightLines(SlabModelImpl aSlab)
        {
            var sensors = GetProximitySensors(SensorSide.RIGHT);

            // Сортируем по увеличению отступа - снизу вверх.
            sensors.Sort((a, b) => a.GetShift() < b.GetShift() ? 1 : 0);
            
            aSlab.RightLines = new Point3D[sensors.Count][];
            for (var i = 0; i < sensors.Count; ++i) {
                var sensor = sensors[i];
                var sensorValues = container.GetSensorValuesBySensorId(sensor.GetId());
                RemoveFilteredValues(ref sensorValues);
                aSlab.RightLines[i] = BuildRightValues(
                    positionValues,
                    sensorValues,
                    sensor.GetShift(),
                    calibrator.GetCalibratedValue(sensor.GetId()) / 2.0);

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
            //var leftIndex = 0;
            //var rightIndex = aPositions.Length - 1;
            //while (leftIndex < rightIndex) {
            //    var medium = leftIndex + (rightIndex - leftIndex) / 2;
            //    if (aPositions[medium].GetTime() == aTime) {
            //        return aPositions[medium].GetValue();
            //    }
            //    if (aPositions[medium].GetTime() < aTime) {
            //        rightIndex = medium - 1;
            //    } else {
            //        leftIndex = medium + 1;
            //    }
            //}

            for (var i = 0; i < aPositions.Length; ++i) {
                if (aPositions[i].GetTime() == aTime) {
                    return aPositions[i].GetValue();
                }
            }

            throw new ArgumentException("Не найдена точка для указанног времени.");
        }

        private ISensorInfo GetPositionSensor() {
            ISensorInfo positionSensor = null;            
            var sensorCount = configuration.GetSensorInfoCount();
            for (var i = 0; i < sensorCount; ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);
                if (sensorInfo.GetSensorType() == SensorType.POSITION) {
                    positionSensor = sensorInfo;
                }
            }

            if (positionSensor == null) {
                throw new ArgumentException("Датчик положения не найден.");
            }

            return positionSensor;
        }

        private List<ISensorInfo> GetProximitySensors(SensorSide aSensorSide) {
            var sensors = new List<ISensorInfo>();
            var sensorCount = configuration.GetSensorInfoCount();
            for (var i = 0; i < sensorCount; ++i) {
                var sensorInfo = configuration.ReadSensorInfoByIndex(i);
                if (sensorInfo.GetSensorSide() == aSensorSide &&
                    sensorInfo.GetSensorType() == SensorType.PROXIMITY) {
                    sensors.Add(sensorInfo);
                }
            }

            return sensors;
        }

        private void MoveToZeroOnZ(Point3D[] aValues)
        {
            var difference = aValues[0].Z;
            
            for (var i = 0; i < aValues.Length; ++i) {
                aValues[i].Z -= difference;
            }
        }        

        /// <summary>
        /// Отфильтровывает только те показания, для которых есть данные датчика положения.
        /// </summary>
        /// <param name="sensorValues"></param>
        private void RemoveFilteredValues(ref ISensorValueInfo[] aSensorValues) {
            var newSensorValues = new List<ISensorValueInfo>();
            for (var i = 0; i < aSensorValues.Length; ++i) {
                if (IsContainPositionByTime(aSensorValues[i].GetTime())) {
                    newSensorValues.Add(aSensorValues[i]);
                }
            }

            aSensorValues = newSensorValues.ToArray();
        }

        private bool IsContainPositionByTime(long aTime) {            
            for (var i = 0; i < positionValues.Length; ++i) {
                if (positionValues[i].GetTime() == aTime) {
                    return true;
                }
            }

            return false;
        }
    }
}
