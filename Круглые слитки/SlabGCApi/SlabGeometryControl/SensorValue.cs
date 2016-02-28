using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Показание датчика.
    /// </summary>
    [DataContract]
    public class SensorValue
    {
        /// <summary>
        /// Значение.
        /// </summary>
        [DataMember]                
        public double Value { get; set; }

        /// <summary>
        /// Время замера.
        /// </summary>
        [DataMember]        
        public long Time { get; set; }
    }
}
