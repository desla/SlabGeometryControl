using System;
using System.Collections.Generic;
using Alvasoft.DataProviderConfiguration;
using Alvasoft.DataEnums;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;
using Alvasoft.Utils.Activity;
using log4net;
using OpcTagAccessProvider;
using OPCAutomation;

namespace Alvasoft.DataProvider.Impl
{
    public class OpcDataProviderImpl : 
        InitializableImpl,
        IDataProvider,        
        IDataProviderConfigurationListener,
        ISensorConfigurationListener,
        IActivatorListener
    {
        private static readonly ILog logger = LogManager.GetLogger("OpcDataProviderImpl");

        private ISensorValueContainer valueContainer;
        private IDataProviderConfiguration opcConfiguration;
        private ISensorConfiguration sensorConfiguration;
        private List<IDataProviderListener> listeners = new List<IDataProviderListener>();        
        private OPCServer server;
        private DataProviderActivatorImpl activator;
        private ControlBlock controlBlock;
        private Dictionary<int, OpcSensor> sensors = new Dictionary<int, OpcSensor>();
        private DateTime startScanning;
        private bool isFirstRun = true;

        public SystemState GetCurrentSystemState()
        {
            return activator.GetCurrentValue() ? 
                SystemState.SCANNING : SystemState.WAITING;
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
            return server != null && server.ServerState == (int) OPCServerState.OPCRunning;
        }

        public bool IsSensorActive(int aSensorId)
        {
            if (!sensors.ContainsKey(aSensorId)) {
                //throw new ArgumentException("Нет датчика с таким идентификатором: " + aSensorId);
                return false;
            }

            var isSensorEnable = Convert.ToBoolean(sensors[aSensorId].Enable.ReadCurrentValue());
            return isSensorEnable;
        }

        public double GetSensorCurrentValue(int aSensorId)
        {
            return Convert.ToDouble(sensors[aSensorId].CurrentValue.ReadCurrentValue());
        }

        public bool IsCalibratedState()
        {
            return true;
        }

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");
            server = new OPCServer();
            var serverName = opcConfiguration.GetServer();
            var serverHost = opcConfiguration.GetHost();
            server.Connect(serverName, serverHost);

            InitializeOpcSensors();
            InitializeControlBlock();            
            InitializeActivator();
            
            logger.Info("Инициализация завершена.");
        }        

        protected override void DoUninitialize()
        {
            activator.Uninitialize();
            controlBlock.Uninitialize();
            foreach (var sensor in sensors.Values) {
                sensor.Uninitialize();
            }
            server.Disconnect();
        }

