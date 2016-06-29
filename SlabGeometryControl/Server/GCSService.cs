using System;
using Alvasoft.Wcf.NetConfiguration;
using Alvasoft.Wcf.Server;
using log4net;
using Alvasoft.SlabGeometryControl;

namespace Alvasoft.Server
{    
    public class GCSService : WcfServerBase, ISlabGeometryControlClient01, IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger("GCSController");

        private GCSServer gcsServer = new GCSServer();

        public GCSService(INetConfiguration aConfig) : base(aConfig)
        {
            ContractType = typeof (ISlabGeometryControlClient01);
            ServiceName = "Alvasoft_SlabGeometryControlSystem";
            IsHttpGetEnabled = true;
            gcsServer.Initialize();                        
        }        

        protected override void LogInfo(string aInfoMessage)
        {
            logger.Info(aInfoMessage);
        }

        public ControllerConnectionState GetConnectionState(long aSessionId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetConnectionState");
                return gcsServer.GetConnectionState();
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return ControllerConnectionState.DISCONNECTED;
            }
        }

        public SGCSystemState GetSGCSystemState(long aSessionId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSGCSystemState");
                return gcsServer.GetSGCSystemState();
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return SGCSystemState.WAITING;
            }            
        }

        public DimentionResult[] GetDimentionResultsBySlabId(long aSessionId, int aSlabId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetDimentionResultsBySlabId(" + aSlabId + ")");
                return gcsServer.GetDimentionResultsBySlabId(aSlabId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }             
        }

        public Dimention[] GetDimentions(long aSessionId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetDimentions");
                return gcsServer.GetDimentions();
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }              
        }

        public StandartSize[] GetStandartSizes(long aSessionId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetStandartSizes");
                return gcsServer.GetStandartSizes();
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            } 
        }

        public Regulation[] GetRegulations(long aSessionId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetRegulations");
                return gcsServer.GetRegulations();
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }             
        }

        public SlabInfo GetSlabInfoByNumber(long aSessionId, string aNumber)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSlabInfoByNumber(" + aNumber + ")");
                return gcsServer.GetSlabInfoByNumber(aNumber);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }            
        }

        public SlabInfo[] GetSlabInfosByTimeInterval(long aSessionId, long aFromTime, long aToTime)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSlabInfosByTimeInterval(" + 
                             DateTime.FromBinary(aFromTime) + ", " + 
                             DateTime.FromBinary(aToTime) + ")");
                var slabInfos = gcsServer.GetSlabInfosByTimeInterval(aFromTime, aToTime);
                logger.Debug("Слитков найдено: " + slabInfos.Length);
                return slabInfos;
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }            
        }

        public int GetSensorsCount(long aSessionId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSensorsCount");
                return gcsServer.GetSensorsCount();
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return 0;
            }            
        }

        public SensorInfo[] GetSensorInfos(long aSessionId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSensorInfos");
                return gcsServer.GetSensorInfos();
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }               
        }

        public SensorValue GetSensorValueBySensorId(long aSessionId, int aSensorId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSensorValueBySensorId(" + aSensorId + ")");                
                return gcsServer.GetSensorValueBySensorId(aSensorId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }            
        }

        public SlabModel3D GetSlabModel3DBySlabId(long aSessionId, int aSlabId, bool aIsUseFilters)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSlabPointsBySlabId(" + aSlabId + ")");
                return gcsServer.GetSlabModel3DBySlabId(aSlabId, aIsUseFilters);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            } 
        }

        public SensorValue[] GetSensorValuesBySlabId(long aSessionId, int aSlabId, int aSensorId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetSensorValuesBySlabId(" + aSlabId + ", " + aSensorId + ")");
                return gcsServer.GetSensorValuesBySlabId(aSlabId, aSensorId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return null;
            }            
        }

        public int AddStandartSize(long aSessionId, StandartSize aStandartSize)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос AddStandartSize(" + aStandartSize + ")");
                return gcsServer.AddStandartSize(aStandartSize);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return -1;
            }            
        }

        public bool RemoveStandartSize(long aSessionId, int aStandartSizeId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос RemoveStandartSize(" + aStandartSizeId + ")");
                return gcsServer.RemoveStandartSize(aStandartSizeId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return false;
            }
        }

        public void EditStandartSize(long aSessionId, StandartSize aStandartSize)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос EditStandartSize(" + aStandartSize + ")");
                gcsServer.EditStandartSize(aStandartSize);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);                
            }            
        }

        public int AddRegulation(long aSessionId, Regulation aRegulation)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос AddRegulation(RegulationId:" + aRegulation.Id + ")");
                return gcsServer.AddRegulation(aRegulation);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return -1;
            }            
        }

        public bool RemoveRegulation(long aSessionId, int aRegulationId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос AddRegulation(" + aRegulationId + ")");
                return gcsServer.RemoveRegulation(aRegulationId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return false;
            }            
        }

        public void EditRegulation(long aSessionId, Regulation aRegulation)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос AddRegulation(RegulationId:" + aRegulation.Id + ")");
                gcsServer.EditRegulation(aRegulation);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);                
            }            
        }

        public int AddSensorInfo(long aSessionId, SensorInfo aSensorInfo)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос AddSensorInfo");
                return gcsServer.AddSensorInfo(aSensorInfo);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return -1;
            }             
        }

        public bool RemoveSensorInfo(long aSessionId, int aSensorInfoId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос RemoveSensorInfo(" + aSensorInfoId + ")");
                return gcsServer.RemoveSensorInfo(aSensorInfoId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return false;
            }            
        }

        public void EditSensorInfo(long aSessionId, SensorInfo aSensorInfo)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос EditSensorInfo(" + aSensorInfo.Id + ")");
                gcsServer.EditSensorInfo(aSensorInfo);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);             
            }            
        }

        public bool ActivateSensor(long aSessionId, int aSensorId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос ActivateSensor(" + aSensorId + ")");
                return gcsServer.ActivateSensor(aSensorId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return false;
            }            
        }

        public void AddOpcSensorTag(long aSessionId, int aSensorId, string aTagValue, string aTagName)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос AddOpcSensorTag(" + aTagName + ", " + aTagValue + ")");
                gcsServer.AddOpcSensorTag(aSensorId, aTagName, aTagValue);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);                
            }            
        }

        public void SetCalibratedValue(long aSessionId, int aSensorId, double aCalibratedValue)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос SetCalibratedValue(" + aSensorId + ", " + aCalibratedValue + ")");
                gcsServer.SerCalibratedValue(aSensorId, aCalibratedValue);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSensorId + " Ошибка: " + ex.Message);
            }
        }

        public string GetRecalculatedValuesString(long aSessionId, int aSlabId)
        {
            try {
                logger.Debug("Сессия: " + aSessionId + " " +
                             "Запрос GetRecalculatedValuesString(" + aSlabId + ")");
                return gcsServer.GetRecalculatedValuesString(aSlabId);
            }
            catch (Exception ex) {
                logger.Error("Сессия: " + aSessionId + " Ошибка: " + ex.Message);
                return "Во время пересчета произошла ошибка: " + ex.Message;
            } 
        }

        public void Dispose()
        {
            if (gcsServer.IsInitialized()) {
                gcsServer.Uninitialize();                
                gcsServer = null;
            }            
        }
    }
}
