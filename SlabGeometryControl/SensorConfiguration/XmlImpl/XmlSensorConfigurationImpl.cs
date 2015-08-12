using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Alvasoft.DataEnums;
using Alvasoft.Utils.Activity;
using log4net;

namespace Alvasoft.SensorConfiguration.XmlImpl
{
    public class XmlSensorConfigurationImpl :
        InitializableImpl,
        ISensorConfiguration
    {
        private static readonly ILog logger = LogManager.GetLogger("XmlSensorConfigurationImpl");       
        private const string SENSOR_NAME = "name";
        private const string SENSOR_TYPE = "type";
        private const string SENSOR_SIDE = "side";
        private const string SENSOR_SHIFT = "shift";
        private const string SENSOR_ID = "id";

        private string xmlFileName = "Settings/SensorConfiguration.xml";
        private List<ISensorInfo> sensorInfos = new List<ISensorInfo>();
        private List<ISensorConfigurationListener> listeners = new List<ISensorConfigurationListener>();

        public XmlSensorConfigurationImpl(string aFileName = null)
        {
            if (!string.IsNullOrEmpty(aFileName)) {
                xmlFileName = aFileName;
            }
        }

        public int GetSensorInfoCount()
        {
            return sensorInfos.Count;
        }

        public void CreateSensorInfo(ISensorInfo aSensorInfo)
        {
            if (sensorInfos.Any(s => s.GetId() == aSensorInfo.GetId() ||
                                     s.GetName().Equals(aSensorInfo.GetName()))) {
                throw new ArgumentException(
                    "Датчик с таким идентификатором или именем уже существует: " + aSensorInfo.GetId());
            }
                        
            sensorInfos.Add(aSensorInfo);
            logger.Info("Создано описание нового датчика: " + aSensorInfo.GetName());
            foreach (var listener in listeners) {
                listener.OnSensorCreated(this, aSensorInfo.GetId());
            }
        }

        public ISensorInfo ReadSensorInfoByName(string aName)
        {
            if (string.IsNullOrEmpty(aName)) {
                throw new ArgumentNullException("aName");
            }

            return sensorInfos.FirstOrDefault(sensor => sensor.GetName().Equals(aName));
        }

        public ISensorInfo ReadSensorInfoById(int aId)
        {
            return sensorInfos.FirstOrDefault(sensor => sensor.GetId() == aId);
        }

        public ISensorInfo ReadSensorInfoByIndex(int aIndex)
        {
            if (aIndex < 0 || aIndex >= sensorInfos.Count) {
                throw new IndexOutOfRangeException("Индекс датчика находится вне диапазона допустимых значений.");
            }

            return sensorInfos[aIndex];
        }

        public void UpdateSensorInfo(ISensorInfo aSensorInfo)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteSensorInfo(ISensorInfo aSensorInfo)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeConfigurationListener(ISensorConfigurationListener aListener)
        {
            if (aListener == null) {
                throw new ArgumentNullException("aListener");
            }

            if (!listeners.Contains(aListener)) {
                listeners.Add(aListener);
            }
        }

        public void UnsubscribeConfigurationListener(ISensorConfigurationListener aListener)
        {
            if (aListener != null) {
                if (listeners.Contains(aListener)) {
                    listeners.Remove(aListener);
                }
            }
        }

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");
            var document = new XmlDocument();
            document.Load(xmlFileName);
            LoadFromXmlNode(document.DocumentElement);
            logger.Info("Инциализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            sensorInfos.Clear();
            listeners.Clear();
            logger.Info("Деинициализация завершена.");
        }

        private void LoadFromXmlNode(XmlNode aRoot)
        {
            logger.Info("Загрузка конфигурации из Xml...");
            if (aRoot == null) {
                throw new ArgumentNullException("aRoot");
            }

            var items = aRoot.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case "sensor":
                        var sensor = LoadSensor(item);
                        sensorInfos.Add(sensor);
                        break;
                }
            }
            logger.Info("Загрузка конфигурации из Xml завершена.");
        }

        private ISensorInfo LoadSensor(XmlNode aSensorNode)
        {
            var sensorId = -1;
            var sensorName = "";
            var sensorType = SensorType.POSITION;
            var sensorSide = SensorSide.TOP;
            var shift = 0.0;
            var items = aSensorNode.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case SENSOR_ID:
                        sensorId = Convert.ToInt32(item.InnerText);
                        break;
                    case SENSOR_NAME:
                        sensorName = item.InnerText;
                        break;
                    case SENSOR_TYPE:
                        sensorType = (SensorType) Enum.Parse(typeof(SensorType), 
                            item.InnerText.ToUpper());
                        break;
                    case SENSOR_SIDE:
                        sensorSide = (SensorSide) Enum.Parse(typeof(SensorSide), 
                            item.InnerText.ToUpper());
                        break;
                    case SENSOR_SHIFT:
                        shift = Convert.ToDouble(item.InnerText);
                        break;
                }
            }

            return new SensorInfoImpl(sensorId, sensorName, sensorType, sensorSide, shift);
        }
    }
}
