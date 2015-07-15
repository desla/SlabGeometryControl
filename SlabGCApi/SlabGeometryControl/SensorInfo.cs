using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Описание датчика в системе.
    /// </summary>
    [DataContract]                
    public class SensorInfo
    {
        /// <summary>
        /// Идентификатор датчика.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Название датчика.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Сторона датчика.
        /// </summary>
        [DataMember]
        public SensorSide Side { get; set; }

        /// <summary>
        /// Отступ датчика от центра рамки.
        /// </summary>
        [DataMember]
        public double Shift { get; set; }

        /// <summary>
        /// Тип датчика.
        /// </summary>
        [DataMember]
        public SensorType SensorType { get; set; }

        /// <summary>
        /// Состояние датчика.
        /// </summary>
        [DataMember]
        public SensorState State { get; set; }
    }
}
