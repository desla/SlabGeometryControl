using Alvasoft.SensorValueContainer;

namespace Alvasoft.DataWriter
{
    public class SensorValueImpl : ISensorValueInfo
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public long Time { get; set; }

        public int GetSensorId()
        {
            return Id;
        }

        public double GetValue()
        {
            return Value;
        }

        public long GetTime()
        {
            return Time;
        }
    }
}
