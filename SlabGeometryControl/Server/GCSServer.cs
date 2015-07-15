﻿using System;
using System.Collections.Generic;
using Alvasoft.DataProvider;
using Alvasoft.DataProvider.Impl;
using Alvasoft.DataEnums;
using Alvasoft.DataProvider.Impl.Emulator;
using Alvasoft.DataProviderConfiguration.XmlImpl;
using Alvasoft.DataWriter;
using Alvasoft.DataWriter.NHibernateImpl;
using Alvasoft.DimentionCalculator.Impl;
using Alvasoft.DimentionConfiguration;
using Alvasoft.DimentionConfiguration.NHibernateImpl;
using Alvasoft.DimentionValueContainer;
using Alvasoft.DimentionValueContainer.Impl;
using Alvasoft.SensorConfiguration.XmlImpl;
using Alvasoft.SensorValueContainer;
using Alvasoft.SensorValueContainer.Impl;
using Alvasoft.SlabBuilder.Impl;
using Alvasoft.Utils;
using Alvasoft.Utils.Activity;
using log4net;
using Alvasoft.SlabGeometryControl;
using SensorSide = Alvasoft.DataEnums.SensorSide;
using SensorType = Alvasoft.DataEnums.SensorType;
using SensorValue = Alvasoft.SlabGeometryControl.SensorValue;

namespace Alvasoft.Server
{
    public class GCSServer
        : InitializableImpl,
        IDataProviderListener,
        ISensorValueContainerListener,
        ISlabInfoWriter        
    {
        private static readonly ILog logger = LogManager.GetLogger("Server");

        private XmlDataProviderConfigurationImpl dataProviderConfiguration;
        private XmlSensorConfigurationImpl sensorConfiguration;
        private NHibernateDimentionConfigurationImpl dimentionConfiguration;

        //private OpcDataProviderImpl dataProvider;        
        private EmulatorDataProvider dataProvider;
        private SlabBuilderImpl slabBuilder;
        private DimentionCalculatorImpl dimentionCalculator;
        private ISensorValueContainer sensorValueContainer;
        private IDimentionValueContainer dimentionValueContainer;

        private NHibernateSensorValueWriter sensorValueWriter;
        private NHibernateDimentionValueWriter dimentionValueWriter;
        private NHibernateSlabInfoWriter slabWriter;
        private ISlabInfoReader slabReader = null;
        private NHibernateStandartSizeReaderWriter standartSizeReaderWriter;
        private NHibernateRegulationReaderWriter regulationsReaderWriter;

        private long startSlabScanTime;
        private long endSlabScanTime;

        private long lastSavedSensorValueTime = long.MinValue;
        private SystemState lastSystemState = SystemState.WAITING;

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");

            dimentionConfiguration = new NHibernateDimentionConfigurationImpl();
            dataProviderConfiguration = new XmlDataProviderConfigurationImpl("Settings/OpcConfiguration.xml");
            sensorConfiguration = new XmlSensorConfigurationImpl("Settings/SensorConfiguration.xml");

            //dataProvider = new OpcDataProviderImpl();            
            dataProvider = new EmulatorDataProvider();
            slabBuilder = new SlabBuilderImpl();
            dimentionCalculator = new DimentionCalculatorImpl();            
            sensorValueContainer = new SensorValueContainerImpl();
            dimentionValueContainer = new DimentionValueContainerImpl();
            standartSizeReaderWriter = new NHibernateStandartSizeReaderWriter();
            regulationsReaderWriter = new NHibernateRegulationReaderWriter();

            sensorValueWriter = new NHibernateSensorValueWriter();
            dimentionValueWriter = new NHibernateDimentionValueWriter();            
            slabWriter = new NHibernateSlabInfoWriter();
            slabReader = slabWriter as ISlabInfoReader;

            sensorValueContainer.SunbscribeContainerListener(this);                       

            dataProvider.SetSensorConfiguration(sensorConfiguration);
            dataProvider.SetSensorValueContainer(sensorValueContainer);
            dataProvider.SetDataProviderConfiguration(dataProviderConfiguration);
            dataProvider.SubscribeDataProviderListener(this);

            slabBuilder.SetCalibrator(dataProvider);
            slabBuilder.SetSensorConfiguration(sensorConfiguration);
            slabBuilder.SetSensorValueContainer(sensorValueContainer);            

            dimentionCalculator.SetDimentionConfiguration(dimentionConfiguration);
            dimentionCalculator.SetDimentionValueContainer(dimentionValueContainer);

            dimentionConfiguration.Initialize();
            standartSizeReaderWriter.Initialize();
            regulationsReaderWriter.Initialize();
            dataProviderConfiguration.Initialize();
            sensorConfiguration.Initialize();
            sensorValueWriter.Initialize();
            slabWriter.Initialize();
            dimentionValueWriter.Initialize();
            dimentionCalculator.Initialize();
            slabBuilder.Initialize();
            dataProvider.Initialize();

            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            try {                
                standartSizeReaderWriter.Uninitialize();
                regulationsReaderWriter.Uninitialize();
                dataProvider.UnsubscribeDataProviderListener(this);
                sensorValueContainer.UnsunbscribeContainerListener(this);
                dataProvider.Uninitialize();
                slabBuilder.Uninitialize();
                dimentionCalculator.Uninitialize();
                sensorValueWriter.Uninitialize();
                sensorConfiguration.Uninitialize();
                slabWriter.Uninitialize();
                dataProviderConfiguration.Uninitialize();
                dimentionConfiguration.Uninitialize();
                NHibernateHelper.CloseConnection();
            }
            catch (Exception ex) {
                logger.Info("Ошибка при деинициализации сервера: " + ex.Message);
            }
            logger.Info("Деинициализация завершена.");
        }        

