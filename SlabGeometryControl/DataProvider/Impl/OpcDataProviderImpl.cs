using System;
using System.Collections.Generic;
using Alvasoft.DataProviderConfiguration;
using Alvasoft.DataEnums;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;
using Alvasoft.Utils.Activity;

namespace Alvasoft.DataProvider.Impl
{
    public class OpcDataProviderImpl : 
        InitializableImpl,
        IDataProvider,
        ICalibrator,
        IDataProviderConfigurationListener,
        ISensorConfigurationListener
    {
        private ISensorValueContainer valueContainer;
        private IDataProviderConfiguration opcConfiguration;
        private ISensorConfiguration sensorConfiguration;
        private List<IDataProviderListener> listeners = new List<IDataProviderListener>();

        public SystemState GetCurrentSystemState()
        {
            return SystemState.WAITING;
        }

        public void SetSensorValueContainer(ISensorValueContainer aSensorValueContainer)
        {
            if (aSensorValueContainer == null) {
                throw new ArgumentNullException("aSensorValueContainer");
            }

            valueContainer = aSensorValueContainer;
        }

        public void SetDataProviderConfiguration(IDataProviderConfiguration aDataProviderConfiguration)
        {
            if (aDataProviderConfiguration == null) {
                throw new ArgumentNullException("aDataProviderConfiguration");
            }

            opcConfiguration = aDataProviderConfiguration;
        }

        public void SetSensorConfiguration(ISensorConfiguration aSensorConfiguration)
        {
            if (aSensorConfiguration == null) {
                throw new ArgumentNullException("aSensorConfiguration");
            }

            sensorConfiguration = aSensorConfiguration;
        }

        public void SubscribeDataProviderListener(IDataProviderListener aListener)
        {
            if (aListener != null) {
                listeners.Add(aListener);
            }
        }

        public void UnsubscribeDataProviderListener(IDataProviderListener aListener)
        {
            if (aListener != null && listeners.Contains(aListener)) {
                listeners.Remove(aListener);
            }
        }

        public bool IsConnected()
        {
            throw new System.NotImplementedException();
        }

        public bool IsSensorActive(int aSensorId)
        {
            return false;
        }

        public double GetSensorCurrentValue(int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsCalibratedState()
        {
            throw new System.NotImplementedException();
        }

        public double GetCalibratedValueBySensorId(int aSensorId)
        {
            throw new System.NotImplementedException();
        }

        protected override void DoInitialize()
        {            
        }

        protected override void DoUninitialize()
        {            
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
