using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Alvasoft.Utils.Activity;
using log4net;

namespace Alvasoft.DataProviderConfiguration.XmlImpl
{
    public class XmlDataProviderConfigurationImpl : 
        InitializableImpl,
        IDataProviderConfiguration
    {
        private static readonly ILog logger = LogManager.GetLogger("XmlDataProviderConfigurationImpl");

        private const string OPC_HOST = "host";
        private const string OPC_SERVER = "server";
        private const string OPC_SENSORS = "sensors";
        private const string OPC_SENSOR = "sensor";
        private const string SENSOR_NAME = "name";
        private const string CONTROL = "control";
        
        private string xmlFileName = "Settings/OpcConfiguration.xml";
        private List<IOpcSensorInfo> opcSensorInfos = new List<IOpcSensorInfo>();
        private IOpcControlBlock controlBlock = null;
        private string serverHost;
        private string serverName;
        private List<IDataProviderConfigurationListener> listeners 
            = new List<IDataProviderConfigurationListener>();
        
        public XmlDataProviderConfigurationImpl(string aFileName = null)
        {
            if (!string.IsNullOrEmpty(aFileName)) {
                xmlFileName = aFileName;
            }
        }

        public string GetHost()
        {
            return serverHost;
        }

        public string GetServer()
        {
            return serverName;
        }

        public IOpcControlBlock GetControlBlock()
        {
            return controlBlock;
        }

        public int GetOpcSensorInfoCount()
        {
            return opcSensorInfos.Count;
        }

        public void CreateOpcSensorInfo(IOpcSensorInfo aOpcSensorInfo)
        {
            if (aOpcSensorInfo == null) {
                throw new ArgumentNullException("aOpcSensorInfo");
            }

            if (opcSensorInfos.Any(s => s.GetId() == aOpcSensorInfo.GetId() ||
                                        s.GetSensorName().Equals(aOpcSensorInfo.GetSensorName()))) {
                throw new ArgumentException(
                    "Датчик с таким идентификатором или именем уже есть: "
                    + aOpcSensorInfo.GetSensorName());
            }

            opcSensorInfos.Add(aOpcSensorInfo);
            foreach (var listener in listeners) {
                listener.OnSensorCreated(this, aOpcSensorInfo.GetId());
            }
        }

        public IOpcSensorInfo ReadOpcSensorInfoById(int aId)
        {
            return opcSensorInfos.FirstOrDefault(s => s.GetId() == aId);
        }

        public IOpcSensorInfo ReadOpcSensorInfoByIndex(int aIndex)
        {
            return opcSensorInfos[aIndex];
        }

        public IOpcSensorInfo ReadOpcSensorInfoByName(string aSensorName)
        {
            return opcSensorInfos.FirstOrDefault(s => s.GetSensorName().Equals(aSensorName));
        }

        public void UpdateOpcSensorInfo(IOpcSensorInfo aOpcSensorInfo)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteOpcSensorInfo(IOpcSensorInfo aOpcSensorInfo)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeConfigurationListener(IDataProviderConfigurationListener aListener)
        {
            if (aListener != null) {
                if (!listeners.Contains(aListener)) {
                    listeners.Add(aListener);
                }
            }
        }

        public void UnsubscribeConfigurationListener(IDataProviderConfigurationListener aListener)
        {
            if (listeners.Contains(aListener)) {
                listeners.Remove(aListener);
            }
        }

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");
            var document = new XmlDocument();
            document.Load(xmlFileName);
            LoadFromXmlNode(document.DocumentElement);
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            opcSensorInfos.Clear();
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
                    case OPC_HOST:
                        serverHost = item.InnerText;
                        break;
                    case OPC_SERVER:
                        serverName = item.InnerText;
                        break;
                    case CONTROL:
                        LoadControlBlock(item);
                        break;
                    case OPC_SENSORS:
                        LoadOpcSensors(item);
                        break;
                }
            }

            logger.Info("Загрузка конфигурации из Xml завершена.");
        }

        private void LoadControlBlock(XmlNode aNode)
        {
            controlBlock = new OpcControlBlockImpl();
            var items = aNode.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    //
                }
            }            
        }

        private void LoadOpcSensors(XmlNode aNode)
        {            
            var items = aNode.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case OPC_SENSOR:
                        LoadOpcSensor(item);
                        break;
                }
            }
        }

        private void LoadOpcSensor(XmlNode aNode)
        {
            var sensorInfo = new OpcSensorInfoImpl {
                Id = opcSensorInfos.Count
            };
            var items = aNode.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case SENSOR_NAME:
                        sensorInfo.Name = item.InnerText;
                        break;
                }
            }

            if (opcSensorInfos.Any(s => s.GetSensorName().Equals(sensorInfo.Name))) {
                throw new ArgumentException("Датчик с таким именем уже существует: " + sensorInfo.Name);
            }

            opcSensorInfos.Add(sensorInfo);
        }
    }
}
