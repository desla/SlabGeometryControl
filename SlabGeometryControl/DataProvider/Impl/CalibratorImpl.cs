using System;
using System.Collections.Generic;
using System.Xml;
using Alvasoft.Utils.Activity;
using log4net;

namespace Alvasoft.DataProvider.Impl
{
    public class CalibratorImpl : 
        InitializableImpl,
        ICalibrator
    {
        private static readonly ILog logger = LogManager.GetLogger("CalibratorImpl");

        private const string XML_FILE_NAME = "Settings/CalibratorConfiguration.xml";
        private const string CALIBRATOR = "calibrator";
        private const string SENSOR = "sensor";
        private const string SENSOR_ID = "sensorId";

        private XmlDocument document;
        private Dictionary<int, double> calibratedValues = new Dictionary<int, double>(); 

        public bool IsCalibratedState()
        {
            return IsInitialized();
        }

        public double GetCalibratedValue(int aSensorId)
        {
            if (!IsInitialized()) {
                throw new ArgumentException(
                    "Калибратор не инициализирован, но произошел запрос калибровочного значения.");
            }

            if (!calibratedValues.ContainsKey(aSensorId)) {
                return -1;
            }

            return calibratedValues[aSensorId];
        }

        public void SetCalibratedValue(int aSensorId, double aCalibratedValue)
        {
            calibratedValues[aSensorId] = aCalibratedValue;
            try {
                document.Load(XML_FILE_NAME);
                var root = document.DocumentElement;
                var items = root.ChildNodes;
                for (var i = 0; i < items.Count; ++i) {
                    var item = items[i];
                    var sensorId = Convert.ToInt32(item.Attributes[SENSOR_ID].Value);
                    if (aSensorId == sensorId) {
                        item.InnerText = aCalibratedValue.ToString();
                        document.Save(XML_FILE_NAME);
                        return;
                    }
                }

                var element = document.CreateElement(SENSOR);
                element.SetAttribute(SENSOR_ID, aSensorId.ToString());
                element.InnerText = aCalibratedValue.ToString();
                root.AppendChild(element);
                document.Save(XML_FILE_NAME);
            }
            catch (Exception ex) {
                logger.Error("Ошибка при сохранении калибровочного значения: " + ex.Message);
            }
        }

        protected override void DoInitialize()
        {            
            logger.Info("Инициализация...");
            try {
                document = new XmlDocument();
                document.Load(XML_FILE_NAME);
                var items = document.DocumentElement.ChildNodes;                
                for (var i = 0; i < items.Count; ++i) {
                    var item = items[i];
                    var sensorId = Convert.ToInt32(item.Attributes[SENSOR_ID].Value);
                    var calibratedValue = Convert.ToDouble(item.InnerText);
                    calibratedValues[sensorId] = calibratedValue;
                }
            }
            catch (Exception ex) {
                logger.Error("Ошибка калибратора: " + ex.Message);
            }
            logger.Info("Инициализация завершена.");
        }
    }
}
