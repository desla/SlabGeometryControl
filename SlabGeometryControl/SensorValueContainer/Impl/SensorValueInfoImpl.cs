namespace Alvasoft.SensorValueContainer.Impl
{
    public class SensorValueInfoImpl : 
        ISensorValueInfo
    {
        private int sensorId;        
        private double value;
        private long time;

        public SensorValueInfoImpl(int aSensorId, double aValue, long aTime)
        {
            sensorId = aSensorId;
            value = aValue;
            time = aTime;
        }

        public int GetSensorId()
        {
            return sensorId;
        }

        public double GetValue()
        {
            return value;
        }

        public long GetTime()
        {
            return time;
        }

        public override string ToString()
        {
            return string.Format("SensorId:{0};Value:{1};Time:{2};", sensorId, value, time);
        }
    }
}