        public void OnStateChanged(IDataProvider aDataProvider)
        {
            logger.Info("Уведомление о изменении состояния.");

            if (!IsInitialized()) {
                return;
            }

            try {                
                var currentSystemState = aDataProvider.GetCurrentSystemState();
                if (IsEndOfScanning(currentSystemState)) {                    
                    endSlabScanTime = DateTime.Now.ToBinary();                    
                    var slabId = GetNewSlabId();                    
                    var slabModel = slabBuilder.BuildSlabModel();
                    dimentionCalculator.CalculateDimentions(slabModel);                    
                    var dimentionValues = dimentionValueContainer.GetDimentionValues();
                    dimentionValueWriter.WriteDimentionValues(slabId, dimentionValues);                    
                }
                else { // начало сканирования.
                    startSlabScanTime = DateTime.Now.ToBinary();
                }

                lastSystemState = currentSystemState;
            }
            catch (Exception ex) {                
                logger.Info("Ошибка после завершения сканирования: " + ex.Message + "\n" +
                            "Stack trace: " + ex.StackTrace);                
            }
            finally {
                sensorValueContainer.Clear();
                dimentionValueContainer.Clear();
            }
        }        

        public void OnDataReceived(ISensorValueContainer aContainer)
        {
            logger.Info("Уведомление о поступлении данных с датчиков в контейнер.");
            try {
                var sensorValues = aContainer.GetNotReviewedValues(lastSavedSensorValueTime);
                if (sensorValues != null && sensorValues.Length > 0) {
                    logger.Info("Данных для сохранения: " + sensorValues.Length);
                    for (var i = 0; i < sensorValues.Length; ++i) {
                        sensorValueWriter.WriteSensorValueInfo(sensorValues[i]);
                        var sensorValueTime = sensorValues[i].GetTime();
                        if (lastSavedSensorValueTime < sensorValueTime) {
                            lastSavedSensorValueTime = sensorValueTime;
                        }
                    }
                    logger.Info("Данные успешно сохранены в базу.");
                }
            }
            catch (Exception ex) {
                logger.Error("Ошибка при сохранении данных с датчиков: " + ex.Message);
            }
        }

        private bool IsEndOfScanning(SystemState aCurrentSystemState)
        {
            return aCurrentSystemState == SystemState.WAITING && lastSystemState == SystemState.SCANNING;
        }

        public int GetNewSlabId()
        {
            return slabWriter.StoreNewSlab(startSlabScanTime, endSlabScanTime);
        }

        // ----------------- Реализация API --------------------------

        public ControllerConnectionState GetConnectionState()
        {
            if (!dataProvider.IsConnected()) {
                return ControllerConnectionState.DISCONNECTED;
            }

            return ControllerConnectionState.CONNECTED;            
        }

        public SGCSystemState GetSGCSystemState()
        {
            if (dataProvider.GetCurrentSystemState() == SystemState.SCANNING) {
                return SGCSystemState.SCANNING;
            }

            if (dataProvider.GetCurrentSystemState() == SystemState.WAITING) {
                return SGCSystemState.WAITING;
            }

            throw new ArgumentException("Необъявленное состояние системы.");
        }

