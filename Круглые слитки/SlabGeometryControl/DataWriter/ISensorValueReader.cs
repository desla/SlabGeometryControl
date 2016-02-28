using Alvasoft.SensorValueContainer;

namespace Alvasoft.DataWriter
{
    public interface ISensorValueReader
    {
        ISensorValueInfo[] ReadSensorValueInfo(int aSensorId, long aFromTime, long aToTime);
    }
}