        public void OnActivationTagValueChanged(bool aCurrentValue)
        {
            try {
                if (isFirstRun) {
                    isFirstRun = false;
                    return;
                }

                if (aCurrentValue) {
                    StartScanning();
                }
                else {
                    EndScanning();
                }
            }
            catch (Exception ex) {
                logger.Info("Ошибка при изменении состояния: " + ex.Message);
            }
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

        private void InitializeControlBlock()
        {
            var controlBlockInfo = opcConfiguration.GetControlBlock();
            controlBlock = new ControlBlock();
            controlBlock.MaxSize = new OpcValueImpl(server, controlBlockInfo.DataMaxSizeTag);
            controlBlock.StartIndex = new OpcValueImpl(server, controlBlockInfo.StartIndexTag);
            controlBlock.EndIndex = new OpcValueImpl(server, controlBlockInfo.EndIndexTag);
            controlBlock.Times = new OpcValueImpl(server, controlBlockInfo.TimesTag);
            controlBlock.TimeSyncActivator = new OpcValueImpl(server, controlBlockInfo.DateTimeSyncActivatorTag);
            controlBlock.TimeForSync = new OpcValueImpl(server, controlBlockInfo.DateTimeForSyncTag);
            controlBlock.Initialize();
            controlBlock.StartIndex.WriteValue(0);
            controlBlock.EndIndex.WriteValue(0);
            controlBlock.TimeForSync.WriteValue(DateTime.Now);
            controlBlock.TimeSyncActivator.WriteValue(true);
        }

        private void InitializeActivator()
        {
            activator = new DataProviderActivatorImpl();
            activator.SetOpcServer(server);
            activator.SetActivationTagName(opcConfiguration.GetControlBlock().ActivationTag);
            activator.SetActivatorListener(this);
            activator.Initialize();
        }

        private void InitializeOpcSensors()
        {
            var sensorsCount = opcConfiguration.GetOpcSensorInfoCount();
            for (var i = 0; i < sensorsCount; ++i) {
                var sensorInfo = opcConfiguration.ReadOpcSensorInfoByIndex(i);
                var sensorId = GetSensorId(sensorInfo);
                if (sensors.ContainsKey(sensorId)) {
                    throw new ArgumentException("OPC несколько датчиков с одинаковыми идентификаторами.");
                }

                var sensor = new OpcSensor();                
                sensor.Enable = new OpcValueImpl(server, sensorInfo.EnableTag);                
                sensor.CurrentValue = new OpcValueImpl(server, sensorInfo.CurrentValueTag);
                sensor.ValuesList = new OpcValueImpl(server, sensorInfo.ValuesListTag);
                sensor.Initialize();

                sensors[sensorId] = sensor;
            }
        }

        private int GetSensorId(IOpcSensorInfo aSensorInfo)
        {
            var sensorInfo = sensorConfiguration.ReadSensorInfoByName(aSensorInfo.Name);
            if (sensorInfo == null) {
                throw new ArgumentException("Настройки OPC: Датчика с именем " + aSensorInfo.Name + " " +
                                            "не найдено в настройках приложения.");
            }

            return sensorInfo.GetId();
        }

        private void EndScanning()
        {            
            var left = Convert.ToInt32(controlBlock.StartIndex.ReadCurrentValue()) + 5;
            var right = Convert.ToInt32(controlBlock.EndIndex.ReadCurrentValue()) - 50;
            if (right < 0)
                right += 40;
            if (left >= right) 
                left -= 5;

            var masSize = Convert.ToInt32(controlBlock.MaxSize.ReadCurrentValue());
            var times = controlBlock.Times.ReadCurrentValue() as Array;
            foreach (var sensorId in sensors.Keys) {
                var sensor = sensors[sensorId];
                var values = sensor.ValuesList.ReadCurrentValue() as Array;
                var currentIndex = left;
                while (currentIndex != right) {
                    if (currentIndex > masSize) {
                        currentIndex = 0;
                    }
                    var value = Convert.ToDouble(values.GetValue(currentIndex));
                    var milliseconds = Convert.ToDouble(times.GetValue(currentIndex));
                    var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
                    var fullTime = startScanning.Date.AddMilliseconds(timeSpan.TotalMilliseconds);
                    valueContainer.AddSensorValue(sensorId, value, fullTime.ToBinary());
                    currentIndex++;
                }
            }

            var slabLength = 1000.0;
            var count = right - left;
            var step = slabLength/(count - 1);
            var t1 = 0;
            while (left != right) {
                if (left > masSize) {
                    left = 0;
                }
                var milliseconds = Convert.ToDouble(times.GetValue(left + 1));
                var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
                var fullTime = startScanning.Date.AddMilliseconds(timeSpan.TotalMilliseconds);
                valueContainer.AddSensorValue(4, t1*step, fullTime.ToBinary());
                t1++;
                left++;
            }           

            controlBlock.StartIndex.WriteValue(0);
            controlBlock.EndIndex.WriteValue(0);

            foreach (var listener in listeners) {
                listener.OnStateChanged(this);
            }
        }

        private void StartScanning()
        {
            startScanning = DateTime.Now;
            foreach (var listener in listeners) {
                listener.OnStateChanged(this);
            }
        }        
    }
}