        public DimentionResult[] GetDimentionResultsBySlabId(int aSlabId)
        {
            var dimentionsCount = dimentionConfiguration.GetDimentionInfosCount();
            var dimentionIds = new List<int>();
            for (var i = 0; i < dimentionsCount; ++i) {
                var dimention = dimentionConfiguration.GetDimentionInfoByIndex(i);
                if (dimention != null) {
                    dimentionIds.Add(dimention.GetId());
                }                
            }

            var results = new List<DimentionResult>();
            for (var i = 0; i < dimentionIds.Count; ++i) {
                var dimentionResultInfo = dimentionValueWriter.ReadDimentionValue(aSlabId, dimentionIds[i]);
                if (dimentionResultInfo != null) {
                    results.Add(new DimentionResult {
                        DimentionId = dimentionResultInfo.GetDimentionId(),
                        SlabId = dimentionResultInfo.GetSlabId(),
                        Value = dimentionResultInfo.GetValue()
                    });
                }
            }

            return results.ToArray();
        }

        public Dimention[] GetDimentions()
        {
            var dimentionsCount = dimentionConfiguration.GetDimentionInfosCount();
            var results = new List<Dimention>();
            for (var i = 0; i < dimentionsCount; ++i) {
                var dimention = dimentionConfiguration.GetDimentionInfoByIndex(i);
                if (dimention != null) {
                    results.Add(new Dimention {
                        Id = dimention.GetId(),
                        Description = dimention.GetDescription(),
                        Name = dimention.GetName()
                    });
                }
            }

            return results.ToArray();
        }

        public StandartSize[] GetStandartSizes()
        {
            var standartSizes = standartSizeReaderWriter.GetStandartSizes();
            var results = new List<StandartSize>();
            if (standartSizes != null) {
                for (var i = 0; i < standartSizes.Length; ++i) {                    
                    results.Add(new StandartSize {
                        Id = standartSizes[i].GetId(),
                        Width = standartSizes[i].GetWidth(),
                        Height = standartSizes[i].GetHeight(),
                        Length = standartSizes[i].GetLength()
                    });                    
                }
            }            

            return results.ToArray();
        }

        public Regulation[] GetRegulations()
        {
            var regulations = regulationsReaderWriter.GetRegulations();
            var results = new List<Regulation>();
            if (regulations != null) {
                for (var i = 0; i < regulations.Length; ++i) {
                    results.Add(new Regulation {
                        Id = regulations[i].GetId(),
                        DimentionId = regulations[i].GetDimentionId(),
                        StandartSizeId = regulations[i].GetStandartSizeId(),
                        MaxValue = regulations[i].GetMaxValue(),
                        MinValue = regulations[i].GetMinValue()
                    });
                }
            }

            return results.ToArray();
        }

        public SlabInfo GetSlabInfoByNumber(string aNumber)
        {
            if (string.IsNullOrEmpty(aNumber)) {
                throw new ArgumentNullException("aNumber");
            }

            var slab = slabReader.GetSlabInfo(aNumber);
            if (slab != null) {
                return new SlabInfo {
                    Id = slab.GetId(),
                    Number = slab.GetNumber(),
                    StandartSizeId = slab.GetStandartSizeId(),
                    StartScanTime = slab.GetStartScanTime(),
                    EndScanTime = slab.GetEndScanTime()
                };
            }

            throw new ArgumentException("Слиток с таким номером не найден.");
        }

        public SlabInfo[] GetSlabInfosByTimeInterval(long aFromTime, long aToTime)
        {            
            var slabs = slabReader.GetSlabInfoByTimeInterval(aFromTime, aToTime);
            var results = new List<SlabInfo>();
            foreach (var slab in slabs) {
                results.Add(new SlabInfo {
                    Id = slab.GetId(),
                    Number = slab.GetNumber(),
                    StandartSizeId = slab.GetStandartSizeId(),
                    StartScanTime = slab.GetStartScanTime(),
                    EndScanTime = slab.GetEndScanTime()
                });
            }

            return results.ToArray();
        }

        public int GetSensorsCount()
        {
            return sensorConfiguration.GetSensorInfoCount();
        }

