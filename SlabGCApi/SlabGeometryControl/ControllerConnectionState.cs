using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Состояние связи с контроллером.
    /// </summary>
    [DataContract]
    public enum ControllerConnectionState
    {
        /// <summary>
        /// Соединение активно.
        /// </summary>
        [EnumMember]
        CONNECTED,
        /// <summary>
        /// Соединение отсутствует.
        /// </summary>
        [EnumMember]
        DISCONNECTED
    }
}
