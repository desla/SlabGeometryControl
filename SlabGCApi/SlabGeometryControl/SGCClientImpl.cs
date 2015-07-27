using System;
using Alvasoft.Wcf.Client;
using Alvasoft.Wcf.NetConfiguration;
using log4net;

namespace Alvasoft.SlabGeometryControl
{
    public class SGCClientImpl : WcfClientBase<ISlabGeometryControlClient01>
    {
        private static readonly ILog logger = LogManager.GetLogger("Server");

        public const string CurrentServiceName = "Alvasoft_SlabGeometryControlSystem";

        public SGCClientImpl(INetConfiguration aConfig) : base(aConfig)
        {
            ServiceName = CurrentServiceName;            
        }

        public ControllerConnectionState GetConnectionState()
        {
            CheckConnection();
            return ServerInstance.GetConnectionState(GetSessionId());
        }

        public SGCSystemState GetSGCSystemState()
        {
            CheckConnection();
            return ServerInstance.GetSGCSystemState(GetSessionId());
        }

        public DimentionResult[] GetDimentionResultsBySlabId(int aSlabId)
        {
            CheckConnection();
            return ServerInstance.GetDimentionResultsBySlabId(GetSessionId(), aSlabId);
        }

        public Dimention[] GetDimentions()
        {
            CheckConnection();
            return ServerInstance.GetDimentions(GetSessionId());
        }

        public StandartSize[] GetStandartSizes()
        {
            CheckConnection();
            return ServerInstance.GetStandartSizes(GetSessionId());
        }

        public Regulation[] GetRegulations()
        {
            CheckConnection();
            return ServerInstance.GetRegulations(GetSessionId());
        }

        public SlabInfo GetSlabInfoByNumber(string aNumber)
        {
            CheckConnection();
            return ServerInstance.GetSlabInfoByNumber(GetSessionId(), aNumber);
        }

        public SlabInfo[] GetSlabInfosByTimeInterval(long aFromTime, long aToTime)
        {
            CheckConnection();
            return ServerInstance.GetSlabInfosByTimeInterval(GetSessionId(), aFromTime, aToTime);
        }

        public int GetSensorsCount()
        {
            CheckConnection();
            return ServerInstance.GetSensorsCount(GetSessionId());
        }

        public SensorInfo[] GetSensorInfos()
        {
            CheckConnection();
            return ServerInstance.GetSensorInfos(GetSessionId());
        }

        public SensorValue GetSensorValueBySensorId(int aSensorId)
        {
            CheckConnection();
            return ServerInstance.GetSensorValueBySensorId(GetSessionId(), aSensorId);
        }

        public SlabModel3D GetSlabModel3DBySlabId(int aSlabId)
        {
            CheckConnection();
            return ServerInstance.GetSlabModel3DBySlabId(GetSessionId(), aSlabId);
        }

        public SensorValue[] GetSensorValuesBySlabId(int aSlabId, int aSensorId)
        {
            CheckConnection();
            return ServerInstance.GetSensorValuesBySlabId(GetSessionId(), aSlabId, aSensorId);
        }

        public int AddStandartSize(StandartSize aStandartSize)
        {
            CheckConnection();
            return ServerInstance.AddStandartSize(GetSessionId(), aStandartSize);
        }

        public bool RemoveStandartSize(int aStandartSizeId)
        {
            CheckConnection();
            return ServerInstance.RemoveStandartSize(GetSessionId(), aStandartSizeId);
        }

        public void EditStandartSize(StandartSize aStandartSize)
        {
            CheckConnection();
            ServerInstance.EditStandartSize(GetSessionId(), aStandartSize);
        }

        public int AddRegulation(Regulation aRegulation)
        {
            CheckConnection();
            return ServerInstance.AddRegulation(GetSessionId(), aRegulation);
        }

        public bool RemoveRegulation(int aRegulationId)
        {
            CheckConnection();
            return ServerInstance.RemoveRegulation(GetSessionId(), aRegulationId);
        }

        public void EditRegulation(Regulation aRegulation)
        {
            CheckConnection();
            ServerInstance.EditRegulation(GetSessionId(), aRegulation);
        }

        public int AddSensorInfo(SensorInfo aSensorInfo)
        {
            CheckConnection();
            return ServerInstance.AddSensorInfo(GetSessionId(), aSensorInfo);
        }

        public bool RemoveSensorInfo(int aSensorInfoId)
        {
            CheckConnection();
            return ServerInstance.RemoveSensorInfo(GetSessionId(), aSensorInfoId);
        }

        public void EditSensorInfo(SensorInfo aSensorInfo)
        {
            CheckConnection();
            ServerInstance.EditSensorInfo(GetSessionId(), aSensorInfo);
        }

        public bool ActivateSensor(int aSensorId)
        {
            CheckConnection();
            return ServerInstance.ActivateSensor(GetSessionId(), aSensorId);
        }

        public void AddOpcSensorTag(int aSensorId, string aTagValue, string aTagName)
        {
            CheckConnection();
            ServerInstance.AddOpcSensorTag(GetSessionId(), aSensorId, aTagValue, aTagName);
        }

        protected override void LogInfo(string aInfoMessage)
        {
            logger.Info(aInfoMessage);
        }

        protected override void LogError(string aErrorMessage)
        {
            logger.Error(aErrorMessage);
        }

        protected override void LogDebug(string aDebugMessage)
        {
            logger.Debug(aDebugMessage);
        }
    }
}
