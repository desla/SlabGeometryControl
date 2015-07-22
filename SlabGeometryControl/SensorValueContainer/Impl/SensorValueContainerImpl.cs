using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Alvasoft.SensorValueContainer.Impl
{
    public class SensorValueContainerImpl
        : ISensorValueContainer
    {
        private static readonly ILog logger = LogManager.GetLogger("SensorValueContainerImpl");

        private Dictionary<int, List<ISensorValueInfo>> container = new Dictionary<int,List<ISensorValueInfo>>();        
        private List<ISensorValueContainerListener> listeners = new List<ISensorValueContainerListener>();
        private object accessLock = new object();

        public void AddSensorValue(int aSensorId, double aValue, long aTime)
        {            
            var sensorValue = new SensorValueInfoImpl(aSensorId, aValue, aTime);
            logger.Debug("Добавить новое значение для датчика: " + sensorValue.ToString());
            lock (accessLock) {
                if (!container.ContainsKey(aSensorId)) {
                    container[aSensorId] = new List<ISensorValueInfo>();
                }
                container[aSensorId].Add(sensorValue);
            }

            AlertListeners();
        }

        public ISensorValueInfo[] GetAllValues()
        {            
            var sensorValueList = new List<ISensorValueInfo>();
            lock (accessLock) {
                foreach (var sensorId in container.Keys) {
                    var values = container[sensorId];
                    sensorValueList.AddRange(values);
                }
            }

            return sensorValueList.ToArray();
        }

        public ISensorValueInfo[] GetSensorValuesBySensorId(int aSensorId)
        {
            if (!container.ContainsKey(aSensorId)) {
                throw new ArgumentException("Данных для датчика с таким идентификатором не найдено.");                
            }

            return container[aSensorId].ToArray();
        }

        public void Clear()
        {
            logger.Debug("Запрос на очистку накопителя.");
            foreach (var valuesList in container.Values) {
                valuesList.Clear();
            }
        }

        public bool IsEmpty()
        {
            lock (accessLock) {
                return container.Values.All(v => v.Count == 0);
            }            
        }

        public void SunbscribeContainerListener(ISensorValueContainerListener aListener)
        {
            if (aListener != null) {
                if (!listeners.Contains(aListener)) {
                    listeners.Add(aListener);
                }                
            }
        }

        public void UnsunbscribeContainerListener(ISensorValueContainerListener aListener)
        {
            if (aListener != null) {
                if (listeners.Contains(aListener)) {
                    listeners.Remove(aListener);
                }                
            }
        }

        private void AlertListeners()
        {
            foreach (var listener in listeners) {
                listener.OnDataReceived(this);
            }
        }
    }
}
