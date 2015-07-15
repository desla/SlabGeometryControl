using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Тип датчика.
    /// </summary>
    [DataContract]
    public enum SensorType
    {
        /// <summary>
        /// Датчик положения.
        /// </summary>
        [EnumMember]
        POSITION,
        /// <summary>
        /// Датчик расстояния.
        /// </summary>
        [EnumMember]
        PROXIMITY
    }
}
