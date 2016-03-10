using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Alvasoft.DataEnums;
using Alvasoft.DataProviderConfiguration;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;
using Alvasoft.Utils.Activity;
using REmulatorConfiguration;
using RoundSlabEmulator;
using Timer = System.Timers.Timer;

namespace Alvasoft.DataProvider.Impl.Emulator {
    public class EmulatorDataProvider :
        InitializableImpl,
        IDataProvider,
        ICalibrator,
        IDataProviderConfigurationListener,
        ISensorConfigurationListener {

        private RSlabEmulator emulator = new RSlabEmulator();
        private EmulatorConfiguration configuration;

        private SystemState currentState = SystemState.WAITING;
        private int position = 0;
        private ISensorValueContainer valueContainer;
        private List<IDataProviderListener> listeners = new List<IDataProviderListener>();

        private Timer scanTimer;        

        public EmulatorDataProvider() {            
            configuration = EmulatorConfiguration.Deserialize("Settings/Emulator.xml");
            emulator.setConfiguration(configuration);

            scanTimer = new Timer();
            scanTimer.Elapsed += StartScan;
            scanTimer.Interval = 10000;
            scanTimer.Start();
            //StartScan(null, null);            
        }        

        private void StartScan(object sender, ElapsedEventArgs e) {
            scanTimer.Stop();
            if (currentState == SystemState.SCANNING) {
                return;
            }
            currentState = SystemState.SCANNING;
            emulator.GenerateSensorValues();
            position = 0;
            foreach (var listener in listeners) {
                listener.OnStateChanged(this);
            }

            var values = new List<double[]>();
            foreach (var sensor in configuration.Frame.Sensors) {
                values.Add(emulator.GetValues(sensor.Id));
            }
            var sensors = configuration.Frame.Sensors;            

            for (var i = 0; i < values[0].Length; ++i) {
                var nowTime = DateTime.Now.ToBinary();
                for (var j = 0; j < sensors.Length; ++j) {
                    valueContainer.AddSensorValue(sensors[j].Id, values[j][i], nowTime);
                }
                Thread.Sleep(15000 / values[0].Length);
                position = i;
            }

            currentState = SystemState.WAITING;
            position = 0;
            foreach (var listener in listeners) {
                listener.OnStateChanged(this);
            }
            scanTimer.Start();
        }


        /// <summary>     
        /// </summary>
        /// <param name="aSensorId"></param>
        /// <returns></returns>
        public double GetCalibratedValue(int aSensorId) {
            return configuration.Frame.Size;
        }


        public SystemState GetCurrentSystemState() {
            return currentState;
        }

        public double GetSensorCurrentValue(int aSensorId) {
            if (currentState == SystemState.WAITING) {
                return 0;
            }
            var values = emulator.GetValues(aSensorId);
            return values[position];
        }

        public bool IsCalibratedState() {
            return true;
        }

        public bool IsConnected() {
            return true;
        }

        public bool IsSensorActive(int aSensorId) {
            return true;
        }

        public void OnSensorCreated(ISensorConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void OnSensorCreated(IDataProviderConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void OnSensorDeleted(ISensorConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void OnSensorDeleted(IDataProviderConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void OnSensorUpdated(ISensorConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void OnSensorUpdated(IDataProviderConfiguration aConfiguration, int aSensorId) {
            throw new NotImplementedException();
        }

        public void SetCalibratedValue(int aSensorId, double aCalibratedValue) {
            throw new NotImplementedException();
        }

        public void SetDataProviderConfiguration(IDataProviderConfiguration aDataProviderConfiguration) {            
        }

        public void SetSensorConfiguration(ISensorConfiguration aSensorConfiguration) {            
        }

        public void SetSensorValueContainer(ISensorValueContainer aSensorValueContainer) {
            valueContainer = aSensorValueContainer;            
        }

        public void SubscribeDataProviderListener(IDataProviderListener aListener) {
            listeners.Add(aListener);
        }

        public void UnsubscribeDataProviderListener(IDataProviderListener aListener) {
            listeners.Remove(aListener);
        }
    }
}
