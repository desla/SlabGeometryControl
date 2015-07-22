using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Xml;
using Alvasoft.DataEnums;
using Alvasoft.DataProviderConfiguration;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;
using Alvasoft.Utils.Activity;
using NHibernate.Mapping;
using Timer = System.Timers.Timer;

namespace Alvasoft.DataProvider.Impl.Emulator
{
    public class EmulatorDataProvider :
        InitializableImpl,
        IDataProvider,
        ICalibrator,
        IDataProviderConfigurationListener,
        ISensorConfigurationListener
    {
        private const string NODE_SENSORS = "sensors";
        private const string NODE_INGOT = "ingot";
        private const string NODE_RATTLE = "rattle";
        private const string NODE_SHIFT = "shift";
        private Random rnd = new Random();
        /// <summary>
        /// Определяет расстояние от слитка до сканирующей рамки перед моделированием.
        /// Измеряется в миллиметрах.
        /// </summary>
        private const double prePosition = 0;
        /// <summary>
        /// Определяет расстояние от рамки до сслитка после сканирования.
        /// </summary>
        private const double postPosition = 0;
        /// Шаг измерения.
        /// В миллиметрах.
        /// </summary>
        private const double step = 7.5;
        /// <summary>
        /// Дребезжание слитка во время движения.
        /// </summary>
        private double[] rattle;
        /// <summary>
        /// Предел дребезжания.
        /// </summary>
        private double rattleMax;
        /// <summary>
        /// Величина подпрыгивания слитка посередине.
        /// </summary>
        private double bounced = 0;
        /// <summary>
        /// Определяет смещение центра слитка относительно центра рамки.
        /// </summary>
        private double shift;
        /// <summary>
        /// Слиток.
        /// </summary>
        private Ingot ingot;
        private Sensor[] sensors;

        private Timer scanTimer = null;
        private SystemState currentState = SystemState.WAITING;
        private ISensorValueContainer valueContainer = null;
        private List<IDataProviderListener> listeners = new List<IDataProviderListener>();
        private SensorValue[][] values = null;
        private int position = -1;

        public EmulatorDataProvider()
        {
            LoadFromXml("Settings/Emulator.xml");

            scanTimer = new Timer();
            scanTimer.Elapsed += StartScan;
            scanTimer.Interval = 10000;
            scanTimer.Start();
        }

        private void StartScan(object sender, ElapsedEventArgs e)
        {
            scanTimer.Stop();
            if (currentState == SystemState.SCANNING) {                
                return;
            }
            currentState = SystemState.SCANNING;
            Init();            
            position = 0;
            foreach (var listener in listeners) {
                listener.OnStateChanged(this);
            }
            
            for (var i = 0; i < values[0].Length; ++i) {
                var nowTime = DateTime.Now.ToBinary();
                for (var j = 0; j < sensors.Length; ++j) {
                    valueContainer.AddSensorValue(j, values[j][i].Value, nowTime);
                }
                Thread.Sleep(15000 / values[0].Length);                
                position = i;
            }

            currentState = SystemState.WAITING;
            position = -1;
            foreach (var listener in listeners) {
                listener.OnStateChanged(this);
            }
            scanTimer.Start();
        }

        private void LoadFromXml(string aConfigFileName)
        {
            var document = new XmlDocument();
            document.Load(aConfigFileName);
            var aRoot = document.DocumentElement;

            var items = aRoot.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case NODE_SENSORS:
                        var sensorsNodes = item.ChildNodes;
                        sensors = new Sensor[sensorsNodes.Count];
                        for (var j = 0; j < sensorsNodes.Count; ++j) {
                            var configuration = new SensorConfiguration(sensorsNodes[j]);
                            sensors[j] = new Sensor();
                            sensors[j].SetConfiguration(configuration);
                        }
                        break;
                    case NODE_INGOT:
                        ingot = new Ingot();
                        ingot.LoadFromXmlNode(item);
                        break;
                    case NODE_RATTLE:
                        rattleMax = Convert.ToDouble(item.InnerText);
                        break;
                    case NODE_SHIFT:
                        shift = Convert.ToDouble(item.InnerText);
                        break;
                } // switch
            } // for
        }

        public SystemState GetCurrentSystemState()
        {
            return currentState;
        }

        public void SetSensorValueContainer(ISensorValueContainer aSensorValueContainer)
        {
            valueContainer = aSensorValueContainer;
        }

        public void SetDataProviderConfiguration(IDataProviderConfiguration aDataProviderConfiguration)
        {
            //throw new System.NotImplementedException();
        }

        public void SetSensorConfiguration(ISensorConfiguration aSensorConfiguration)
        {
            //throw new System.NotImplementedException();
        }

        public void SubscribeDataProviderListener(IDataProviderListener aListener)
        {
            listeners.Add(aListener);
        }

        public void UnsubscribeDataProviderListener(IDataProviderListener aListener)
        {
            listeners.Remove(aListener);
        }

        public bool IsConnected()
        {
            return true;
        }

        public bool IsSensorActive(int aSensorId)
        {
            return true;
        }

        public double GetSensorCurrentValue(int aSensorId)
        {
            if (GetCurrentSystemState() == SystemState.WAITING) {
                return GetCalibratedValueBySensorId(aSensorId);
            }

            return values[aSensorId][position].Value;
        }

        public bool IsCalibratedState()
        {
            return true;
        }

        public double GetCalibratedValueBySensorId(int aSensorId)
        {
            return 2000;
        }

        private void Init()
        {
            // создаем дребезжание слитка во время движения.
            // дребезжание одинаково будет отражаться на верхней и нижней сторонах слитка.
            // поэтому запоминаем дребезжание во времени.
            rattle = new double[(int)(1 + (ingot.Length + prePosition + postPosition) / step)];
            var rnd = new Random();
            for (var i = 0; i < rattle.Length; ++i) {
                rattle[i] = rnd.Next((int)(-rattleMax * 1000), (int)(rattleMax * 1000)) / 1000.0;

                if (i > rattle.Length / 2) {
                    rattle[i] += bounced;
                }
            }

            // Генерируем значения.
            values = new SensorValue[sensors.Length][];
            for (var i = 0; i < sensors.Length; ++i) {
                values[i] = GenerateValues(sensors[i]);
            }
        }

        private SensorValue[] GenerateValues(Sensor aSensor)
        {
            if (aSensor.Configuration.SideType == SensorType.POSITION) {
                return GeneratePositionValues(aSensor);
            }
            else {
                return GenerateSideValues(aSensor);
            }
        }

        private SensorValue[] GenerateSideValues(Sensor aSensor)
        {
            switch (aSensor.Configuration.SideType) {
                case SensorType.TOP:
                    return GenerateValuesByParameters(aSensor.Configuration.Horizont,
                                                      aSensor.Configuration.Remove,
                                                      ingot.Height,
                                                      1,
                                                      -1,
                                                      0,
                                                      0);
                case SensorType.BOTTOM:
                    return GenerateValuesByParameters(aSensor.Configuration.Horizont,
                                                      aSensor.Configuration.Remove,
                                                      ingot.Height,
                                                      -1,
                                                      1,
                                                      0,
                                                      0);
                case SensorType.LEFT:
                case SensorType.RIGHT:
                    return GenerateValuesByParameters(aSensor.Configuration.Horizont,
                                                      aSensor.Configuration.Remove,
                                                      ingot.Width,
                                                      0,
                                                      0,
                                                      0,
                                                      0);
            }

            return null;
        }

        /// <summary>
        /// Генерирует данные на основле параметром.
        /// </summary>
        /// <param name="aHorizont">Горизонт для датчика.</param>
        /// <param name="aSensorRemove">Удаление датчика от центра рамки.</param>
        /// <param name="aSideValue">Параметр слитка, влиящий на положение датчика.</param>
        /// <param name="aRattleFactor">Множитель дребезжания.</param>
        /// <param name="aShiftFactor">Множитель смещения.</param>
        /// <param name="aSag">Искривление поверхности.</param>
        /// <param name="aSensorShift">Смещение датчика от центра стороны.</param>
        /// <returns></returns>
        private SensorValue[] GenerateValuesByParameters(
            double aHorizont,
            double aSensorRemove,
            double aSideValue,
            double aRattleFactor,
            double aShiftFactor,
            double aSag,
            double aSensorShift)
        {
            //TODO: реализовать искривление.
            var values = new SensorValue[(int)(1 + (ingot.Length + prePosition + postPosition) / step)];
            var overHorizont = aHorizont + 100;
            var currentPosition = .0;
            var currentTime = 0;
            for (var i = 0; i < values.Length; ++i) {
                if (currentPosition < prePosition || currentPosition > prePosition + ingot.Length) {
                    values[i] = new SensorValue(overHorizont, currentTime);
                }
                else {
                    var value = aSensorRemove; // первоначально - удаление датчика от центра рамки.
                    value = value - aSideValue / 2; // учитываем высоту или ширину слитка слитка.
                    value = value + aRattleFactor * rattle[i]; // дребезжание слитка в пределах 2 мм.   
                    value = value + aShiftFactor * shift; // учитываем смещение слитка относительно центра рамки.           
                    value = value +
                        rnd.Next((int)(-ingot.Seediness * 1000),
                                 (int)(ingot.Seediness * 1000))
                        / 1000.0; // шероховатость.
                    var sensorValue = new SensorValue(value, currentTime);
                    values[i] = sensorValue;
                }

                currentTime++;
                currentPosition += step;
            }

            return values.ToArray();
        }

        private SensorValue[] GeneratePositionValues(Sensor aSensor)
        {
            var values = new SensorValue[(int)(1 + (ingot.Length + prePosition + postPosition) / step)];
            var currentPosition = .0;
            var currentTime = 0;
            for (var i = 0; i < values.Length; ++i) {
                var value = currentPosition; // обкновенное линейное движение.
                values[i] = new SensorValue(value, currentTime);
                currentTime++;
                currentPosition += step;
            }

            return values;
        }

        public void OnSensorCreated(IDataProviderConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorUpdated(IDataProviderConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorDeleted(IDataProviderConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorCreated(ISensorConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorUpdated(ISensorConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorDeleted(ISensorConfiguration aConfiguration, int aSensorId)
        {
            throw new System.NotImplementedException();
        }
    }
}
