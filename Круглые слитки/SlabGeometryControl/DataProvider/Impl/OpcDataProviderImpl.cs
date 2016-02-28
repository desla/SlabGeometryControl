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
    using System.Threading;

    public class OpcDataProviderImpl : 
        InitializableImpl,
        IDataProvider,        
        IDataProviderConfigurationListener,
        ISensorConfigurationListener,
        IActivatorListener, 
        IOpcValueListener
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
        private bool isScanning = false;
        private int lastDataBlock;
        private object lastDataBlockLock = new object();

        private int positionSensorId = -1;
        /// <summary>
        /// Очередь индексов блоков данных для чтения.
        /// </summary>
        private Queue<int> dataBlockQueue = new Queue<int>();

        /// <summary>
        /// Поток для чтения блоков данных из OPC контроллера.
        /// Индексы блоков данных для чтения он берет из очереди dataBlockQueue. 
        /// </summary>
        private Thread opcReadingThread;// = new Thread(DataBlockReading);        

        public SystemState GetCurrentSystemState()
        {
            return isScanning ? SystemState.SCANNING : SystemState.WAITING;            
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

            try {
                InitializeControlBlock();
                InitializeOpcSensors();
                InitializeActivator();
            }
            catch (Exception ex) {
                logger.Error("Ошибка при инициализации: " + ex.Message);
            }

            lastDataBlock = controlBlock.DataBlocksCount - 1;

            opcReadingThread = new Thread(DataBlockReading);
            opcReadingThread.Start();
            logger.Info("Инициализация завершена.");
        }        

        protected override void DoUninitialize()
        {
            isScanning = false;
            activator.Uninitialize();
            controlBlock.Uninitialize();
            foreach (var sensor in sensors.Values) {
                sensor.Uninitialize();
            }
            server.Disconnect();

            opcReadingThread.Abort();            
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
            controlBlock.EndIndex = new OpcValueImpl(server, controlBlockInfo.EndIndexTag, 
                OPCDataSource.OPCDevice, 25);            
            controlBlock.TimeSyncActivator = new OpcValueImpl(server, controlBlockInfo.DateTimeSyncActivatorTag);
            controlBlock.TimeForSync = new OpcValueImpl(server, controlBlockInfo.DateTimeForSyncTag);
            controlBlock.ResetToZeroItem = new OpcValueImpl(server, controlBlockInfo.ResetToZeroTag);
            controlBlock.DataBlockSize = controlBlockInfo.DataBlockSize;
            controlBlock.DataBlocksCount = controlBlockInfo.DataBlocksCount;
            controlBlock.Times = new OpcValueImpl[controlBlockInfo.DataBlocksCount];
            for (var i = 0; i < controlBlockInfo.DataBlocksCount; ++i) {
                controlBlock.Times[i] = new OpcValueImpl(server, controlBlockInfo.TimesTags[i], OPCDataSource.OPCDevice);            
            }

            controlBlock.EndIndex.IsListenValueChanging = true;
            controlBlock.EndIndex.SubscribeToValueChange(this);

            controlBlock.Initialize();
            foreach (var timeBlock in controlBlock.Times) {
                timeBlock.WriteValue(new int[100]);
            }
            controlBlock.StartIndex.WriteValue(0);
            controlBlock.EndIndex.WriteValue(0);            
            controlBlock.TimeForSync.WriteValue(DateTime.Now);
            controlBlock.TimeSyncActivator.WriteValue(true);
            controlBlock.ResetToZeroItem.WriteValue(true);
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
                sensor.dataBlockSize = controlBlock.DataBlockSize;
                sensor.Enable = new OpcValueImpl(server, sensorInfo.EnableTag);                
                sensor.CurrentValue = new OpcValueImpl(server, sensorInfo.CurrentValueTag);
                sensor.DataBlocks = new OpcValueImpl[controlBlock.DataBlocksCount];
                for (var j = 0; j < controlBlock.DataBlocksCount; ++j) {
                    sensor.DataBlocks[j] = 
                        new OpcValueImpl(server, sensorInfo.DataBlocksTags[j], OPCDataSource.OPCDevice);
                }                
                sensor.Initialize();

                sensors[sensorId] = sensor;

                var sensorConfig = sensorConfiguration.ReadSensorInfoByIndex(i);
                if (sensorConfig.GetSensorType() == SensorType.POSITION) {
                    positionSensorId = sensorId;
                }
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
            isScanning = false;
            logger.Info("Сканирование завершено. Слушатели не оповещены.");
            // Для того, чтобы в очередь попали данные из последнего блока данных.
            var rightIndex = Convert.ToInt32(controlBlock.EndIndex.ReadCurrentValue());            
            AddPreviosDataBlockToQueue(rightIndex + controlBlock.DataBlockSize);

            logger.Info("Ожидаем окончания чтения данных...");
            while (dataBlockQueue.Count > 0) {
                Thread.Sleep(100);
            }
            logger.Info("Данные полностью прочитаны.");        
            try {
                foreach (var listener in listeners) {
                    listener.OnStateChanged(this);
                }
            }
            catch (Exception ex) {
                logger.Info("Ошибка в callback'е: " + ex.Message);
            }

            try {                                
                Console.WriteLine("Обнуление данных контроллера.");
                controlBlock.StartIndex.WriteValue(0);
                controlBlock.EndIndex.WriteValue(0);
                lastDataBlock = controlBlock.DataBlocksCount - 1;
                controlBlock.ResetToZeroItem.WriteValue(true);
                //foreach (var opcSensor in sensors.Values) {
                //    opcSensor.ResetValues();
                //}
            }
            catch (Exception ex) {
                logger.Error("Ошибка при обнулении значений после сканирования: " + ex.Message);
            }            
        }

        private void StartScanning()
        {
            startScanning = DateTime.Now;
            isScanning = true;
            foreach (var listener in listeners) {
                listener.OnStateChanged(this);
            }
        }

        /// <summary>
        /// Колбэк на правую границу массива.        
        /// </summary>
        /// <param name="aOpcValue"></param>
        /// <param name="aValueChangedEventArgs"></param>
        public void OnValueChanged(IOpcValue aOpcValue, OpcValueChangedEventArgs aValueChangedEventArgs)
        {            
            if (!isScanning) {
                Console.WriteLine("FAIL " + aValueChangedEventArgs.Value);
                return;
            }

            var rightIndex = Convert.ToInt32(aValueChangedEventArgs.Value);
            AddPreviosDataBlockToQueue(rightIndex);
        }


        /// <summary>
        /// Если граница перешла за значение блока данных, он добавляется в очередь для чтения.
        /// </summary>
        /// <param name="aRightIndex"></param>
        private void AddPreviosDataBlockToQueue(int aRightIndex)
        {
            var currentDataBlock = aRightIndex / controlBlock.DataBlockSize;
            var previosDataBlock = currentDataBlock > 0 ?
                                            currentDataBlock - 1 :
                                            controlBlock.DataBlocksCount - 1;
            lock (lastDataBlockLock) {
                // Если предыдущий блок уже был помещен в очередь,
                // то ничего делать не надо.
                if (lastDataBlock == previosDataBlock) {
                    return;
                }

                lastDataBlock = previosDataBlock;
            }

            lock (dataBlockQueue) {
                logger.Info("Добавляем блок для чтения " + previosDataBlock);
                dataBlockQueue.Enqueue(previosDataBlock);
            }
        }

        /// <summary>
        /// Метод, который постоянно работает и читает блоки данных из opc контроллера.
        /// </summary>
        private void DataBlockReading()
        {

            while (true) {
                if (dataBlockQueue.Count == 0) {
                    Thread.Sleep(100);
                }
                else {
                    var leftIndex = Convert.ToInt32(controlBlock.StartIndex.ReadCurrentValue());
                    var rightIndex = Convert.ToInt32(controlBlock.EndIndex.ReadCurrentValue());
                    var dataBlock = -1;
                    lock (dataBlockQueue) {
                        dataBlock = dataBlockQueue.Peek();
                    }
                    
                    // Если левая граница не в блоке, то это непонятно!
                    if (leftIndex < dataBlock*controlBlock.DataBlockSize ||
                        leftIndex >= dataBlock * controlBlock.DataBlockSize + controlBlock.DataBlockSize) {
                        logger.Error(string.Format(
                            "Левая граница {0} лежит не в блоке для чтения {1}.", leftIndex, dataBlock));
                    }

                    var fromIndex = Math.Max(leftIndex, dataBlock*controlBlock.DataBlockSize);
                    var toIndex = Math.Min(rightIndex - 1, (dataBlock + 1)*controlBlock.DataBlockSize - 1);
                    if (toIndex < fromIndex) {
                        toIndex = (dataBlock + 1) * controlBlock.DataBlockSize - 1;
                    }

                    // теперь есть fromIndex и toIndex.
                    try {
                        logger.Info("Читаем блок " + dataBlock + " от " + fromIndex + " до " + toIndex);
                        ReadDataBlock(
                            dataBlock, 
                            fromIndex - dataBlock * controlBlock.DataBlockSize, 
                            toIndex - dataBlock * controlBlock.DataBlockSize);
                    }
                    catch (Exception ex) {
                        logger.Error("Ошибка при чтении блока данных " + dataBlock + ": " + ex.Message);
                    }

                    try {                        
                        var nextPosition = dataBlock != controlBlock.DataBlocksCount - 1
                            ? toIndex + 1
                            : 0;
                        logger.Info("Сдвигаем границу в позицию " + nextPosition);
                        controlBlock.StartIndex.WriteValue(nextPosition);                        
                    }
                    catch (Exception ex) {
                        logger.Error("Ошибка при записи границы.");
                    }

                    lock (dataBlockQueue) {
                        dataBlockQueue.Dequeue();
                    }
                }
            }
        }

        private void ReadDataBlock(int aDataBlock, int aFromIndex, int aToIndex)
        {                        
            var times = controlBlock.Times[aDataBlock].ReadCurrentValue() as Array;            
            var longTimes = new long[times.Length];            
            for (var i = 0; i < times.Length; ++i) {
                var milliseconds = Convert.ToDouble(times.GetValue(i));
                var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
                var fullTime = startScanning.Date.AddMilliseconds(timeSpan.TotalMilliseconds);
                longTimes[i] = fullTime.ToBinary();
            }

            var positionSensor = sensors[positionSensorId];
            var positionValues = positionSensor.DataBlocks[aDataBlock].ReadCurrentValue() as Array;            

            foreach (var sensorId in sensors.Keys) {
                var sensor = sensors[sensorId];                                
                var values = sensor.DataBlocks[aDataBlock].ReadCurrentValue() as Array;
                var currentIndex = aFromIndex;
                var ignored = 0;
                var previousPosition = 0.0;
                while (currentIndex <= aToIndex) {
                    var currentPosition = Convert.ToDouble(positionValues.GetValue(currentIndex));
                    if (currentIndex == 0 ||
                        Math.Abs(currentPosition - previousPosition) > 0.001) {
                        previousPosition = currentPosition;
                        var value = Convert.ToDouble(values.GetValue(currentIndex));
                        valueContainer.AddSensorValue(sensorId, value, longTimes[currentIndex]);
                    }
                    else {
                        ignored++;
                    }
                    currentIndex++;
                }
                Console.WriteLine("Игнорировано: " + ignored);
            }   
        }
    }
}
