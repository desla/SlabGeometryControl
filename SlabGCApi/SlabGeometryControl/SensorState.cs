using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Состояние датчика.
    /// </summary>
    [DataContract]
    public enum SensorState
    {
        /// <summary>
        /// Активный.
        /// </summary>
        [EnumMember]
        ACTIVE,
        /// <summary>
        /// Неактивный.
        /// </summary>
        [EnumMember]
        INACTIVE
    }
}
