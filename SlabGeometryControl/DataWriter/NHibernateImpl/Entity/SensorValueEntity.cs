using Alvasoft.SensorValueContainer;

namespace Alvasoft.DataWriter.NHibernateImpl.Entity
{
    public class SensorValueEntity
    {
        public virtual int Id { get; set; }
        public virtual int SensorId { get; set; }
        public virtual double Value { get; set; }
        public virtual long Time { get; set; }

        public SensorValueEntity()
        {
            SensorId = -1;
            Value = 0;
            Time = 0;
        }

        public SensorValueEntity(ISensorValueInfo aValue)
        {
            SensorId = aValue.GetSensorId();
            Value = aValue.GetValue();
            Time = aValue.GetTime();
        }                
    }
}