        public SensorInfo[] GetSensorInfos()
        {
            var sensorCount = sensorConfiguration.GetSensorInfoCount();
            var results = new List<SensorInfo>();
            for (var i = 0; i < sensorCount; ++i) {
                var sensor = sensorConfiguration.ReadSensorInfoByIndex(i);
                var sensorInfo = new SensorInfo();
                sensorInfo.Id = sensor.GetId();
                sensorInfo.Name = sensor.GetName();
                sensorInfo.SensorType = (Alvasoft.SlabGeometryControl.SensorType) sensor.GetSensorType();
                sensorInfo.Side = (Alvasoft.SlabGeometryControl.SensorSide) sensor.GetSensorSide();
                sensorInfo.Shift = sensor.GetShift();
                sensorInfo.State = dataProvider.IsSensorActive(sensor.GetId())
                    ? SensorState.ACTIVE
                    : SensorState.INACTIVE;
                results.Add(sensorInfo);
            }

            return results.ToArray();
        }

        public SensorValue GetSensorValueBySensorId(int aSensorId)
        {
            return new SensorValue {
                Value = dataProvider.GetSensorValue(aSensorId),
                Time = DateTime.Now.ToBinary()
            };
        }

        public SlabPoint[] GetSlabPointsBySlabId(int aSlabId)
        {
            throw new NotImplementedException();
        }

        public SensorValue[] GetSensorValuesBySlabId(int aSlabId, int aSensorId)
        {
            var slab = slabReader.GetSlabInfo(aSlabId);
            var sensorValues = sensorValueWriter
                .ReadSensorValueInfo(aSensorId, slab.GetStartScanTime(), slab.GetEndScanTime());
            var results = new List<SensorValue>();
            if (sensorValues != null) {
                for (var i = 0; i < sensorValues.Length; ++i) {
                    results.Add(new SensorValue {
                        Value = sensorValues[i].GetValue(),
                        Time = sensorValues[i].GetTime()
                    });
                }
            }

            return results.ToArray();
        }

        public int AddStandartSize(StandartSize aStandartSize)
        {
            var standartSize = new StandartSizeImpl {
                Width = aStandartSize.Width,
                Height = aStandartSize.Height,
                Length = aStandartSize.Length
            };

            return standartSizeReaderWriter.AddStandartSize(standartSize);
        }

        public bool RemoveStandartSize(int aStandartSizeId)
        {
            try {
                standartSizeReaderWriter.RemoveStandartSize(aStandartSizeId);
                return true;
            }
            catch {
                return false;
            }
        }

        public void EditStandartSize(StandartSize aStandartSize)
        {
            var standartSize = new StandartSizeImpl {
                Id = aStandartSize.Id,
                Width = aStandartSize.Width,
                Height = aStandartSize.Height,
                Length = aStandartSize.Length
            };

            standartSizeReaderWriter.EditStandartSize(standartSize);
        }

        public int AddRegulation(Regulation aRegulation)
        {
            var regulation = new RegulationImpl {
                DimentionId = aRegulation.DimentionId,
                StandartSizeId = aRegulation.StandartSizeId,
                MaxValue = aRegulation.MaxValue,
                MinValue = aRegulation.MinValue
            };

            return regulationsReaderWriter.CreateRegulation(regulation);
        }

        public bool RemoveRegulation(int aRegulationId)
        {
            try {
                regulationsReaderWriter.RemoveRegulation(aRegulationId);
                return true;
            }
            catch {
                return false;
            }
        }

        public void EditRegulation(Regulation aRegulation)
        {
            var regulation = new RegulationImpl {
                Id = aRegulation.Id,
                DimentionId = aRegulation.DimentionId,
                StandartSizeId = aRegulation.StandartSizeId,
                MaxValue = aRegulation.MaxValue,
                MinValue = aRegulation.MinValue
            };

            regulationsReaderWriter.EditRegulation(regulation);
        }

        public int AddSensorInfo(SensorInfo aSensorInfo)
        {
            var sensorInfo = new SensorInfoImpl(sensorConfiguration.GetSensorInfoCount(), 
                aSensorInfo.Name, 
                (SensorType) aSensorInfo.SensorType, 
                (SensorSide) aSensorInfo.Side, 
                aSensorInfo.Shift);
            sensorConfiguration.CreateSensorInfo(sensorInfo);

            return sensorInfo.GetId();
        }

        public bool RemoveSensorInfo(int aSensorInfoId)
        {
            throw new NotImplementedException();
        }

        public void EditSensorInfo(SensorInfo aSensorInfo)
        {
            throw new NotImplementedException();
        }

        public bool ActivateSensor(int aSensorId)
        {
            throw new NotImplementedException();
        }

        public void AddOpcSensorTag(int aSensorId, string aTagValue, string aTagName)
        {
            throw new NotImplementedException();
        }
    }
}
