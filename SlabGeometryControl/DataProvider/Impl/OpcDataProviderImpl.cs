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
        public SystemState GetCurrentSystemState()
        {
            return SystemState.WAITING;
        }

        public void SetSensorValueContainer(ISensorValueContainer aSensorValueContainer)
        {
            //throw new System.NotImplementedException();
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
            //throw new System.NotImplementedException();
        }

        public void UnsubscribeDataProviderListener(IDataProviderListener aListener)
        {
            //throw new System.NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new System.NotImplementedException();
        }

        public bool IsSensorActive(int aSensorId)
        {
            return false;
        }

        public double GetSensorValue(int aSensorId)
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
